using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Config;
namespace DelNoteItems
{
    public class Supplier
    {
        public int? BranchNumber { get; set; }

        public Supplier(string line)
        {
            int intVal = 0;

            //BranchNumber
            if (line.Length >= Settings.Default.BranchNoStart + Settings.Default.BranchNoLength)
            {
                if(Int32.TryParse(line.Substring(Settings.Default.BranchNoStart, Settings.Default.BranchNoLength).Trim(), out intVal))
                {
                    BranchNumber = intVal;
                }
            }
            else if(line.Length >= Settings.Default.BranchNoStart)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.BranchNoStart).Trim(), out intVal))
                {
                    BranchNumber = intVal;
                }
            }
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
