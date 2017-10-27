using System;
using System.Collections;
using System.Reflection;

namespace DelNoteItems
{
    public partial class Position : DelNoteItems
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
        public decimal? PharmacySellPrice { get; set; }                 //BG: Цена Аптека
        public decimal? InvoicedPriceInclVATNoDiscount { get; set; }
        public decimal? InvoicedPriceExclVAT { get; set; }
        public decimal? InvoicedPriceInclVAT { get; set; }
        
        //$$POS2$$ Line properties
        public DateTime? ExpiryDate { get; set; }
        public string Batch { get; set; }
        
        //$$POS3$$ Line properties
        public string ArticleLongName { get; set; }

        //$$POS4$$ Line properties
        public string PharmaceuticalForm { get; set; }
        public decimal? WholesalePurchasePrice { get; set; }            //GEP       BG: Базова Цена
        public decimal? PharmacyPurchasePrice { get; set; }             //AEP       BG: ЦенаТЕ
        public decimal? MaxPharmacySalesPrice { get; set; }             //MAXAVP    BG: Пределна Цена

        public int? PriceType { get; set; }


        //$$POS5$$ Line properties
        public string ArticleRemark { get; set; }

        public Position(string[] lines, bool isCreditNote)
        {
            try
            {
                if (isCreditNote)
                {
                    InitializeCreditNote(lines);
                }
                else
                {
                    InitializeInvoice(lines);
                }
            }
            catch (Exception)
            {

            }
        }

        private void InitializeInvoice(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("$$POS$$"))
                {
                    try
                    {
                        Line0(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS1$"))
                {
                    try
                    {
                        Line1(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS2$$"))
                {
                    try
                    {
                        Line2(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS3$"))
                {
                    try
                    {
                        Line3(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS4$$"))
                {
                    try
                    {
                        Line4(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS5$$"))
                {
                    try
                    {
                        Line5(line);
                    }
                    catch (NotImplementedException) { }
                }
            }
        }
        private void InitializeCreditNote(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("$$POS$$"))
                {
                    try
                    {
                        Line0(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS1$$"))
                {
                    try
                    {
                        Line1(RemoveSymbol(line));
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS2$$"))
                {
                    try
                    {
                        Line2(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS3$$"))
                {
                    try
                    {
                        Line3(RemoveSymbol(line));
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS4$$"))
                {
                    try
                    {
                        Line4(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS5$$"))
                {
                    try
                    {
                        Line5(line);
                    }
                    catch (NotImplementedException) { }
                }
            }
        }

        private string RemoveSymbol(string line)
        {
            string str = line.Substring(0, 7) + line.Substring(8);
            return str;
        }
    }
}
