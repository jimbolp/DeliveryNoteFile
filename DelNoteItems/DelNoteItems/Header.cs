using System;
using Settings = DelNoteItems.Properties.Settings1;

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
                if ((line.Length >= Settings.Default.DeliveryNoteNumberStart + Settings.Default.DeliveryNoteNumberLength) 
                    && Int64.TryParse(line.Substring(Settings.Default.DeliveryNoteNumberStart, Settings.Default.DeliveryNoteNumberLength).Trim(), out longVal))
                {
                    DeliveryNoteNumber = longVal;
                }
                
                if (line.Length >= Settings.Default.DeliveryNoteDateStart + Settings.Default.DeliveryNoteDateLength)
                {
                    DeliveryNoteDate = DateID.Convert(line.Substring(Settings.Default.DeliveryNoteDateStart, Settings.Default.DeliveryNoteDateLength).Trim());
                }
                
                if ((line.Length >= Settings.Default.RebateInKindOrderStart + Settings.Default.RebateInKindOrderLength) 
                    && Int32.TryParse(line.Substring(Settings.Default.RebateInKindOrderStart, Settings.Default.RebateInKindOrderLength).Trim(), out intVal))
                {
                    RebateInKindOrder = IntToBool(intVal);
                }
                
                if (line.Length >= Settings.Default.isNZOKOrderStart + Settings.Default.isNZOKOrderLength)
                {
                    switch(line.Substring(Settings.Default.isNZOKOrderStart, Settings.Default.isNZOKOrderLength).Trim())
                    {
                        case "J":
                            isNZOKOrder = true;
                            break;
                        case "N":
                            isNZOKOrder = false;
                            break;
                        default:
                            isNZOKOrder = null;
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
