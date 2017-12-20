using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Line5(string line)
        {
            //ArticleRemark
            if (line.Length >= Settings.Default.ArticleRemarkStart + Settings.Default.ArticleRemarkLength)
            {
                ArticleRemark = line.Substring(Settings.Default.ArticleRemarkStart, Settings.Default.ArticleRemarkLength).Trim();
            }
            else if (line.Length >= Settings.Default.ArticleRemarkStart)
            {
                ArticleRemark = line.Substring(Settings.Default.ArticleRemarkStart).Trim();
            }
        }
    }
}
