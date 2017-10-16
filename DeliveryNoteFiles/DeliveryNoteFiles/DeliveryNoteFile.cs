using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DelNoteItems;
using Type = DelNoteItems.Type;
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

        private List<string> Lines = new List<string>();

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
            }catch(Exception)
            {
                //TODO...
            }
            InitializeComponents(Lines.ToArray());
        }

        /// <summary>
        /// Initializes each Property with the information from the file.
        /// </summary>
        /// <param name="lines"></param>
        private void InitializeComponents(string[] lines)
        {
            BitArray havePos = new BitArray(new bool[5]);
            //Position lastPosition;
            string[] posLines = new string[5];
            
            foreach (var line in lines)
            {
                if (line.StartsWith("$$TYPE$$"))
                {
                    DocType = new Type(line);
                }
                else if (line.StartsWith("$$SUPPLIER$$"))
                {
                    Supplier = new Supplier(line);
                }
                else if (line.StartsWith("$$HEADER$$"))
                {
                    Header = new Header(line);
                }
                else if (line.StartsWith("$$FOOTER$$"))
                {
                    if (!string.IsNullOrEmpty(posLines[0]))
                    {
                        ProcessPosition(posLines);
                        posLines = null;
                    }
                    Footer = new Footer(line);
                }
                else if (line.StartsWith("$$MWST$$"))
                {
                    if(VATTable == null)
                    {
                        VATTable = new VATTable(line);
                    }
                    else
                    {
                        VATTable.Table.Add(new MWST(line));
                    }
                }
                else if (line.StartsWith("$$POS$$"))
                {
                    if (!string.IsNullOrEmpty(posLines[0]))
                    {
                        ProcessPosition(posLines);
                        havePos.SetAll(false);
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
        }

        private void ProcessPosition(string[] lines)
        {
            if (Positions == null)
            {
                Positions = new List<Position>() { new Position(lines) };
                return;
            }
            Position current = new Position(lines);
            Position last;
            try
            {
                last = Positions.LastOrDefault();
            }
            catch (ArgumentNullException)
            {
                last = null;
            }

            if(current.InvoicedQty == 0 || current.InvoicedQty == null)
            {
                if(last != null)
                {

                }
            }
            Positions.Add(current);
        }

        private void FixRebatePosition()
        {

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
        //private int BitCount(BitArray array)
        //{
        //    int count = 0;
        //    foreach (bool bit in array)
        //    {
        //        if (bit)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}
    }
}
