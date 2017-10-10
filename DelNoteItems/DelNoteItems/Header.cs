using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Header
    {
        //$$Header$$ Line properties
        public long? DeliveryNoteNumber { get; set; }        //DelNoteNo
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
            DateTime dateVal;
            try
            {
                try
                {
                    if (Int64.TryParse(line.Substring(Settings.Default.DelNoteNoStart, Settings.Default.DelNoteNoLength), out longVal))
                    {
                        DeliveryNoteNumber = longVal;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(
                        $"You're searching for the " +
                        $"{Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength}th index from a " +
                        $"{line.Length} chars long string! There is no such index! -> \"DeliveryNoteNumber\"");
                }
                try
                {
                    if (DateTime.TryParse(line.Substring(Settings.Default.DelNoteDateStart, Settings.Default.DelNoteDateLength), out dateVal))
                    {
                        DeliveryNoteDate = dateVal;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(
                        $"You're searching for the " +
                        $"{Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength}th index from a " +
                        $"{line.Length} chars long string! -> \"DeliveryNoteDate\"");
                }
                try
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.RebateOrderStart, Settings.Default.RebateOrderLength), out intVal))
                    {
                        RebateInKindOrder = IntToBool(intVal);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(
                        $"You're searching for the " +
                        $"{Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength}th index from a " +
                        $"{line.Length} chars long string! -> \"RebateInKindOrder\"");
                }
                try
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.isNZOKOrderStart, Settings.Default.isNZOKOrderLength), out intVal))
                    {
                        isNZOK = IntToBool(intVal);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(
                        $"You're searching for the " +
                        $"{Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength}th index from a " +
                        $"{line.Length} chars long string! -> \"isNZOKOrder\"");
                }
                try
                {
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
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(
                        $"You're searching for the " +
                        $"{Settings.Default.DelNoteNoStart + Settings.Default.DelNoteNoLength}th index from a " +
                        $"{line.Length} chars long string! -> \"DiscountType\"");
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
