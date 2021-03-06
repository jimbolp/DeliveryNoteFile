﻿using System;
using System.Collections;
using System.Reflection;

namespace DelNoteItems
{
    public partial class Header : DelNoteItems
    {
        //$$Header$$ Line properties
        public long? DeliveryNoteNumber { get; set; }       //Invoice Number
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
        public string CreditNoteType { get; set; }

        public Header(string[] lines, bool isCreditNote)
        {
            try
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
            catch (Exception e)
            {
                WriteExceptionToLog(e);
                throw e;
            }
        }

        private void InitializeInvoice(string line)
        {
            if (string.IsNullOrEmpty(line))
                return;
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
    }
}
