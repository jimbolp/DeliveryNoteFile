using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Line2(string line)
        {
            try
            {
                //ExpiryDate
                if (line.Length >= Settings.Default.ExpiryDateStart + Settings.Default.ExpiryDateLength)
                {
                    ExpiryDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.ExpiryDateStart, Settings.Default.ExpiryDateLength));
                }
                else if(line.Length >= Settings.Default.ExpiryDateStart)
                {
                    ExpiryDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.ExpiryDateStart));
                }

                //Batch
                if (line.Length >= Settings.Default.BatchStart + Settings.Default.BatchLength)
                {
                    Batch = line.Substring(Settings.Default.BatchStart, Settings.Default.BatchLength).Trim();
                }
                else if(line.Length >= Settings.Default.BatchStart)
                {
                    Batch = line.Substring(Settings.Default.BatchStart).Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
