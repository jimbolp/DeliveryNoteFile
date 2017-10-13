using System;
using System.Collections;
using System.Reflection;
using Settings = DelNoteItems.Properties.Config;

namespace DelNoteItems
{
    public class Header
    {
        //$$Header$$ Line properties
        public long? DeliveryNoteNumber { get; set; }
        public DateTime? DeliveryNoteDate { get; set; }
        public bool? RebateInKindOrder { get; set; }
        public bool? isNZOKOrder { get; set; }
        public string NarcoticsFormID { get; set; }
        public DateTime? NarcoticsFormDate { get; set; }
        public DateTime? DateOfDelivery { get; set; }
        public bool? DiscountType { get; set; }

        //$$Header2$$ Line properties still not known

        public Header(string line)
        {
            int intVal = 0;
            long longVal = 0;
            //DateTime dateVal;
            try
            {
                //DeliveryNoteNumber
                if (line.Length >= Settings.Default.DeliveryNoteNumberStart + Settings.Default.DeliveryNoteNumberLength)
                {
                    if(Int64.TryParse(line.Substring(Settings.Default.DeliveryNoteNumberStart, Settings.Default.DeliveryNoteNumberLength).Trim(), out longVal))
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
                else if(line.Length >= Settings.Default.DeliveryNoteDateStart)
                {
                    DeliveryNoteDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.DeliveryNoteDateStart).Trim());
                }

                //RebateInKindOrder
                if (line.Length >= Settings.Default.RebateInKindOrderStart + Settings.Default.RebateInKindOrderLength)
                {
                    if(Int32.TryParse(line.Substring(Settings.Default.RebateInKindOrderStart, Settings.Default.RebateInKindOrderLength).Trim(), out intVal))
                    {
                        RebateInKindOrder = Parse.IntToBool(intVal);
                    }
                }
                else if(line.Length >= Settings.Default.RebateInKindOrderStart)
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
                else if(line.Length >= Settings.Default.NarcoticsFormIDStart)
                {
                    NarcoticsFormID = line.Substring(Settings.Default.NarcoticsFormIDStart).Trim();
                }

                //NarcoticsFormDate
                if (line.Length >= Settings.Default.NarcoticsFormDateStart + Settings.Default.NarcoticsFormDateLength)
                {
                    NarcoticsFormDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.NarcoticsFormDateStart, Settings.Default.NarcoticsFormDateLength).Trim());
                }
                else if(line.Length >= Settings.Default.NarcoticsFormDateStart)
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


    }
}
