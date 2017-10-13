using System;
using Settings = DelNoteItems.Properties.Config;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Pos3(string line)
        {
            try
            {
                //ArticleLongName
                if (line.Length >= Settings.Default.ArticleLongNameStart + Settings.Default.ArticleLongNameLength)
                {
                    ArticleLongName = line.Substring(Settings.Default.ArticleLongNameStart, Settings.Default.ArticleLongNameLength).Trim();
                }
                else if(line.Length >= Settings.Default.ArticleLongNameStart)
                {
                    ArticleLongName = line.Substring(Settings.Default.ArticleLongNameStart).Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
