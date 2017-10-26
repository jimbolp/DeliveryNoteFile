using System;

namespace DelNoteItems
{
    public partial class Supplier : DelNoteItems
    {
        //$$SUPPLIER$$ Line properties

        //According to Mr. Rolf Raab, these are constant values for Bulgaria and I don't know what they represent and how to change them.
        private long? notKnownNumber { get; set; }          //100000266
        private string notKnownString { get; set; }         //LIBRA AG
        private long? notKnownLongerNumber { get; set; }    //8606004100001

        public int? BranchNumber { get; set; }

        //$$SUPPLIER2$$ Line properties (Mask PA12, PA16 in PHARMOS)
        public string BranchName { get; set; }          //PA12
        public string BranchAddress { get; set; }       //PA12
        public int? BranchCIP { get; set; }             //Post Code (PA12)
        public string BranchCity { get; set; }          //PA12
        public string BranchUIN { get; set; }           //Bulstat I guess (PA16)

        //$$SUPPLIER3$$ Line properties (Mask PA96 in PHARMOS)
        public string BranchLicenceNumber { get; set; }
        public string BranchNarcLicenceNumber { get; set; }
        public string BranchResponsible { get; set; }           //Branch Responsible Pharmacist 

        public Supplier(string[] lines, bool isCreditNote)
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
            catch (Exception)
            {
                throw;
            }            
        }

        private void InitializeInvoice(string line)
        {
            if (line.StartsWith("$$SUPPLIER$$"))
            {
                Line1(line);
            }
            else if (line.StartsWith("$$SUPPLIER2$$"))
            {
                Line2(line);
            }
            else if (line.StartsWith("$$SUPPLIER3$$"))
            {
                Line3(line);
            }
            
        }
    
        private void InitializeCreditNote(string line)
        {
            //FixLine(line);
            InitializeInvoice(line);
        }
    }
}
