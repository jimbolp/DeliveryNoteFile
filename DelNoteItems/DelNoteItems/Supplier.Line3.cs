using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Supplier
    {
        private void Line3(string line)
        {
            try
            {
                //BranchLicenceNumber
                if (line.Length >= Settings.Default.BranchLicenceNumberStart + Settings.Default.BranchLicenceNumberLength)
                {
                    BranchLicenceNumber = line.Substring(Settings.Default.BranchLicenceNumberStart, Settings.Default.BranchLicenceNumberLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchLicenceNumberStart)
                {
                    BranchLicenceNumber = line.Substring(Settings.Default.BranchLicenceNumberStart).Trim();
                }

                //BranchNarcLicenceNumber
                if (line.Length >= Settings.Default.BranchNarcLicenceNumberStart + Settings.Default.BranchNarcLicenceNumberLength)
                {
                    BranchNarcLicenceNumber = line.Substring(Settings.Default.BranchNarcLicenceNumberStart, Settings.Default.BranchNarcLicenceNumberLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchNarcLicenceNumberStart)
                {
                    BranchNarcLicenceNumber = line.Substring(Settings.Default.BranchNarcLicenceNumberStart).Trim();
                }

                //BranchResponsible
                if (line.Length >= Settings.Default.BranchResponsibleStart + Settings.Default.BranchResponsibleLength)
                {
                    BranchResponsible = line.Substring(Settings.Default.BranchResponsibleStart, Settings.Default.BranchResponsibleLength).Trim();
                }
                else if (line.Length >= Settings.Default.BranchResponsibleStart)
                {
                    BranchResponsible = line.Substring(Settings.Default.BranchResponsibleStart).Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
