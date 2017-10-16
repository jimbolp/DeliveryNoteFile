using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Config;

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

            //TotalDiscounts
            if (line.Length >= Settings.Default.TotalDiscountsStart + Settings.Default.TotalDiscountsLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.TotalDiscountsStart, Settings.Default.TotalDiscountsLength).Trim().Replace(',', '.'), out decimalValue))
                {
                    TotalDiscounts = decimalValue;
                }
            }
            else if(line.Length >= Settings.Default.TotalDiscountsStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.TotalDiscountsStart).Trim().Replace(',', '.'), out decimalValue))
                {
                    TotalDiscounts = decimalValue;
                }
            }

            //TotalWithDiscountNoVAT
            if (line.Length >= Settings.Default.TotalWithDiscountNoVATStart + Settings.Default.TotalWithDiscountNoVATLength)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.TotalWithDiscountNoVATStart, Settings.Default.TotalWithDiscountNoVATLength).Trim().Replace(',', '.'), out decimalValue))
                {
                    TotalWithDiscountNoVAT = decimalValue;
                }
            }
            else if(line.Length >= Settings.Default.TotalWithDiscountNoVATStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.TotalWithDiscountNoVATStart).Trim().Replace(',', '.'), out decimalValue))
                {
                    TotalWithDiscountNoVAT = decimalValue;
                }
            }

            //InvoiceTotal
            if (line.Length >= Settings.Default.InvoiceTotalStart + Settings.Default.InvoiceTotalLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.InvoiceTotalStart, Settings.Default.InvoiceTotalLength).Trim().Replace(',', '.'), out decimalValue))
                {
                    InvoiceTotal = decimalValue;
                }
            }
            else if(line.Length >= Settings.Default.InvoiceTotalStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.InvoiceTotalStart).Trim().Replace(',', '.'), out decimalValue))
                {
                    InvoiceTotal = decimalValue;
                }
            }

            //DueDate
            if (line.Length >= Settings.Default.DueDateStart + Settings.Default.DueDateLength)
            {
                DueDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.DueDateStart, Settings.Default.DueDateLength).Trim());
            }
            else if(line.Length >= Settings.Default.DueDateStart)
            {
                DueDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.DueDateStart).Trim());
            }
        }

#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                if (!pi.GetType().IsAssignableFrom(typeof(IEnumerable)))
                {
                    toString += pi.Name + " -> ";
                    try
                    {
                        toString += pi.GetValue(this).ToString();
                    }
                    catch (Exception) { }
                    toString += Environment.NewLine;
                }
            }
            return toString;
        } 
#endif
    }
}
