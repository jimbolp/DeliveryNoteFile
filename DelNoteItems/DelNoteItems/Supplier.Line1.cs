﻿using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Supplier
    {
        private void Line1(string line)
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
    }
}
