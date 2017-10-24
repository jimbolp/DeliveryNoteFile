using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DelNoteItems;
using Type = DelNoteItems.Type;
using Settings = DelNoteItems.Properties.Settings;
using System.Reflection;

namespace DeliveryNoteFiles
{
    class DeliveryNoteFile
    {
        public Type DocType { get; set; }
        public Supplier Supplier { get; set; }
        public Header Header { get; set; }
        public Customer Customer { get; set; }
        public List<Position> Positions { get; set; }
        public Footer Footer { get; set; }
        public VATTable VATTable { get; set; }

        private List<string> suppLines = new List<string>();
        private List<string> custLines = new List<string>();
        private List<string> headLines = new List<string>();

        private bool _isSupplierProcessing = false;
        private bool _isHeaderProcessing = false;
        private bool _isCustomerProcessing = false;

        //will try to fire an event here!
        public bool IsSupplierProcessing {
            get
            {
                return _isSupplierProcessing;
            }
            set
            {
                if (_isSupplierProcessing && !value)
                {
                    _isSupplierProcessing = value;
                    ProcessSupplier(suppLines);
                }
                else
                {
                    _isSupplierProcessing = value;
                }
            }
        }        

        public bool IsHeaderProcessing
        {
            get
            {
                return _isHeaderProcessing;
            }
            set
            {
                if(_isHeaderProcessing && !value)
                {
                    _isHeaderProcessing = value;
                    ProcessHeader(headLines);
                }
                else
                {
                    _isHeaderProcessing = value;
                }
            }
        }
        public bool IsCustomerProcessing
        {
            get
            {
                return _isCustomerProcessing;
            }
            set
            {
                if (_isCustomerProcessing && !value)
                {
                    _isCustomerProcessing = value;
                    ProcessCustomer(custLines);
                }
                else
                {
                    _isCustomerProcessing = value;
                }
            }
        }

        private BitArray havePos = new BitArray(new bool[5]);       //Using Array for two reasons.. 1. Less variables(less memory :D) 2. The array indexes coincides with the "position's line numbers"
                                                                    //Example: POS == 0; POS1 == 1; etc.
        private List<string> Lines = new List<string>();

        //debuging purposes...
        //Work
        private string processedFilesPath = Settings.Default.SaveFilesPath;

        //Home
        //private string processedFilesPath = @"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\Moved Files";

        public DeliveryNoteFile(string filePath)
        {
            ReadFile(filePath);
        }

