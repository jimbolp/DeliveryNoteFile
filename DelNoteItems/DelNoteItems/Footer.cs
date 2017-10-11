using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Footer
    {
        public decimal? TotalDiscounts { get; set; }            //TotalDiscounts
        public decimal? TotalWithDiscountNoVAT { get; set; }    //TotalWithDiscountNoVAT
        public decimal? InvoiceTotal { get; set; }              //InvoiceTotal
        public DateTime? DueDate { get; set; }                  //DueDate

        public Footer(string line)
        {
            decimal decimalValue = 0;
            //DateTime dateValue;

            if((line.Length >= Settings.Default.TotalDiscountsStart + Settings.Default.TotalDiscountsLength) 
                && Decimal.TryParse(line.Substring(Settings.Default.TotalDiscountsStart, Settings.Default.TotalDiscountsLength).Trim().Replace(',', '.'), out decimalValue))
            {
                TotalDiscounts = decimalValue;
            }

            if((line.Length >= Settings.Default.TotalWithDiscountNoVATStart + Settings.Default.TotalWithDiscountNoVATLength)
                && Decimal.TryParse(line.Substring(Settings.Default.TotalWithDiscountNoVATStart, Settings.Default.TotalWithDiscountNoVATLength).Trim().Replace(',', '.'), out decimalValue))
            {
                TotalWithDiscountNoVAT = decimalValue;
            }

            if ((line.Length >= Settings.Default.InvoiceTotalStart + Settings.Default.InvoiceTotalLength)
                && Decimal.TryParse(line.Substring(Settings.Default.InvoiceTotalStart, Settings.Default.InvoiceTotalLength).Trim().Replace(',', '.'), out decimalValue))
            {
                InvoiceTotal = decimalValue;
            }
            if(line.Length >= Settings.Default.DueDateStart + Settings.Default.DueDateLength)
            {
                DueDate = DateID.Convert(line.Substring(Settings.Default.DueDateStart, Settings.Default.DueDateLength).Trim().Replace(',','.'));
            }
        }
    }
}
