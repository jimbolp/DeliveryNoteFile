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
            if(Int32.TryParse(line.Substring(Settings.Default.BranchNoStart, Settings.Default.BranchNoLength), out intVal))
            {
                BranchNumber = intVal;
            }
        }
    }
}
