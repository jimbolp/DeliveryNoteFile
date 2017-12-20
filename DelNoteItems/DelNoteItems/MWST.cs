using Settings = DelNoteItems.Properties.Settings;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Collections;

namespace DelNoteItems
{
    public class MWST : DelNoteItems
    {
        public decimal? OrderVATPercentage { get; set; }
        public decimal? TotalVAT { get; set; }

        /// <summary>
        /// MwSt is the abbreviation for "Mehrwertsteuer", which is also known as "Umsatzsteuer" (USt). This is the equivalent to VAT in the UK.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="isCreditNote"></param>
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
    }
    public class VATTable : DelNoteItems
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
        public decimal? TotalPercent
        {
            get
            {
                if (Table != null && Table.Count > 0)
                {
                    decimal? val = 0;
                    foreach (var item in Table)
                    {
                        val += item.OrderVATPercentage;
                    }
                    return val;
                }
                else
                    return 0;
            }
        }

        public VATTable(string line, bool isCreditNote)
        {
            try
            {
                if (Table == null)
                {
                    Table = new List<MWST>();
                }
                Table.Add(new MWST(line, isCreditNote));
            }
            catch (Exception e)
            {
                WriteExceptionToLog(e);
                throw e;
            }
        }

    }
}
