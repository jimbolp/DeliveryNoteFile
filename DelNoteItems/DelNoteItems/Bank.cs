using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Bank
    {
        public string BranchBankName { get; set; }
        public string BranchBankIBAN { get; set; }
        public string BranchBankBIC { get; set; }

        public Bank(string line, bool isCreditNote)
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
            try
            {
                //BranchBankName
                if (line.Length >= Settings.Default.BranchBankNameStart + Settings.Default.BranchBankNameLength)
                {
                    BranchBankName = line.Substring(Settings.Default.BranchBankNameStart, Settings.Default.BranchBankNameLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchBankNameStart)
                {
                    BranchBankName = line.Substring(Settings.Default.BranchBankNameStart).Trim();
                }

                //BranchBankIBAN
                if (line.Length >= Settings.Default.BranchBankIBANStart + Settings.Default.BranchBankIBANLength)
                {
                    BranchBankIBAN = line.Substring(Settings.Default.BranchBankIBANStart, Settings.Default.BranchBankIBANLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchBankIBANStart)
                {
                    BranchBankIBAN = line.Substring(Settings.Default.BranchBankIBANStart).Trim();
                }

                //BranchBankBIC
                if (line.Length >= Settings.Default.BranchBankBICStart + Settings.Default.BranchBankBICLength)
                {
                    BranchBankBIC = line.Substring(Settings.Default.BranchBankBICStart, Settings.Default.BranchBankBICLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchBankBICStart)
                {
                    BranchBankBIC = line.Substring(Settings.Default.BranchBankBICStart).Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void InitializeCreditNote(string line)
        {
            //FixLine();
            InitializeInvoice(line);
        }
    }
}
