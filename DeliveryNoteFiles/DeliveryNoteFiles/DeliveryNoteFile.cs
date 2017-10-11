using System;
using System.Collections.Generic;
using System.IO;
using DelNoteItems;
using Type = DelNoteItems.Type;

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
                using (StreamReader file = new StreamReader(filePath))
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
            bool havePos1 = false;
            bool havePos2 = false;
            bool havePos3 = false;
            bool havePos4 = false;

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
            }
        }
    }
}
