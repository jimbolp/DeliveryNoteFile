
using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Customer : DelNoteItems
    {
        //$$CUSTOMER$$ Line properties
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerCIP { get; set; }                    //Post Code
        public string CustomerCity { get; set; }
        public string CustomerUIN { get; set; }                 //Bulstat
        public string CustomerLicenceNumber { get; set; }
        public string CustomerNarcLicenceNumber { get; set; }
        public string CustomerAccountablePerson { get; set; }   //МОЛ
        public long CustomerPhoneNo { get; set; }

        public Customer(string line, bool isCreditNote)
        {
            try
            {
                if (isCreditNote)
                {
                    InitializeCreditNote(line);
                }
                else
                {
                    InitializeInvoice(line);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitializeInvoice(string line)
        {
            switch (line)
            {
                case "$$CUSTOMER$$":
                    Line1(line);
                    break;
                case "$$CUSTOMER2$$":
                    Line2(line);
                    break;
            }            
        }

        

        private void InitializeCreditNote(string line)
        {
            //FixLine();
            InitializeInvoice(line);
        }
    }
}
