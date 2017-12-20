
using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Customer : DelNoteItems
    {
        private void Line2(string line)
        {
            long longVal = 0;
            //CustomerLicenceNumber
            if (line.Length >= Settings.Default.CustomerLicenceNumberStart + Settings.Default.CustomerLicenceNumberLength)
            {
                CustomerLicenceNumber = line.Substring(Settings.Default.CustomerLicenceNumberStart, Settings.Default.CustomerLicenceNumberLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerLicenceNumberStart)
            {
                CustomerLicenceNumber = line.Substring(Settings.Default.CustomerLicenceNumberStart).Trim();
            }

            //CustomerNarcLicenceNumber
            if (line.Length >= Settings.Default.CustomerNarcLicenceNumberStart + Settings.Default.CustomerNarcLicenceNumberLength)
            {
                CustomerNarcLicenceNumber = line.Substring(Settings.Default.CustomerNarcLicenceNumberStart, Settings.Default.CustomerNarcLicenceNumberLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerNarcLicenceNumberStart)
            {
                CustomerNarcLicenceNumber = line.Substring(Settings.Default.CustomerNarcLicenceNumberStart).Trim();
            }

            //CustomerAccountablePerson
            if (line.Length >= Settings.Default.CustomerAccountablePersonStart + Settings.Default.CustomerAccountablePersonLength)
            {
                CustomerAccountablePerson = line.Substring(Settings.Default.CustomerAccountablePersonStart, Settings.Default.CustomerAccountablePersonLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerAccountablePersonStart)
            {
                CustomerAccountablePerson = line.Substring(Settings.Default.CustomerAccountablePersonStart).Trim();
            }

            //CustomerPhoneNo
            if (line.Length >= Settings.Default.CustomerPhoneNoStart + Settings.Default.CustomerPhoneNoLength)
            {
                if (Int64.TryParse(line.Substring(Settings.Default.CustomerPhoneNoStart, Settings.Default.CustomerPhoneNoLength).Trim(), out longVal))
                {
                    CustomerPhoneNo = longVal;
                }
                else if (line.Length >= Settings.Default.CustomerPhoneNoStart)
                {
                    if (Int64.TryParse(line.Substring(Settings.Default.CustomerPhoneNoStart).Trim(), out longVal))
                    {
                        CustomerPhoneNo = longVal;
                    }
                }
            }
        }
    }
}
