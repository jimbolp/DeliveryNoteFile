
using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Customer : DelNoteItems
    {
        //$$CUSTOMER$$ Line properties
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerCIP { get; set; }            //Post Code

        public Customer(string line, bool isCreditNote)
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
            catch (Exception)
            {
                throw;
            }
        }

        private void InitializeInvoice(string line)
        {
            try
            {
                int intVal;
                //CustomerName
                if (line.Length >= Settings.Default.CustomerNameStart + Settings.Default.CustomerNameLength)
                {
                    CustomerName = line.Substring(Settings.Default.CustomerNameStart, Settings.Default.CustomerNameLength).Trim();
                }
                else if (line.Length >= Settings.Default.CustomerNameStart)
                {
                    CustomerName = line.Substring(Settings.Default.CustomerNameStart).Trim();
                }

                //CustomerAddress
                if (line.Length >= Settings.Default.CustomerAddressStart + Settings.Default.CustomerAddressLength)
                {
                    CustomerAddress = line.Substring(Settings.Default.CustomerAddressStart, Settings.Default.CustomerAddressLength).Trim();
                }
                else if (line.Length >= Settings.Default.CustomerAddressStart)
                {
                    CustomerAddress = line.Substring(Settings.Default.CustomerAddressStart).Trim();
                }

                //CustomerCIP
                if (line.Length >= Settings.Default.CustomerCIPStart + Settings.Default.CustomerCIPLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.CustomerCIPStart, Settings.Default.CustomerCIPLength).Trim(), out intVal))
                    {
                        CustomerCIP = intVal;
                    }
                    else if (line.Length >= Settings.Default.CustomerCIPStart)
                    {
                        if (Int32.TryParse(line.Substring(Settings.Default.CustomerCIPStart).Trim(), out intVal))
                        {
                            CustomerCIP = intVal;
                        }
                    }
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
