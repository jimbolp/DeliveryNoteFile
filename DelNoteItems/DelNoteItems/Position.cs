using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public partial class Position
    {
        //$$POS$$ Line properties        
        public int? ArticleNo { get; set; }
        public long? EAN { get; set; }
        public string ArticleName { get; set; }
        public string ArticleUnit { get; set; }
        public int? InvoicedQty { get; set; }
        public decimal? InvoicedPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? ArticleVATPercentage { get; set; }
        
        //$$POS1$$ Line properties
        public string Reference { get; set; }
        public string ArticleCertification { get; set; }
        public bool? isNZOKArticle { get; set; }
        public string NZOKArticleCode { get; set; }
        public int? OrderQty { get; set; }
        public int? DeliveryQty { get; set; }
        public int? BonusQty { get; set; }
        public decimal? PharmacyPurchasePrice { get; set; }
        public decimal? InvoicedPriceInclVATNoDiscount { get; set; }
        public decimal? InvoicedPriceExclVAT { get; set; }
        public decimal? InvoicedPriceInclVAT { get; set; }
        
        //$$POS2$$ Line properties
        public DateTime? ExpiryDate { get; set; }
        public string Batch { get; set; }
        
        //$$POS3$$ Line properties
        public string ArticleLongName { get; set; }

        //$$POS4$$ Line - still no explanation about this one!

        public Position(string line)
        {

        }
        public Position(string[] lines)
        {
            foreach(string line in lines)
            {
                if (line.StartsWith("$$POS$$"))
                {
                    Pos(line);
                }
                else if (line.StartsWith("$$POS1$"))
                {
                    Pos1(line);
                }
                else if (line.StartsWith("$$POS2$$"))
                {
                    Pos2(line);
                }
                else if (line.StartsWith("$$POS3$"))
                {
                    Pos3(line);
                }
                else if (line.StartsWith("$$POS4$$"))
                {
                    Pos4(line);
                }
            }
        }
    }
}
