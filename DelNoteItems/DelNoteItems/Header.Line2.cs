﻿using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Header
    {
        private void Line2(string line)
        {
            int intVal = 0;
            long longVal = 0;
            try
            {
                //OrderType
                if (line.Length >= Settings.Default.OrderTypeStart + Settings.Default.OrderTypeLength)
                {
                    OrderType = line.Substring(Settings.Default.OrderTypeStart, Settings.Default.OrderTypeLength).Trim();
                }
                else if (line.Length >= Settings.Default.OrderTypeStart)
                {
                    OrderType = line.Substring(Settings.Default.OrderTypeStart).Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}