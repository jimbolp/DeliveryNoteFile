using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Header
    {
        //$$Header$$ Line properties
        public int? DeliveryNoteNumber { get; set; }        //DelNoteNo
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
            DateTime dateVal;
            try
            {
                if (Int32.TryParse(line.Substring(Settings.Default.DelNoteNoStart, Settings.Default.DelNoteNoLength), out intVal))
                {
                    DeliveryNoteNumber = intVal;
                }
                if(DateTime.TryParse(line.Substring(Settings.Default.DelNoteDateStart, Settings.Default.DelNoteDateLength), out dateVal))
                {
                    DeliveryNoteDate = dateVal;
                }
                if(Int32.TryParse(line.Substring(Settings.Default.RebateOrderStart, Settings.Default.RebateOrderLength), out intVal))
                {
                    RebateInKindOrder = IntToBool(intVal);
                }
                if (Int32.TryParse(line.Substring(Settings.Default.isNZOKOrderStart, Settings.Default.isNZOKOrderLength), out intVal))
                {
                    isNZOK = IntToBool(intVal);
                }
                switch (line.Substring(Settings.Default.DiscountTypeStart, Settings.Default.DiscountTypeLength))
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
            catch (ArgumentOutOfRangeException ex)
            {

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
