using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Line1(string line)
        {
            int intVal = 0;
            decimal decVal = 0;

            //Reference
            if (line.Length >= Settings.Default.ReferenceStart + Settings.Default.ReferenceLength)
            {
                Reference = line.Substring(Settings.Default.ReferenceStart, Settings.Default.ReferenceLength).Trim();
            }
            else if(line.Length >= Settings.Default.ReferenceStart)
            {
                Reference = line.Substring(Settings.Default.ReferenceStart).Trim();
            }

            //ArticleCertification
            if (line.Length >= Settings.Default.ArticleCertificationStart + Settings.Default.ArticleCertificationLength)
            {
                ArticleCertification = line.Substring(Settings.Default.ArticleCertificationStart, Settings.Default.ArticleCertificationLength).Trim();
            }
            else if(line.Length >= Settings.Default.ArticleCertificationStart)
            {
                ArticleCertification = line.Substring(Settings.Default.ArticleCertificationStart).Trim();
            }

            //isNZOKArticle
            if (line.Length >= Settings.Default.isNZOKArticleStart + Settings.Default.isNZOKArticleLength)
            {
                isNZOKArticle = Parse.StringToBool(line.Substring(Settings.Default.isNZOKArticleStart, Settings.Default.isNZOKArticleLength));                    
            }
            else if(line.Length >= Settings.Default.isNZOKArticleStart)
            {
                isNZOKArticle = Parse.StringToBool(line.Substring(Settings.Default.isNZOKArticleStart));
            }

            //NZOKArticleCode
            if (line.Length >= Settings.Default.NZOKArticleCodeStart + Settings.Default.NZOKArticleCodeLength)
            {
                NZOKArticleCode = line.Substring(Settings.Default.NZOKArticleCodeStart, Settings.Default.NZOKArticleCodeLength).Trim();
            }
            else if(line.Length >= Settings.Default.NZOKArticleCodeStart)
            {
                NZOKArticleCode = line.Substring(Settings.Default.NZOKArticleCodeStart).Trim();
            }

            //OrderQty
            if (line.Length >= Settings.Default.OrderQtyStart + Settings.Default.OrderQtyLength)
            {
                if(Int32.TryParse(line.Substring(Settings.Default.OrderQtyStart, Settings.Default.OrderQtyLength), out intVal))
                {
                    OrderQty = intVal;
                }
            }
            else if(line.Length >= Settings.Default.OrderQtyStart)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.OrderQtyStart), out intVal))
                {
                    OrderQty = intVal;
                }
            }

            //DeliveryQty
            if (line.Length >= Settings.Default.DeliveryQtyStart + Settings.Default.DeliveryQtyLength)
            {
                if(Int32.TryParse(line.Substring(Settings.Default.DeliveryQtyStart, Settings.Default.DeliveryQtyLength), out intVal))
                {
                    DeliveryQty = intVal;
                }
            }
            else if (line.Length >= Settings.Default.DeliveryQtyStart)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.DeliveryQtyStart), out intVal))
                {
                    DeliveryQty = intVal;
                }
            }

            //BonusQty
            if (line.Length >= Settings.Default.BonusQtyStart + Settings.Default.BonusQtyLength)
            {
                if(Int32.TryParse(line.Substring(Settings.Default.BonusQtyStart, Settings.Default.BonusQtyLength), out intVal))
                {
                    BonusQty = intVal;
                }
            }
            else if(line.Length >= Settings.Default.BonusQtyStart)
            {
                if (Int32.TryParse(line.Substring(Settings.Default.BonusQtyStart), out intVal))
                {
                    BonusQty = intVal;
                }
            }

            //PharmacySellPrice
            if (line.Length >= Settings.Default.PharmacySellPriceStart + Settings.Default.PharmacySellPriceLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.PharmacySellPriceStart, Settings.Default.PharmacySellPriceLength).Trim().Replace(',','.'), out decVal))
                {
                    PharmacySellPrice = decVal;
                }
            }
            else if (line.Length >= Settings.Default.PharmacySellPriceStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.PharmacySellPriceStart).Trim().Replace(',', '.'), out decVal))
                {
                    PharmacySellPrice = decVal;
                }
            }

            //InvoicedPriceInclVATNoDiscount
            if (line.Length >= Settings.Default.InvoicedPriceInclVATNoDiscountStart + Settings.Default.InvoicedPriceInclVATNoDiscountLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATNoDiscountStart, Settings.Default.InvoicedPriceInclVATNoDiscountLength).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceInclVATNoDiscount = decVal;
                }
            }
            else if(line.Length >= Settings.Default.InvoicedPriceInclVATNoDiscountStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATNoDiscountStart).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceInclVATNoDiscount = decVal;
                }
            }

            //InvoicedPriceExclVAT
            if (line.Length >= Settings.Default.InvoicedPriceExclVATStart + Settings.Default.InvoicedPriceExclVATLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceExclVATStart, Settings.Default.InvoicedPriceExclVATLength).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceExclVAT = decVal;
                }
            }
            else if(line.Length >= Settings.Default.InvoicedPriceExclVATStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceExclVATStart).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceExclVAT = decVal;
                }
            }

            //InvoicedPriceInclVAT
            if (line.Length >= Settings.Default.InvoicedPriceInclVATStart + Settings.Default.InvoicedPriceInclVATLength)
            {
                if(Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATStart, Settings.Default.InvoicedPriceInclVATLength).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceInclVAT = decVal;
                }
            }
            else if(line.Length >= Settings.Default.InvoicedPriceInclVATStart)
            {
                if (Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceInclVATStart).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPriceInclVAT = decVal;
                }
            }
        }
    }
}
