using System;
using Settings = DelNoteItems.Properties.Settings1;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Pos(string line)
        {
            try
            {
                int intVal = 0;
                long longVal = 0;
                decimal decVal = 0;
                if (line.Length >= Settings.Default.ArticleNoStart + Settings.Default.ArticleNoLength
                    && Int32.TryParse(line.Substring(Settings.Default.ArticleNoStart, Settings.Default.ArticleNoLength).Trim(), out intVal))
                {
                    ArticleNo = intVal;
                }

                if (line.Length >= Settings.Default.EANStart + Settings.Default.EANLength
                    && Int64.TryParse(line.Substring(Settings.Default.EANStart, Settings.Default.EANLength).Trim(), out longVal))
                {
                    EAN = longVal;
                }

                if (line.Length >= Settings.Default.ArticleNameStart + Settings.Default.ArticleNameLength)
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

                if (line.Length >= Settings.Default.InvoicedPriceStart + Settings.Default.InvoicedPriceLength
                    && Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceStart, Settings.Default.InvoicedPriceLength).Trim().Replace(',', '.'), out decVal))
                {
                    InvoicedPrice = decVal;
                }

                if (line.Length >= Settings.Default.DiscountPercentageStart + Settings.Default.DiscountPercentageLength
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
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
