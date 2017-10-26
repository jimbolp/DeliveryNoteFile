using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Header
    {
        private void Line2(string line)
        {
            int intVal = 0;
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

                //UserName
                if (line.Length >= Settings.Default.UserNameStart + Settings.Default.UserNameLength)
                {
                    UserName = line.Substring(Settings.Default.UserNameStart, Settings.Default.UserNameLength).Trim();
                }
                else if (line.Length >= Settings.Default.UserNameStart)
                {
                    UserName = line.Substring(Settings.Default.UserNameStart).Trim();
                }

                //DiscountOnInvoice
                if (line.Length >= Settings.Default.DiscountOnInvoiceStart + Settings.Default.DiscountOnInvoiceLength)
                {
                    DiscountOnInvoice = Parse.StringToBool(line.Substring(Settings.Default.DiscountOnInvoiceStart, Settings.Default.DiscountOnInvoiceLength).Trim());
                }
                else if (line.Length >= Settings.Default.DiscountOnInvoiceStart)
                {
                    DiscountOnInvoice = Parse.StringToBool(line.Substring(Settings.Default.DiscountOnInvoiceStart).Trim());
                }

                //OrderRemark
                if (line.Length >= Settings.Default.OrderRemarkStart + Settings.Default.OrderRemarkLength)
                {
                    OrderRemark = line.Substring(Settings.Default.OrderRemarkStart, Settings.Default.OrderRemarkLength).Trim();
                }
                else if (line.Length >= Settings.Default.OrderRemarkStart)
                {
                    OrderRemark = line.Substring(Settings.Default.OrderRemarkStart).Trim();
                }

                //PickingType
                if (line.Length >= Settings.Default.PickingTypeStart + Settings.Default.PickingTypeLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PickingTypeStart, Settings.Default.PickingTypeLength).Trim(), out intVal))
                    {
                        PickingType = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.PickingTypeStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PickingTypeStart).Trim(), out intVal))
                    {
                        PickingType = intVal;
                    }
                }

                //BookingType
                if (line.Length >= Settings.Default.BookingTypeStart + Settings.Default.BookingTypeLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.BookingTypeStart, Settings.Default.BookingTypeLength).Trim(), out intVal))
                    {
                        BookingType = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.BookingTypeStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.BookingTypeStart).Trim(), out intVal))
                    {
                        BookingType = intVal;
                    }
                }

                //CSCOrderNumber
                if (line.Length >= Settings.Default.CSCOrderNumberStart + Settings.Default.CSCOrderNumberLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.CSCOrderNumberStart, Settings.Default.CSCOrderNumberLength).Trim(), out intVal))
                    {
                        CSCOrderNumber = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.CSCOrderNumberStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.CSCOrderNumberStart).Trim(), out intVal))
                    {
                        CSCOrderNumber = intVal;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
