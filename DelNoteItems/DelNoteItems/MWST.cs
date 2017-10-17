using Settings = DelNoteItems.Properties.Config;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Collections;

namespace DelNoteItems
{
    /// <summary>
    /// MwSt is the abbreviation for "Mehrwertsteuer", which is also known as "Umsatzsteuer" (USt). This is the equivalent to VAT in the UK.
    /// </summary>
    public class MWST
    {
        public decimal? OrderVATPercentage { get; set; }
        public decimal? TotalVAT { get; set; }

        public MWST(string line, bool isCreditNote)
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

        private void InitializeInvoice(string line)
        {
            decimal decVal = 0;

            //OrderVATPercentage
            if ((line.Length >= Settings.Default.OrderVATPercentageStart + Settings.Default.OrderVATPercentageLength)
                && Decimal.TryParse(line.Substring(Settings.Default.OrderVATPercentageStart, Settings.Default.OrderVATPercentageLength).Trim().Replace(',', '.'), out decVal))
            {
                OrderVATPercentage = decVal;
            }

            //TotalVAT
            if ((line.Length >= Settings.Default.OrderTotalVATStart + Settings.Default.OrderTotalVATLength)
                && Decimal.TryParse(line.Substring(Settings.Default.OrderTotalVATStart, Settings.Default.OrderTotalVATLength).Trim().Replace(',', '.'), out decVal))
            {
                TotalVAT = decVal;
            }
        }
        private void InitializeCreditNote(string line)
        {
            //FixLine(line);
            InitializeInvoice(line);
        }

#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!pi.GetType().IsAssignableFrom(typeof(IEnumerable)))
                {
                    try
                    {
                        toString += pi.Name + " -> ";
                        toString += pi.GetValue(this).ToString();
                        toString += Environment.NewLine;
                    }
                    catch (Exception) { }
                }
            }
            return toString;
        } 
#endif
    }
    public class VATTable
    {
        public List<MWST> Table { get; set; }
        public decimal? Value
        {
            get
            {
                if (Table != null && Table.Count > 0)
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
        
        public VATTable(string line, bool isCreditNote)
        {
            if (Table == null)
            {
                Table = new List<MWST>();
            }
            Table.Add(new MWST(line, isCreditNote));
        }

#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType))
                {
                    toString += pi.Name + " -> ";
                    try
                    {
                        toString += pi.GetValue(this).ToString();
                    }
                    catch (Exception) { }
                    toString += Environment.NewLine;
                }
                else
                {
                    foreach (var m in pi.GetValue(this) as IEnumerable)
                    {
                        try
                        {
                            toString += m.ToString();
                        }
                        catch (Exception) { }
                    }
                }
            }
            return toString;
        } 
#endif
    }
}
