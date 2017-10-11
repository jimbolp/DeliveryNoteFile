using Settings = DelNoteItems.Properties.Settings1;
using System.Collections.Generic;
using System;

namespace DelNoteItems
{
    /// <summary>
    /// MwSt is the abbreviation for "Mehrwertsteuer", which is also known as "Umsatzsteuer" (USt). This is the equivalent to VAT in the UK.
    /// </summary>
    public class MWST
    {
        public decimal? VATPercentage { get; set; }     //OrderVATPercentage
        public decimal? TotalVAT { get; set; }

        public MWST(string line)
        {
            decimal decVal = 0;

            if((line.Length >= Settings.Default.OrderVATPercentageStart + Settings.Default.OrderVATPercentageLength)
                && Decimal.TryParse(line.Substring(Settings.Default.OrderVATPercentageStart, Settings.Default.OrderVATPercentageLength).Trim().Replace(',','.'), out decVal))
            {
                VATPercentage = decVal;
            }

            if((line.Length >= Settings.Default.OrderTotalVATStart + Settings.Default.OrderTotalVATLength)
                && Decimal.TryParse(line.Substring(Settings.Default.OrderTotalVATStart, Settings.Default.OrderTotalVATLength).Trim().Replace(',', '.'), out decVal))
            {
                TotalVAT = decVal;
            }
        }
    }
    public class VATTable
    {
        public decimal? Value
        {
            get
            {
                if (!(Table is null) && Table.Count > 0)
                {
                    decimal? val = 0;
                    foreach (var item in Table)
                    {
                        val += item.TotalVAT;
                    }
                    return val;
                }
                else
                    return 0;
            }
        }
        public List<MWST> Table { get; set;}
        public VATTable(string line)
        {
            if (Table == null)
            {
                Table = new List<MWST>();
            }
            Table.Add(new MWST(line));
        }
    }
}
