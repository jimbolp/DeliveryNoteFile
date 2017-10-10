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

        private List<string> Lines = new List<string>();

        public DeliveryNoteFile(string filePath)
        {
            ReadFile(filePath);
        }

        /// <summary>
        /// Reads each line of the file asynchronous and initialize a List of strings "Lines"
        /// </summary>
        /// <param name="filePath"></param>
        private async void ReadFile(string filePath)
        {
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    string line;
                    while ((line = await file.ReadLineAsync()) != null)
                    {
                        Lines.Add(line);
                    }
                }
            }catch(Exception e)
            {
                //TODO...
            }
            InitializeComponents(Lines.ToArray());
        }

        /// <summary>
        /// Initializes each Property according to the information from the file.. Duuhhh.. What COULD go wrong WILL go wrong!!!
        /// </summary>
        /// <param name="lines"></param>
        private void InitializeComponents(string[] lines)
        {
            //throw new NotImplementedException();
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
            }
        }
    }
}
