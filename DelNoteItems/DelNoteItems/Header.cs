using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Header
    {
        //$$Header$$ Line properties
        public long? DeliveryNoteNumber { get; set; }       //DelNoteNo
        public DateTime? DeliveryNoteDate { get; set; }     //DelNoteDate
        public bool? RebateInKindOrder { get; set; }        //RebateOrder
        public bool? isNZOK { get; set; }                   //isNZOKOrder
        public string NarcoticsFormID { get; set; }         //NarcoticsFormID
        public DateTime? NarcoticsFormDate { get; set; }    //NarcoticsFormDate
        public DateTime? DateOfDelivery { get; set; }       //DateOfDelivery
        public bool? DiscountType { get; set; }             //DiscountType

        //$$Header2$$ Line properties still not known

        public Header(string line)
        {
            int intVal = 0;
            long longVal = 0;
            //DateTime dateVal;
            try
            {
                if ((line.Length >= Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength) 
                    && Int64.TryParse(line.Substring(Settings.Default.DelNoteNoStart, Settings.Default.DelNoteNoLength).Trim(), out longVal))
                {
                    DeliveryNoteNumber = longVal;
                }
                
                if (line.Length >= Settings.Default.DelNoteDateStart + Settings.Default.DelNoteDateLength)
                {
                    DeliveryNoteDate = DateID.Convert(line.Substring(Settings.Default.DelNoteDateStart, Settings.Default.DelNoteDateLength).Trim());
                }
                
                if ((line.Length >= Settings.Default.RebateOrderStart + Settings.Default.RebateOrderLength) 
                    && Int32.TryParse(line.Substring(Settings.Default.RebateOrderStart, Settings.Default.RebateOrderLength).Trim(), out intVal))
                {
                    RebateInKindOrder = IntToBool(intVal);
                }
                
                if (line.Length >= Settings.Default.isNZOKOrderStart + Settings.Default.isNZOKOrderLength)
                {
                    switch(line.Substring(Settings.Default.isNZOKOrderStart, Settings.Default.isNZOKOrderLength).Trim())
                    {
                        case "J":
                            isNZOK = true;
                            break;
                        case "N":
                            isNZOK = false;
                            break;
                        default:
                            isNZOK = null;
                            break;
                    }
                }

                if(line.Length >= Settings.Default.NarcoticsFormIDStart + Settings.Default.NarcoticsFormIDLength)
                {
                    NarcoticsFormID = line.Substring(Settings.Default.NarcoticsFormIDStart, Settings.Default.NarcoticsFormIDLength);
                    if (!string.IsNullOrEmpty(NarcoticsFormID))
                    {
                        NarcoticsFormID = NarcoticsFormID.Trim();
                    }
                }

                if(line.Length >= Settings.Default.NarcoticsFormDateStart + Settings.Default.NarcoticsFormDateLength)
                {
                    NarcoticsFormDate = DateID.Convert(line.Substring(Settings.Default.NarcoticsFormDateStart, Settings.Default.NarcoticsFormDateLength).Trim());
                }

                if(line.Length >= Settings.Default.DateOfDeliveryStart + Settings.Default.DateOfDeliveryLength)
                {
                    DateOfDelivery = DateID.Convert(line.Substring(Settings.Default.DateOfDeliveryStart, Settings.Default.DateOfDeliveryLength).Trim());
                }

                if (line.Length >= Settings.Default.DiscountTypeStart + Settings.Default.DiscountTypeLength)
                {
                    switch (line.Substring(Settings.Default.DiscountTypeStart, Settings.Default.DiscountTypeLength).Trim())
                    {
                        case "J":
                            DiscountType = true;
                            break;
                        case "N":
                            DiscountType = false;
                            break;
                        default:
                            DiscountType = null;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private bool? IntToBool(int i)
        {
            switch (i)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    return null;                
            }
        }
    }
}
