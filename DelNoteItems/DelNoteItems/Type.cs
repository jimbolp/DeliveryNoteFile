using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Type : DelNoteItems
    {
        public string DocumentType { get; set; }
        public bool isCreditNote { get; set; }

        public Type(string line)
        {
            DocumentType = (line.Substring(Settings.Default.DocTypeStart)).Trim();
            switch (DocumentType)
            {
                case "INVOICE":
                    isCreditNote = false;
                    break;
                case "CREDITNOTE":
                    isCreditNote = true;
                    break;
                default:
                    throw new Exception($"The document type should be INVOICE or CREDITNOTE and not {DocumentType ?? "null"}!");
            }
        }
    }
}