        /// <summary>
        /// Reads each line of the file and initialize a List of strings "Lines"
        /// </summary>
        /// <param name="filePath"></param>
        private void ReadFile(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader file = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            Lines.Add(line);
                        }
                    }
                }
                InitializeComponents(Lines.ToArray());
            }
            catch(Exception)
            {
                throw;
            }
            string movedFilePath = null;
            try
            {
                movedFilePath = MoveFile(filePath, processedFilesPath);
                SendEmail(movedFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem moving file: {filePath}!");
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
            
        }

        private void SendEmail(string movedFilePath)
        {
            
        }
        private string MoveFile(string currentFilePath, string processedFilesPath)
        {
            processedFilesPath += "\\" + Path.GetFileName(currentFilePath);
            try
            {
                //File.Copy(currentFilePath, processedFilesPath);
                //File.Move(currentFilePath, processedFilesPath);
            }
            catch (Exception)
            {
                throw;
            }
            return processedFilesPath;
        }

        /// <summary>
        /// Initializes each Property with the information from the file.
        /// </summary>
        /// <param name="lines"></param>
        private void InitializeComponents(string[] lines)
        {
            string[] posLines = new string[5];                  //Reason to use array: The array indexes coincides with the "position's line numbers"

            //
            foreach (var line in lines)
            {
                try
                {
                    if (line.StartsWith("$$TYPE$$"))
                    {
                        DocType = new Type(line);
                    }
                    else if (line.StartsWith("$$SUPPLIER$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }
                    else if (line.StartsWith("$$SUPPLIER2$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }
                    else if (line.StartsWith("$$SUPPLIER3$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }
                    else if (line.StartsWith("$$HEADER$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = true;
                        IsSupplierProcessing = false;
                        headLines.Add(line);
                    }
                    else if (line.StartsWith("$$HEADER2$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = true;
                        IsSupplierProcessing = false;
                        headLines.Add(line);
                    }
                    else if (line.StartsWith("$$CUSTOMER$$"))
                    {
                        IsCustomerProcessing = true;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = false;
                        custLines.Add(line);
                    }
                    else if (line.StartsWith("$$CUSTOMER2$$"))
                    {
                        IsCustomerProcessing = true;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = false;
                        custLines.Add(line);
                    }
                    else if (line.StartsWith("$$FOOTER$$"))
                    {
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = false;
                        if (!string.IsNullOrEmpty(posLines[0]))
                        {
                            ProcessPosition(posLines, DocType.isCreditNote);
                            posLines = null;
                        }
                        Footer = new Footer(line, DocType.isCreditNote);
                    }
                    else if (line.StartsWith("$$MWST$$"))
                    {
                        if (VATTable == null)
                        {
                            VATTable = new VATTable(line, DocType.isCreditNote);
                        }
                        else
                        {
                            VATTable.Table.Add(new MWST(line, DocType.isCreditNote));
                        }
                    }
                    else if (line.StartsWith("$$POS$$"))
                    {
                        if (!string.IsNullOrEmpty(posLines[0]))
                        {
                            ProcessPosition(posLines, DocType.isCreditNote);
                        }
                        else
                        {
                            IsCustomerProcessing = false;
                            IsHeaderProcessing = false;
                            IsSupplierProcessing = false;
                        }
                        posLines[0] = line;
                        havePos[0] = true;
                    }
                    else if (line.StartsWith("$$POS1$"))
                    {
                        posLines[1] = line;
                        havePos[1] = true;
                    }
                    else if (line.StartsWith("$$POS2$$"))
                    {
                        posLines[2] = line;
                        havePos[2] = true;
                    }
                    else if (line.StartsWith("$$POS3$"))
                    {
                        posLines[3] = line;
                        havePos[3] = true;
                    }
                    else if (line.StartsWith("$$POS4$$"))
                    {
                        posLines[4] = line;
                        havePos[4] = true;
                    }
                }
                catch (NotImplementedException)
                {

                }
            }
        }

        private void ProcessPosition(string[] lines, bool isCreditNote)
        {            
            if (Positions == null)
            {
                Positions = new List<Position>();
            }
            
            Position current;
            try
            {
               current = new Position(lines, isCreditNote);
            }
            catch (Exception)
            {
                throw;
            }

            Position last = null;
            if(Positions.Count != 0)
                last = Positions.LastOrDefault();
            if(last == null)
            {
                //Console.WriteLine("Все още няма въведена позиция!");
                //Console.ReadLine();
            }

            bool testWriteL = false;
            if (havePos[1])
            {
                if (current.InvoicedQty == 0 || current.InvoicedQty == null)
                {
                    if (current.DeliveryQty == 0 || current.DeliveryQty == null)
                    {
                        current.InvoicedQty = current.BonusQty ?? 0;
                    }
                    else
                    {
                        current.InvoicedQty = current.DeliveryQty;
                    }
                }
            }
            else
            {
#if DEBUG
                if (current.InvoicedQty != current.DeliveryQty)
                {
                    if (last.ArticleNo == current.ArticleNo)
                    {
                        testWriteL = true;
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, "Before correction:" + Environment.NewLine);
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50));
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, last.ToString() + Environment.NewLine);
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, current.ToString() + Environment.NewLine);
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50) + Environment.NewLine);
                    }
                }
#endif
                if (current.ArticleNo == last.ArticleNo)
                {
#if DEBUG
                    if (last.OrderQty != last.DeliveryQty)
                    {
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, "Different DeliveryQty:" + Environment.NewLine);
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50));
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, last.ToString() + Environment.NewLine);
                        File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50) + Environment.NewLine);
                    }
#endif
                    if (last.DeliveryQty == (last.InvoicedQty + current.InvoicedQty))
                    {
                        current.DeliveryQty -= last.InvoicedQty;
                        last.DeliveryQty -= current.InvoicedQty;
                    }

                    if(last.BonusQty == (last.InvoicedQty + current.InvoicedQty))
                    {
                        current.BonusQty -= last.InvoicedQty;
                        last.BonusQty -= current.InvoicedQty;
                    }
                }
            }
            Positions.Add(current);
            havePos.SetAll(false);
#if DEBUG
            if (testWriteL)
            {
                File.AppendAllText(Settings.Default.ChangedPosFilePath, "Corrected:" + Environment.NewLine);
                File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50));
                File.AppendAllText(Settings.Default.ChangedPosFilePath, last.ToString() + Environment.NewLine);
                File.AppendAllText(Settings.Default.ChangedPosFilePath, current.ToString() + Environment.NewLine);
                File.AppendAllText(Settings.Default.ChangedPosFilePath, new string('-', 50) + Environment.NewLine);
            }
#endif
        }

        private void FixRebatePosition()
        {
            throw new NotImplementedException();
        }

        private void ProcessSupplier(List<string> lines)
        {
            Supplier = new Supplier(lines.ToArray(), DocType.isCreditNote);
        }

        private void ProcessCustomer(List<string> custLines)
        {
            Customer = new Customer(custLines.ToArray(), DocType.isCreditNote);
        }

        private void ProcessHeader(List<string> headLines)
        {
            Header = new Header(headLines.ToArray(), DocType.isCreditNote);
        }


#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) || pi.PropertyType == typeof(string))
                {
                    try
                    {
                        toString += pi.GetValue(this).ToString();
                        toString += Environment.NewLine;
                    }
                    catch (Exception) { }
                }
                else
                {
                    if (pi.GetValue(this) != null)
                    {
                        foreach (var val in pi.GetValue(this) as IEnumerable)
                        {
                            toString += val.ToString();
                        }
                    }
                }
            }
            return toString;
        } 
#endif
    }
}
