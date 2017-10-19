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
        public List<Position> Positions { get; set; } = new List<Position>();
        public Footer Footer { get; set; }
        public VATTable VATTable { get; set; }

        private BitArray havePos = new BitArray(new bool[5]);       //Using Array for two reasons.. 1. Less variables(less memory :D) 2. The array indexes coincides with the "position's line numbers"
                                                                    //Example: POS == 0; POS1 == 1; etc.
        private List<string> Lines = new List<string>();
        private string processedFilesPath = Settings.Default.SaveFilesPath;

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
                using (StreamReader file = new StreamReader(filePath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        Lines.Add(line);
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
            catch (Exception)
            {
                Console.WriteLine($"Problem moving file: {filePath}!");
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
                File.Move(currentFilePath, processedFilesPath);
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
                        Supplier = new Supplier(line, DocType.isCreditNote);
                    }
                    else if (line.StartsWith("$$HEADER$$"))
                    {
                        Header = new Header(line, DocType.isCreditNote);
                    }
                    else if (line.StartsWith("$$FOOTER$$"))
                    {
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
            //If the list of positions is NULL there is nothing to check.
            //Initializing the List, and adding the first position in it.
            if (Positions == null)
            {
                Positions = new List<Position>() { new Position(lines, isCreditNote) };
                return;
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

            Position last = Positions.LastOrDefault();
            
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

#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType))
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
                    foreach (var val in pi.GetValue(this) as IEnumerable)
                    {
                        toString += val.ToString();
                    }
                }
            }
            return toString;
        } 
#endif
    }
}
