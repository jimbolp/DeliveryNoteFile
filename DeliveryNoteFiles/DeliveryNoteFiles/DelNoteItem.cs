//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliveryNoteFiles
{
    using System;
    using System.Collections.Generic;
    
    public partial class DelNoteItem
    {
        public int ID { get; set; }
        public Nullable<int> DelNoteID { get; set; }
        public Nullable<int> ArticlePZN { get; set; }
        public string ArticleLongName { get; set; }
        public Nullable<int> DelQty { get; set; }
        public Nullable<int> BonusQty { get; set; }
        public Nullable<decimal> PharmacyPurchasePrice { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> InvoicedPrice { get; set; }
        public Nullable<decimal> InvoicedPriceExclVAT { get; set; }
        public Nullable<decimal> InvoicedPriceInclVAT { get; set; }
        public string ParcelNo { get; set; }
        public string Certification { get; set; }
        public string ExpiryDate { get; set; }
        public Nullable<decimal> PharmacySellPrice { get; set; }
        public Nullable<decimal> BasePrice { get; set; }
        public Nullable<decimal> InvoicePriceNoDisc { get; set; }
        public Nullable<decimal> RetailerMaxPrice { get; set; }
        public Nullable<byte> GroupID { get; set; }
    }
}