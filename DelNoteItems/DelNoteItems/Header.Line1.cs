using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Header
    {
        private void Line1(string line)
        {
            int intVal = 0;
            long longVal = 0;
            try
            {
                //DeliveryNoteNumber
                if (line.Length >= Settings.Default.DeliveryNoteNumberStart + Settings.Default.DeliveryNoteNumberLength)
                {
                    if (Int64.TryParse(line.Substring(Settings.Default.DeliveryNoteNumberStart, Settings.Default.DeliveryNoteNumberLength).Trim(), out longVal))
                    {
                        DeliveryNoteNumber = longVal;
                    }
                }
                else if (line.Length >= Settings.Default.DeliveryNoteNumberStart)
                {
                    if (Int64.TryParse(line.Substring(Settings.Default.DeliveryNoteNumberStart).Trim(), out longVal))
                    {
                        DeliveryNoteNumber = longVal;
                    }
                }

                //DeliveryNoteDate
                if (line.Length >= Settings.Default.DeliveryNoteDateStart + Settings.Default.DeliveryNoteDateLength)
                {
                    DeliveryNoteDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.DeliveryNoteDateStart, Settings.Default.DeliveryNoteDateLength).Trim());
                }
                else if (line.Length >= Settings.Default.DeliveryNoteDateStart)
                {
                    DeliveryNoteDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.DeliveryNoteDateStart).Trim());
                }

                //RebateInKindOrder
                if (line.Length >= Settings.Default.RebateInKindOrderStart + Settings.Default.RebateInKindOrderLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.RebateInKindOrderStart, Settings.Default.RebateInKindOrderLength).Trim(), out intVal))
                    {
                        RebateInKindOrder = Parse.IntToBool(intVal);
                    }
                }
                else if (line.Length >= Settings.Default.RebateInKindOrderStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.RebateInKindOrderStart).Trim(), out intVal))
                    {
                        RebateInKindOrder = Parse.IntToBool(intVal);
                    }
                }

                
                //isNZOKOrder
                if (line.Length >= Settings.Default.isNZOKOrderStart + Settings.Default.isNZOKOrderLength)
                {
                    isNZOKOrder = Parse.StringToBool(line.Substring(Settings.Default.isNZOKOrderStart, Settings.Default.isNZOKOrderLength));
                }
                else if (line.Length >= Settings.Default.isNZOKOrderStart)
                {
                    isNZOKOrder = Parse.StringToBool(line.Substring(Settings.Default.isNZOKOrderStart));
                }

                //NarcoticsFormID
                if (line.Length >= Settings.Default.NarcoticsFormIDStart + Settings.Default.NarcoticsFormIDLength)
                {
                    NarcoticsFormID = line.Substring(Settings.Default.NarcoticsFormIDStart, Settings.Default.NarcoticsFormIDLength).Trim();
                }
                else if (line.Length >= Settings.Default.NarcoticsFormIDStart)
                {
                    NarcoticsFormID = line.Substring(Settings.Default.NarcoticsFormIDStart).Trim();
                }

                //NarcoticsFormDate
                if (line.Length >= Settings.Default.NarcoticsFormDateStart + Settings.Default.NarcoticsFormDateLength)
                {
                    NarcoticsFormDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.NarcoticsFormDateStart, Settings.Default.NarcoticsFormDateLength).Trim());
                }
                else if (line.Length >= Settings.Default.NarcoticsFormDateStart)
                {
                    NarcoticsFormDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.NarcoticsFormDateStart).Trim());
                }

                //DateOfDelivery
                if (line.Length >= Settings.Default.DateOfDeliveryStart + Settings.Default.DateOfDeliveryLength)
                {
                    DateOfDelivery = Parse.ConvertToDateTime(line.Substring(Settings.Default.DateOfDeliveryStart, Settings.Default.DateOfDeliveryLength).Trim());
                }
                else if (line.Length >= Settings.Default.DateOfDeliveryStart)
                {
                    DateOfDelivery = Parse.ConvertToDateTime(line.Substring(Settings.Default.DateOfDeliveryStart).Trim());
                }

                //DiscountType
                if (line.Length >= Settings.Default.DiscountTypeStart + Settings.Default.DiscountTypeLength)
                {
                    DiscountType = Parse.StringToBool(line.Substring(Settings.Default.DiscountTypeStart, Settings.Default.DiscountTypeLength));
                }
                else if (line.Length >= Settings.Default.DiscountTypeStart)
                {
                    DiscountType = Parse.StringToBool(line.Substring(Settings.Default.DiscountTypeStart));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
