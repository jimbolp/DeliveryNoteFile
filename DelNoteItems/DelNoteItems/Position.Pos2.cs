using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Pos2(string line)
        {
            try
            {
                if(line.Length >= Settings.Default.ExpiryDateStart + Settings.Default.ExpiryDateLength)
                {
                    ExpiryDate = DateID.Convert(line.Substring(Settings.Default.ExpiryDateStart, Settings.Default.ExpiryDateLength));
                }

                if(line.Length >= Settings.Default.BatchStart + Settings.Default.BatchLength)
                {
                    Batch = line.Substring(Settings.Default.BatchStart, Settings.Default.BatchLength).Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
