
using System.Collections.Generic;

namespace DelNoteItems
{
    /// <summary>
    /// MwSt is the abbreviation for "Mehrwertsteuer", which is also known as "Umsatzsteuer" (USt). This is the equivalent to VAT in the UK.
    /// </summary>
    public class MWST
    {
        public decimal? VATPercentage { get; set; }
        public decimal? TotalVAT { get; set; }
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
            Table = new List<MWST>();
        }
    }
}
