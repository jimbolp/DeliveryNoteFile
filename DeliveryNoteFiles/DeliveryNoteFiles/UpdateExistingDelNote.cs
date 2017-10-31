using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using DelNoteItems;

namespace DeliveryNoteFiles
{
    partial class Program
    {
        private static void UpdateExistingDelNote(int ID, DeliveryNoteFile delNote)
        {
            DelNote dNote = db.DelNotes.Find(ID);
            if (delNote.Positions == null || dNote == null)
                return;
            
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    UpdateDelNote(dNote, delNote);
                    List<DelNoteItem> items = db.DelNoteItems.Where(di => di.DelNoteID == ID).ToList();
                    if (items.Count > delNote.Positions.Count)
                        return;
                    for (int i = 0; i < items.Count; i++)
                    {
                        UpdateDelNoteItem(items[i], delNote.Positions[0]);
                        delNote.Positions.RemoveAt(0);
                    }
                    if (delNote.Positions.Count != 0)
                    {
                        db.DelNoteItems.AddRange(AddDelNoteItems(ID, delNote));
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var err in e.EntityValidationErrors)
                    {
                        foreach (var err1 in err.ValidationErrors)
                        {
                            Console.WriteLine(err1.ErrorMessage);
                        }

                    }

                    transaction.Rollback();
                }
                catch (DbUpdateException ue)
                {
                    Console.WriteLine(ue.ToString());
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString()); 
                    transaction.Rollback();
                }
            }
        }

        private static void UpdateDelNoteItem(DelNoteItem delNoteItem, Position pos)
        {
            string expiryDate = "";
            if (pos.ExpiryDate != null)
            {
                expiryDate = pos.ExpiryDate.Value.ToString("yyyyMMdd");
            }
            delNoteItem.ArticlePZN = pos.ArticleNo;
            delNoteItem.ArticleLongName = pos.ArticleLongName;
            delNoteItem.DelQty = pos.DeliveryQty;
            delNoteItem.BonusQty = pos.BonusQty;
            delNoteItem.PharmacyPurchasePrice = pos.PharmacyPurchasePrice;
            delNoteItem.DiscountPercentage = pos.DiscountPercentage;
            delNoteItem.InvoicedPrice = pos.InvoicedPrice;
            delNoteItem.InvoicedPriceExclVAT = pos.InvoicedPriceExclVAT;
            delNoteItem.InvoicedPriceInclVAT = pos.InvoicedPriceInclVAT;
            delNoteItem.ParcelNo = pos.Batch;
            delNoteItem.Certification = pos.ArticleCertification;
            delNoteItem.ExpiryDate = expiryDate;
            delNoteItem.PharmacySellPrice = pos.PharmacySellPrice;
            delNoteItem.BasePrice = pos.WholesalePurchasePrice;
            delNoteItem.InvoicePriceNoDisc = pos.InvoicedPriceInclVATNoDiscount;
            delNoteItem.RetailerMaxPrice = pos.MaxPharmacySalesPrice;
            delNoteItem.GroupID = GetOverrateGroupID(pos.ArticleNo.Value);
        }

        private static void UpdateDelNote(DelNote dNote, DeliveryNoteFile delNote)
        {
            string creditNoteDescr = "";
            if (delNote.DocType.isCreditNote && (delNote.Positions != null && delNote.Positions.Count > 0))
            {
                creditNoteDescr = delNote.Positions.FirstOrDefault().ArticleName;
            }
            dNote.FileName = delNote.FileName;
            dNote.ProcessTime = DateTime.Now;
            dNote.CustomerIDF = delNote.Header.PharmacyID.ToString();
            dNote.DocNo = delNote.Header.DeliveryNoteNumber.ToString();
            dNote.DocDate = delNote.Header.DeliveryNoteDate;
            dNote.PaymentSum = delNote.Footer.InvoiceTotal;
            dNote.TotalDiscounts = delNote.Footer.TotalDiscounts;
            dNote.CreditNoteType = delNote.Header.CreditNoteType;
            dNote.CreditNoteDescr = creditNoteDescr;
            dNote.ShipmentDate = delNote.Tour.TourDate;
            dNote.RouteID = delNote.Tour.TourID;
            dNote.VatPercent = delNote.VATTable.TotalPercent;
            dNote.PaymentTimeID = delNote.PaymentTimeID;
            dNote.PaymentConsignDate = delNote.Footer.DueDate;
            dNote.isNZOK = delNote.Header.isNZOKOrder;
            dNote.isRebateDiscount = delNote.Header.RebateInKindOrder;
            dNote.KSCOrderNo = delNote.Header.CSCOrderNumber.ToString();
        }
    }
}
