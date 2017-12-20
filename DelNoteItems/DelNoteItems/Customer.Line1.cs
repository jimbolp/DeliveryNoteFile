
using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Customer : DelNoteItems
    {
        private void Line1(string line)
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

            //CustomerCity
            if (line.Length >= Settings.Default.CustomerCityStart + Settings.Default.CustomerCityLength)
            {
                CustomerCity = line.Substring(Settings.Default.CustomerCityStart, Settings.Default.CustomerCityLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerCityStart)
            {
                CustomerCity = line.Substring(Settings.Default.CustomerCityStart).Trim();
            }

            //CustomerUIN
            if (line.Length >= Settings.Default.CustomerUINStart + Settings.Default.CustomerUINLength)
            {
                CustomerUIN = line.Substring(Settings.Default.CustomerUINStart, Settings.Default.CustomerUINLength).Trim();
            }
            else if (line.Length >= Settings.Default.CustomerUINStart)
            {
                CustomerUIN = line.Substring(Settings.Default.CustomerUINStart).Trim();
            }
        }
    }
}
