using System;
using Settings = DelNoteItems.Properties.Config;

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

                //ArticleNo
                if (line.Length >= Settings.Default.ArticleNoStart + Settings.Default.ArticleNoLength)
                {
                    if(Int32.TryParse(line.Substring(Settings.Default.ArticleNoStart, Settings.Default.ArticleNoLength).Trim(), out intVal))
                    {
                        ArticleNo = intVal;
                    }
                }
                else if(line.Length >= Settings.Default.ArticleNoStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.ArticleNoStart).Trim(), out intVal))
                    {
                        ArticleNo = intVal;
                    }
                }

                //EAN
                if (line.Length >= Settings.Default.EANStart + Settings.Default.EANLength)
                {
                    if(Int64.TryParse(line.Substring(Settings.Default.EANStart, Settings.Default.EANLength).Trim(), out longVal))
                    {
                        EAN = longVal;
                    }
                }
                else if(line.Length >= Settings.Default.EANStart)
                {
                    if (Int64.TryParse(line.Substring(Settings.Default.EANStart).Trim(), out longVal))
                    {
                        EAN = longVal;
                    }
                }

                //ArticleName
                if (line.Length >= Settings.Default.ArticleNameStart + Settings.Default.ArticleNameLength)
                {
                    ArticleName = line.Substring(Settings.Default.ArticleNameStart, Settings.Default.ArticleNameLength).Trim();
                }
                else if(line.Length >= Settings.Default.ArticleNameStart)
                {
                    ArticleName = line.Substring(Settings.Default.ArticleNameStart).Trim();
                }
                
                //ArticleUnit
                if (line.Length >= Settings.Default.ArticleUnitStart + Settings.Default.ArticleUnitLength)
                {
                    ArticleUnit = line.Substring(Settings.Default.ArticleUnitStart, Settings.Default.ArticleUnitLength).Trim();
                }
                else if(line.Length >= Settings.Default.ArticleUnitStart)
                {
                    ArticleUnit = line.Substring(Settings.Default.ArticleUnitStart).Trim();
                }

                //InvoicedQty
                if (line.Length >= Settings.Default.InvoicedQtyStart + Settings.Default.InvoicedQtyLength)
                { 
                    if(Int32.TryParse(line.Substring(Settings.Default.InvoicedQtyStart, Settings.Default.InvoicedQtyLength).Trim(), out intVal))
                    {
                        InvoicedQty = intVal;
                    }
                }
                else if(line.Length >= Settings.Default.InvoicedQtyStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.InvoicedQtyStart).Trim(), out intVal))
                    {
                        InvoicedQty = intVal;
                    }
                }

                //InvoicedPrice
                if (line.Length >= Settings.Default.InvoicedPriceStart + Settings.Default.InvoicedPriceLength)
                {
                    if(Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceStart, Settings.Default.InvoicedPriceLength).Trim().Replace(',', '.'), out decVal))
                    {
                        InvoicedPrice = decVal;
                    }
                }
                else if(line.Length >= Settings.Default.InvoicedPriceStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.InvoicedPriceStart).Trim().Replace(',', '.'), out decVal))
                    {
                        InvoicedPrice = decVal;
                    }
                }

                //DiscountPercentage
                if (line.Length >= Settings.Default.DiscountPercentageStart + Settings.Default.DiscountPercentageLength)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.DiscountPercentageStart, Settings.Default.DiscountPercentageLength).Trim().Replace(',', '.'), out decVal))
                    {
                        DiscountPercentage = decVal;
                    }
                }
                else if(line.Length >= Settings.Default.DiscountPercentageStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.DiscountPercentageStart).Trim().Replace(',', '.'), out decVal))
                    {
                        DiscountPercentage = decVal;
                    }
                }

                //ArticleVATPercentage
                if (line.Length >= Settings.Default.ArticleVATPercentageStart + Settings.Default.ArticleVATPercentageLength)
                {
                    if(Decimal.TryParse(line.Substring(Settings.Default.ArticleVATPercentageStart, Settings.Default.ArticleVATPercentageLength).Trim().Replace(',', '.'), out decVal))
                    {
                        ArticleVATPercentage = decVal;
                    }
                }
                else if(line.Length >= Settings.Default.ArticleVATPercentageStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.ArticleVATPercentageStart).Trim().Replace(',', '.'), out decVal))
                    {
                        ArticleVATPercentage = decVal;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
