using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Config;

namespace DelNoteItems
{
    public class Type
    {
        public string DocumentType { get; set; }

        public Type(string line)
        {
            DocumentType = (line.Substring(Settings.Default.DocTypeStart)).Trim();
        }
#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties())
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
