using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public partial class Position
    {
        private void Line4(string line)
        {
            int intVal;
            decimal decVal;
            try
            {
                //PharmaceuticalForm
                if (line.Length >= Settings.Default.PharmaceuticalFormStart + Settings.Default.PharmaceuticalFormLength)
                {
                    PharmaceuticalForm = line.Substring(Settings.Default.PharmaceuticalFormStart, Settings.Default.PharmaceuticalFormLength).Trim();
                }
                else if (line.Length >= Settings.Default.PharmaceuticalFormStart)
                {
                    PharmaceuticalForm = line.Substring(Settings.Default.PharmaceuticalFormStart).Trim();
                }

                //WholesalePurchasePrice
                if (line.Length >= Settings.Default.WholesalePurchasePriceStart + Settings.Default.WholesalePurchasePriceLength)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.WholesalePurchasePriceStart, Settings.Default.WholesalePurchasePriceLength).Trim().Replace(',', '.'), out decVal))
                    {
                        WholesalePurchasePrice = decVal;
                    }
                }
                else if (line.Length >= Settings.Default.WholesalePurchasePriceStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.WholesalePurchasePriceStart).Trim().Replace(',', '.'), out decVal))
                    {
                        WholesalePurchasePrice = decVal;
                    }
                }

                //PharmacyPurchasePrice
                if (line.Length >= Settings.Default.PharmacyPurchasePriceStart + Settings.Default.PharmacyPurchasePriceLength)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.PharmacyPurchasePriceStart, Settings.Default.PharmacyPurchasePriceLength).Trim().Replace(',', '.'), out decVal))
                    {
                        PharmacyPurchasePrice = decVal;
                    }
                }
                else if (line.Length >= Settings.Default.PharmacyPurchasePriceStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.PharmacyPurchasePriceStart).Trim().Replace(',', '.'), out decVal))
                    {
                        PharmacyPurchasePrice = decVal;
                    }
                }

                //MaxPharmacySalesPrice
                if (line.Length >= Settings.Default.MaxPharmacySalesPriceStart + Settings.Default.MaxPharmacySalesPriceLength)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.MaxPharmacySalesPriceStart, Settings.Default.MaxPharmacySalesPriceLength).Trim().Replace(',', '.'), out decVal))
                    {
                        MaxPharmacySalesPrice = decVal;
                    }
                }
                else if (line.Length >= Settings.Default.MaxPharmacySalesPriceStart)
                {
                    if (Decimal.TryParse(line.Substring(Settings.Default.MaxPharmacySalesPriceStart).Trim().Replace(',', '.'), out decVal))
                    {
                        MaxPharmacySalesPrice = decVal;
                    }
                }

                //Reserved for the "Empty Space" mentioned by Rolf Raab

                //PriceType
                if (line.Length >= Settings.Default.PriceTypeStart + Settings.Default.PriceTypeLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PriceTypeStart, Settings.Default.PriceTypeLength).Trim(), out intVal))
                    {
                        PriceType = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.PriceTypeStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PriceTypeStart).Trim(), out intVal))
                    {
                        PriceType = intVal;
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
