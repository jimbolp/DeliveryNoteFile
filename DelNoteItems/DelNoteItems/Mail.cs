using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Mail : DelNoteItems
    {
        public string CustomerEmailAddress { get; set; }
        public string ValueOfFieldInSK17 { get; set; }      //Seriously... That's the information I got from Mr. Rolf Raab!? What field... Who knows?!

        public Mail(string line, bool isCreditNote)
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
            catch (Exception e)
            {
                WriteExceptionToLog(e);
                throw e;
            }
        }

        private void InitializeInvoice(string line)
        {
            //CustomerEmailAddress
            if (line.Length >= Settings.Default.CustomerEmailAddressStart + Settings.Default.CustomerEmailAddressLength)
            {
                CustomerEmailAddress = line.Substring(Settings.Default.CustomerEmailAddressStart, Settings.Default.CustomerEmailAddressLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerEmailAddressStart)
            {
                CustomerEmailAddress = line.Substring(Settings.Default.CustomerEmailAddressStart).Trim();
            }

            //ValueOfFieldInSK17
            if (line.Length >= Settings.Default.ValueOfFieldInSK17Start + Settings.Default.ValueOfFieldInSK17Length)
            {
                ValueOfFieldInSK17 = line.Substring(Settings.Default.ValueOfFieldInSK17Start, Settings.Default.ValueOfFieldInSK17Length).Trim();
            }
            else if (line.Length >= Settings.Default.ValueOfFieldInSK17Start)
            {
                ValueOfFieldInSK17 = line.Substring(Settings.Default.ValueOfFieldInSK17Start).Trim();
            }
        }

        private void InitializeCreditNote(string line)
        {
            InitializeInvoice(line);
        }
    }
}
