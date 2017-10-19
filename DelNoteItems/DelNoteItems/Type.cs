using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Type
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
#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!pi.GetType().IsAssignableFrom(typeof(IEnumerable)))
                {
                    toString += pi.Name + " -> ";
                    try
                    {                        
                        toString += pi.GetValue(this).ToString();                        
                    }
                    catch (Exception) { }
                    toString += Environment.NewLine;
                }
            }
            return toString;
        }
#endif
    }
}
