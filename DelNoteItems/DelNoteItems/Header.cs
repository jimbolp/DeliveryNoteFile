﻿using System;
using System.Collections;
using System.Reflection;

namespace DelNoteItems
{
    public partial class Header : DelNoteItems
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
        public int PharmacyID { get; set; }                 //Pharmos Customer Number!!!

        //$$Header2$$ Line properties
        public string OrderType { get; set; }
        public string UserName { get; set; }            //Pharmos initials of the user created the document (usualy this is only for Credit Notes)
        public bool? DiscountOnInvoice { get; set; }    //(J/N)
        public string OrderRemark { get; set; }
        public int? PickingType { get; set; }
        public int? BookingType { get; set; }
        public int? CSCOrderNumber { get; set; }        //Order Number in KSC/Order Entry

        public Header(string[] lines, bool isCreditNote)
        {
            if (isCreditNote)
            {
                foreach (string line in lines)
                {
                    InitializeCreditNote(line);
                }
            }
            else
            {
                foreach (string line in lines)
                {
                    InitializeInvoice(line);
                }
            }
            
        }

        private void InitializeInvoice(string line)
        {
            if (line.StartsWith("$$HEADER$$"))
            {
                Line1(line);
            }
            else if (line.StartsWith("$$HEADER2$$"))
            {
                Line2(line);
            }
        }
        private void InitializeCreditNote(string line)
        {
            //FixLine(line);
            InitializeInvoice(line);
        }

#if DEBUG
        /*public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
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
        }//*/ 
#endif


    }
}
