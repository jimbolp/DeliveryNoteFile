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
        #region Properties
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

        private bool IsSupplierProcessing
        {
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

        private BitArray havePos = new BitArray(new bool[6]);       //Reason to use array: The array indexes coincides with the "position's line numbers"
                                                                    //Example: POS == 0; POS1 == 1; etc.

        string[] posLines = new string[6];                          //Reason to use array: The same."

        //private List<string> Lines = new List<string>();

        #endregion

        //debuging purposes...
        //Work
        private string processedFilesPath = Settings.Default.SaveFilesPath;

        //Home
        //private string processedFilesPath = @"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\Moved Files";

        /// <summary>
        /// Returns fully initialized object with the information from the file
        /// </summary>
        /// <param name="filePath"></param>
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
        /// Reads each line of the file, initializes a List of strings "Lines" and passes it to InitializeComponents()
        /// </summary>
        /// <param name="filePath"></param>
        private void ReadFile(string filePath)
        {
            try
            {
                //Open the file in Read-Only mode in case the service is still writing in it
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //If the Default Encoding is not specified explicitly, the encoding is broken!
                    using (StreamReader file = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            //Lines.Add(line);
                            InitializeComponents(line);
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }    
        }
        
        /// <summary>
        /// Initializes each Property with the information from the file.
        /// </summary>
        /// <param name="line"></param>
        private void InitializeComponents(string line)
        {
            //Sometimes there are empty lines. I see no problem in that. Just continue
            if (string.IsNullOrEmpty(line.Trim()) || string.IsNullOrWhiteSpace(line.Trim()))
                return;

            try
            {
                #region Type
                //Two options for now. Either an Invoice or a Credit Note.
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
                #endregion

                #region Supplier
                //There are a couple of lines that contains the Supplier info. It's mandatory that they are consecutive.
                //A flag is raised when we start reading those lines and when flag is set back to false again, we initialize the Supplier with the whole information
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
                #endregion Supplier

                #region Header
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
                #endregion Header

                #region Customer
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
                #endregion Customer

                #region Mail
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
                #endregion

                #region Tour
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
                #endregion

                #region Bank
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
                #endregion

                #region Footer
                //The Footer is a mandatory line. 
                //When the Footer line is received, we know that there are no more positions, so we process the last position and then, the Footer
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
                    finally
                    {
                        //Set the flags to false
                        IsCustomerProcessing = false;
                        IsHeaderProcessing = false;
                        IsSupplierProcessing = false;
                    }
                }
                #endregion Footer

                #region VAT
                else if (line.StartsWith("$$MWST$$"))
                {
                    try
                    {
                        //In the VAT Table we could have more than one VAT percentage for different reasons.
                        //If the table is empty, we create it and add the current info to it as MWST object
                        if (VATTable == null)
                        {
                            VATTable = new VATTable(line, DocType.isCreditNote);
                        }
                        //Otherwise just add another VAT to the table
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
                #endregion VAT

                #region Positions
                //Each position starts with this line. For each POS line received, the previous position is initialized. Except when it's the first one...  obviously! :D
                //The last position from the file is initialized when we receive the Footer line
                else if (line.StartsWith("$$POS$$"))
                {
                    //If the first index of the posLines is null or empty, this should be the first position
                    //Otherwise process the previous one
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
                        //On the first received position, set those flags to false.
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
                //I think, this line is only received on Credit Notes.
                else if (line.StartsWith("$$POS5$$"))
                {
                    posLines[5] = line;
                    havePos[5] = true;
                }
                #endregion Positions

                //The unkown lines are ignored
                    
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
        
        /// <summary>
        /// Validates each position by checking if it is a rebate position and if it has the right quantities.
        /// If necessary, fix the quantities and add to the List
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="isCreditNote"></param>
        private void ProcessPosition(string[] lines, bool isCreditNote)
        {            
            if (Positions == null)
                Positions = new List<Position>();
            
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

            //If there is no previous position, there should be nothing to fix. Just add the current position to the list
            //We shouldn't receive rebate at the first position
            if(last == null)
            {
                Positions.Add(current);
                havePos.SetAll(false);
                return;
            }

            //The quantities are received on POS1 line
            if (havePos[1])
            {
                //if Invoiced quantity is 0 we set it to be as the Delivery or as the Bonus quantity. Last case scenario is 0
                if (current.InvoicedQty == 0 || current.InvoicedQty == null)
                {
                    if (current.DeliveryQty == 0 || current.DeliveryQty == null)
                    {
                        current.InvoicedQty = current.BonusQty ?? 0; //Last case scenario
                    }
                    else
                    {
                        current.InvoicedQty = current.DeliveryQty;
                    }
                }
            }

            //If POS1 line is not received for the current position, it's a rebate/bonus to the previous position
            else
            {
                if (current.ArticleNo == last.ArticleNo)
                {
                    if (last.DeliveryQty == (last.InvoicedQty + current.InvoicedQty))
                    {
                        current.DeliveryQty -= last.InvoicedQty;
                        last.DeliveryQty -= current.InvoicedQty;
                    }

                    if (last.BonusQty == (last.InvoicedQty + current.InvoicedQty))
                    {
                        current.BonusQty -= last.InvoicedQty;
                        last.BonusQty -= current.InvoicedQty;
                    }
                }
                //improbable case: 
                //If there are no quantities received for the current line and it's not the same article as the previous position, just do nothing. 
                //Add the position as it's received (with no quantities)
                else { }
            }
            Positions.Add(current);
            havePos.SetAll(false);
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

        public static void WriteExceptionToLog(Exception e)
        {
            File.AppendAllText(Settings.Default.LogFilePath,
                DateTime.Now + Environment.NewLine + e.ToString() + Environment.NewLine);
        }
        public static void WriteExceptionToLog(string e)
        {
            File.AppendAllText(Settings.Default.LogFilePath,
                DateTime.Now + Environment.NewLine + e + Environment.NewLine);
        }

        //Only when debugging
        #region ToString
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
                        toString += Environment.NewLine;
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
        #endregion
    }
}
