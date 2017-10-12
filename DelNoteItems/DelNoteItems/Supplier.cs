using System;
using Settings = DelNoteItems.Properties.Settings1;
namespace DelNoteItems
{
    public class Supplier
    {
        public int? BranchNumber { get; set; }

        public Supplier(string line)
        {
            int intVal = 0;
            if(line.Length >= Settings.Default.BranchNoStart + Settings.Default.BranchNoLength
                && Int32.TryParse(line.Substring(Settings.Default.BranchNoStart, Settings.Default.BranchNoLength).Trim(), out intVal))
            {
                BranchNumber = intVal;
            }
        }
    }
}
