using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Supplier
    {
        private void Line2(string line)
        {
            try
            {
                int intVal = 0;
                //BranchName
                if (line.Length >= Settings.Default.BranchNameStart + Settings.Default.BranchNameLength)
                {
                    BranchName = line.Substring(Settings.Default.BranchNoStart, Settings.Default.BranchNoLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchNameStart)
                {
                    BranchName = line.Substring(Settings.Default.BranchNoStart).Trim();
                }

                //BranchAddress
                if (line.Length >= Settings.Default.BranchAddressStart + Settings.Default.BranchAddressLength)
                {
                    BranchAddress = line.Substring(Settings.Default.BranchAddressStart, Settings.Default.BranchAddressLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchAddressStart)
                {
                    BranchAddress = line.Substring(Settings.Default.BranchAddressStart).Trim();
                }

                //BranchCIP
                if (line.Length >= Settings.Default.BranchCIPStart + Settings.Default.BranchCIPLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.BranchCIPStart, Settings.Default.BranchCIPLength).Trim(), out intVal))
                    {
                        BranchCIP = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.BranchCIPStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.BranchCIPStart).Trim(), out intVal))
                    {
                        BranchCIP = intVal;
                    }
                }

                //BranchCity
                if (line.Length >= Settings.Default.BranchCityStart + Settings.Default.BranchCityLength)
                {
                    BranchCity = line.Substring(Settings.Default.BranchCityStart, Settings.Default.BranchCityLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchCityStart)
                {
                    BranchCity = line.Substring(Settings.Default.BranchCityStart).Trim();
                }

                //BranchUIN
                if (line.Length >= Settings.Default.BranchUINStart + Settings.Default.BranchUINLength)
                {
                    BranchUIN = line.Substring(Settings.Default.BranchUINStart, Settings.Default.BranchUINLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchUINStart)
                {
                    BranchUIN = line.Substring(Settings.Default.BranchUINStart).Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
