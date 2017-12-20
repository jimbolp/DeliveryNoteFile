
using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Customer : DelNoteItems
    {
        //$$CUSTOMER$$ Line properties
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int? CustomerCIP { get; set; }                    //Post Code
        public string CustomerCity { get; set; }
        public string CustomerUIN { get; set; }                 //Bulstat
        public string CustomerLicenceNumber { get; set; }
        public string CustomerNarcLicenceNumber { get; set; }
        public string CustomerAccountablePerson { get; set; }   //МОЛ
        public long? CustomerPhoneNo { get; set; }

        public Customer(string[] lines, bool isCreditNote)
        {
            try
            {
                if (isCreditNote)
                {
                    foreach (string line in lines)
                    {
                        InitializeCreditNote(line);
                    }
                }
                else
                {
                    foreach (string line in lines)
                    {
                        InitializeInvoice(line);
                    }
                }
            }
            catch (Exception e)
            {
                WriteExceptionToLog(e);
                throw e;
            }
        }

        private void InitializeInvoice(string line)
        {
            if (string.IsNullOrEmpty(line))
                return;
            if (line.StartsWith("$$CUSTOMER$$"))
            {
                Line1(line);
            }
            if (line.StartsWith("$$CUSTOMER2$$"))
            {
                Line2(line);
            }
        }
    

        private void InitializeCreditNote(string line)
        {
            //FixLine();
            InitializeInvoice(line);
        }
    }
}
