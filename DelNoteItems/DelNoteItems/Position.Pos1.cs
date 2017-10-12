using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Pos1(string line)
        {
            try
            {
                int intVal = 0;
                decimal decVal = 0;

                if (line.Length >= Settings.Default.ReferenceStart + Settings.Default.ReferenceLength)
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

                if (line.Length >= Settings.Default.NZOKArticleCodeStart + Settings.Default.NZOKArticleCodeLength)
                {
                    NZOKArticleCode = line.Substring(Settings.Default.NZOKArticleCodeStart, Settings.Default.NZOKArticleCodeLength).Trim();
                }

                if (line.Length >= Settings.Default.OrderQtyStart + Settings.Default.OrderQtyLength
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

                if (line.Length >= Settings.Default.PharmacyPurchasePriceStart + Settings.Default.PharmacyPurchasePriceLength
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
    }
}
