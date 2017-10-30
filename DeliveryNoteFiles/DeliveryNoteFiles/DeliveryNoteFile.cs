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
        public Mail Mail { get; set; }
        public Bank Bank { get; set; }
        public Tour Tour { get; set; }
        public List<Position> Positions { get; set; }
        public Footer Footer { get; set; }
        public VATTable VATTable { get; set; }
        public byte PaymentTimeID
        {
            get
            {
                if(Tour.TourDate.HasValue && Footer.DueDate.HasValue)
                {
                    byte id = (byte)(Tour.TourDate.Value.Date < Footer.DueDate.Value.Date ? 2 : 1);
                    return id;
                }
                return 1;
            }
        }

        public string FileName { get; set; }

        private List<string> suppLines = new List<string>();
        private List<string> custLines = new List<string>();
        private List<string> headLines = new List<string>();

        private bool _isSupplierProcessing = false;
        private bool _isHeaderProcessing = false;
        private bool _isCustomerProcessing = false;

        
        private bool IsSupplierProcessing {
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

        private bool IsHeaderProcessing
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
        private bool IsCustomerProcessing
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

        private BitArray havePos = new BitArray(new bool[6]);       //Using Array for two reasons.. 1. Less variables(less memory :D) 2. The array indexes coincides with the "position's line numbers"
                                                                    //Example: POS == 0; POS1 == 1; etc.
        private List<string> Lines = new List<string>();

        //debuging purposes...
        //Work
        private string processedFilesPath = Settings.Default.SaveFilesPath;

        //Home
        //private string processedFilesPath = @"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\Moved Files";

        public DeliveryNoteFile(string filePath)
        {
            try
            {
                FileName = Path.GetFileName(filePath);
            }
            catch(Exception e)
            {
                throw e; 
            }
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
            string[] posLines = new string[6];                  //Reason to use array: The array indexes coincides with the "position's line numbers"

            //
            foreach (var line in lines)
            {
                try
                {
                    if (line.StartsWith("$$TYPE$$"))
                    {
                        try
                        {
                            DocType = new Type(line);
                        }
                        catch(Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$SUPPLIER$$"))
                    {
                        if (!IsSupplierProcessing)
                            IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }

                    else if (line.StartsWith("$$SUPPLIER2$$"))
                    {
                        if (!IsSupplierProcessing)
                            IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }

                    else if (line.StartsWith("$$SUPPLIER3$$"))
                    {
                        if (!IsSupplierProcessing)
                            IsSupplierProcessing = true;
                        suppLines.Add(line);
                    }

                    else if (line.StartsWith("$$HEADER$$"))
                    {
                        if (!IsHeaderProcessing)
                            IsHeaderProcessing = true;
                        headLines.Add(line);
                    }

                    else if (line.StartsWith("$$HEADER2$$"))
                    {
                        if (!IsHeaderProcessing)
                            IsHeaderProcessing = true;                        
                        headLines.Add(line);
                    }

                    else if (line.StartsWith("$$CUSTOMER$$"))
                    {
                        if (!IsCustomerProcessing)
                            IsCustomerProcessing = true;
                        custLines.Add(line);
                    }

                    else if (line.StartsWith("$$CUSTOMER2$$"))
                    {
                        if(!IsCustomerProcessing)
                            IsCustomerProcessing = true;                        
                        custLines.Add(line);
                    }

                    else if (line.StartsWith("$$MAIL$$") || line.StartsWith("$$DISC$$") || line.StartsWith("$$BLPD$$"))
                    {
                        try
                        {
                            Mail = new Mail(line, DocType.isCreditNote);
                        }
                        catch(Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$TOUR$$"))
                    {
                        try
                        {
                            Tour = new Tour(line, DocType.isCreditNote);
                        }
                        catch(Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$BANK$$"))
                    {
                        try
                        {
                            Bank = new Bank(line, DocType.isCreditNote);
                        }
                        catch (Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$FOOTER$$"))
                    {                        
                        if (!string.IsNullOrEmpty(posLines[0]))
                        {
                            ProcessPosition(posLines, DocType.isCreditNote);
                            posLines = null;
                        }
                        try
                        {
                            Footer = new Footer(line, DocType.isCreditNote);
                        }
                        catch (Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$MWST$$"))
                    {
                        try
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
                        catch (Exception e)
                        {
                            WriteExceptionToLog(e);
                        }
                    }

                    else if (line.StartsWith("$$POS$$"))
                    {
                        if (!string.IsNullOrEmpty(posLines[0]))
                        {
                            try
                            {
                                ProcessPosition(posLines, DocType.isCreditNote);
                            }
                            catch (Exception e)
                            {
                                WriteExceptionToLog(e);
                            }
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

                    else if (line.StartsWith("$$POS5$$"))
                    {
                        posLines[5] = line;
                        havePos[5] = true;
                    }

                    else
                    {
                        if(!(string.IsNullOrEmpty(line.Trim()) || string.IsNullOrWhiteSpace(line.Trim())))
                        {
                            //Console.WriteLine(line);
                        }
                    }

                    IsCustomerProcessing = false;
                    IsHeaderProcessing = false;
                    IsSupplierProcessing = false;
                }
                catch (NotImplementedException nie)
                {
                    WriteExceptionToLog(nie);
                }
                catch(Exception e)
                {
                    WriteExceptionToLog(e);
                }
            }
        }

        private void WriteExceptionToLog(Exception e)
        {
            File.AppendAllText(Settings.Default.LogFilePath,
                DateTime.Now + Environment.NewLine + e.ToString() + Environment.NewLine);
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
                if (current.ArticleNo == last.ArticleNo)
                {
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
