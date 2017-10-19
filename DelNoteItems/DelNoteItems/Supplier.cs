using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Settings;
namespace DelNoteItems
{
    public class Supplier : DelNoteItems
    {
        public int? BranchNumber { get; set; }

        public Supplier(string line, bool isCreditNote)
        {
            try
            {
                if (isCreditNote)
                {
                    InitializeCreditNote(line);
                }
                else
                {
                    InitializeInvoice(line);
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private void InitializeInvoice(string line)
        {
            int intVal = 0;

            //BranchNumber
            if (line.Length >= Settings.Default.BranchNoStart + Settings.Default.BranchNoLength)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.BranchNoStart, Settings.Default.BranchNoLength).Trim(), out intVal))
                {
                    BranchNumber = intVal;
                }
            }
            else if (line.Length >= Settings.Default.BranchNoStart)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.BranchNoStart).Trim(), out intVal))
                {
                    BranchNumber = intVal;
                }
            }
        }
        private void InitializeCreditNote(string line)
        {
            //FixLine(line);
            InitializeInvoice(line);
        }
    }
}
