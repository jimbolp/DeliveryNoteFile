using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Footer
    {
        public decimal? TotalDiscounts { get; set; }
        public decimal? TotalWithDiscountNoVAT { get; set; }
        public decimal? InvoiceTotal { get; set; }
        public DateTime? DueDate { get; set; }

        public Footer(string line)
        {
            decimal decimalValue = 0;
            DateTime dateValue;
            if(Decimal.TryParse(line.Substring(Settings.Default.TotalDiscountsStart, Settings.Default.TotalDiscountsLength), out decimalValue))
            {
                TotalDiscounts = decimalValue;
            }
            if(Decimal.TryParse(line.Substring(Settings.Default.TotalWithDiscountNoVATStart, Settings.Default.TotalWithDiscountNoVATLength), out decimalValue))
            {
                TotalWithDiscountNoVAT = decimalValue;
            }
            if (Decimal.TryParse(line.Substring(Settings.Default.InvoiceTotalStart, Settings.Default.InvoiceTotalLength), out decimalValue))
            {
                InvoiceTotal = decimalValue;
            }
            if(DateTime.TryParse(line.Substring(Settings.Default.DueDateStart, Settings.Default.DueDateLength), out dateValue))
            {
                DueDate = dateValue;
            }
        }
    }
}
