using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public class Position
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
        //.Trim().Replace(',','.')
        
        private void Pos(string line)
        {
            try
            {
                int intVal = 0;
                long longVal = 0;
                decimal decVal = 0;
                if(line.Length >= Settings.Default.ArticleNoStart + Settings.Default.ArticleNoLength
                    && Int32.TryParse(line.Substring(Settings.Default.ArticleNoStart, Settings.Default.ArticleNoLength).Trim(), out intVal))
                {
                    ArticleNo = intVal;
                }

                if (line.Length >= Settings.Default.EANStart + Settings.Default.EANLength
                    && Int64.TryParse(line.Substring(Settings.Default.EANStart, Settings.Default.EANLength).Trim(), out longVal))
                {
                    EAN = longVal;
                }

                if(line.Length >= Settings.Default.ArticleNameStart + Settings.Default.ArticleNameLength)
                {
                    ArticleName = line.Substring(Settings.Default.ArticleNameStart, Settings.Default.ArticleNameLength).Trim();
                }

                if (line.Length >= Settings.Default.ArticleUnitStart + Settings.Default.ArticleUnitLength)
                {
                    ArticleUnit = line.Substring(Settings.Default.ArticleUnitStart, Settings.Default.ArticleUnitLength).Trim();
                }

                if (line.Length >= Settings.Default.InvoicedQtyStart + Settings.Default.InvoicedQtyLength
                    && Int32.TryParse(line.Substring(Settings.Default.InvoicedQtyStart, Settings.Default.InvoicedQtyLength).Trim(), out intVal))
                {
                    InvoicedQty = intVal;
                }

                if(line.Length >= Settings.Default.InvoicedPriceStart + Settings.Default.InvoicedPriceLength
                    && Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceStart, Settings.Default.InvoicedPriceLength).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPrice = decVal;
                }

                if(line.Length >= Settings.Default.DiscountPercentageStart + Settings.Default.DiscountPercentageLength
                    && Decimal.TryParse(line.Substring(Settings.Default.DiscountPercentageStart, Settings.Default.DiscountPercentageLength).Trim().Replace(',', '.'), out decVal))
                {
                    DiscountPercentage = decVal;
                }

                if (line.Length >= Settings.Default.ArticleVATPercentageStart + Settings.Default.ArticleVATPercentageLength
                    && Decimal.TryParse(line.Substring(Settings.Default.ArticleVATPercentageStart, Settings.Default.ArticleVATPercentageLength).Trim().Replace(',', '.'), out decVal))
                {
                    ArticleVATPercentage = decVal;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        private void Pos1(string line)
        {
            try
            {
                int intVal = 0;
                decimal decVal = 0;

                if(line.Length >= Settings.Default.ReferenceStart + Settings.Default.ReferenceLength)
                {
                    Reference = line.Substring(Settings.Default.ReferenceStart, Settings.Default.ReferenceLength).Trim();
                }

                if (line.Length >= Settings.Default.ArticleCertificationStart + Settings.Default.ArticleCertificationLength)
                {
                    ArticleCertification = line.Substring(Settings.Default.ArticleCertificationStart, Settings.Default.ArticleCertificationLength).Trim();
                }

                if (line.Length >= Settings.Default.isNZOKArticleStart + Settings.Default.isNZOKArticleLength)
                {
                    switch (line.Substring(Settings.Default.isNZOKArticleStart, Settings.Default.isNZOKArticleLength).Trim())
                    {
                        case "J":
                            isNZOKArticle = true;
                            break;
                        case "N":
                            isNZOKArticle = false;
                            break;
                        default:
                            isNZOKArticle = null;
                            break;
                    }
                }

                if(line.Length >= Settings.Default.NZOKArticleCodeStart + Settings.Default.NZOKArticleCodeLength)
                {
                    NZOKArticleCode = line.Substring(Settings.Default.NZOKArticleCodeStart, Settings.Default.NZOKArticleCodeLength).Trim();
                }

                if(line.Length >= Settings.Default.OrderQtyStart + Settings.Default.OrderQtyLength
                    && Int32.TryParse(line.Substring(Settings.Default.OrderQtyStart, Settings.Default.OrderQtyLength), out intVal))
                {
                    OrderQty = intVal;
                }

                if (line.Length >= Settings.Default.DeliveryQtyStart + Settings.Default.DeliveryQtyLength
                    && Int32.TryParse(line.Substring(Settings.Default.DeliveryQtyStart, Settings.Default.DeliveryQtyLength), out intVal))
                {
                    DeliveryQty = intVal;
                }

                if (line.Length >= Settings.Default.BonusQtyStart + Settings.Default.BonusQtyLength
                    && Int32.TryParse(line.Substring(Settings.Default.BonusQtyStart, Settings.Default.BonusQtyLength), out intVal))
                {
                    BonusQty = intVal;
                }

                if(line.Length >= Settings.Default.PharmacyPurchasePriceStart + Settings.Default.PharmacyPurchasePriceLength
                    && Decimal.TryParse(line.Substring(Settings.Default.PharmacyPurchasePriceStart, Settings.Default.PharmacyPurchasePriceLength), out decVal))
                {
                    PharmacyPurchasePrice = decVal;
                }

                if (line.Length >= Settings.Default.InvoicedPriceInclVATNoDiscountStart + Settings.Default.InvoicedPriceInclVATNoDiscountLength
                    && Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATNoDiscountStart, Settings.Default.InvoicedPriceInclVATNoDiscountLength), out decVal))
                {
                    InvoicedPriceInclVATNoDiscount = decVal;
                }

                if (line.Length >= Settings.Default.InvoicedPriceExclVATStart + Settings.Default.InvoicedPriceExclVATLength
                    && Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceExclVATStart, Settings.Default.InvoicedPriceExclVATLength), out decVal))
                {
                    InvoicedPriceExclVAT = decVal;
                }

                if (line.Length >= Settings.Default.InvoicedPriceInclVATStart + Settings.Default.InvoicedPriceInclVATLength
                    && Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATStart, Settings.Default.InvoicedPriceInclVATLength), out decVal))
                {
                    InvoicedPriceInclVAT = decVal;
                }


            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void Pos2(string line)
        {

        }
        private void Pos3(string line)
        {

        }
        private void Pos4(string line)
        {

        }
    }
}
