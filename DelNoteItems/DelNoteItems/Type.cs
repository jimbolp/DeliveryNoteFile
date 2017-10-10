using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Type
    {
        public string DocumentType { get; set; }

        public Type(string line)
        {
            DocumentType = (line.Substring(Settings.Default.DocTypeStart)).Trim();
        }
    }
}
