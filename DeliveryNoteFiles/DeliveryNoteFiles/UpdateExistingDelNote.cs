using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using DelNoteItems;

namespace DeliveryNoteFiles
{
    partial class Program
    {
        private static Stopwatch update = new Stopwatch();
        private static void UpdateExistingDelNote(int ID, DeliveryNoteFile delNoteFile)
        {
            update.Reset();
            DelNote dNote = db.DelNotes.Find(ID);
            if (dNote == null)
                return;
            if (delNoteFile.Positions == null)
            {
                UpdateDelNote(dNote, delNoteFile);
                db.SaveChanges();
                return;
            }
            
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    update.Start();
                    IQueryable<int> items = db.DelNoteItems.Where(di => di.DelNoteID == ID).Select(di => di.ID);
                    if (items.Count() > delNoteFile.Positions.Count)
                        return;
                    foreach (int item in items)
                    { 
                        DelNoteItem dNoteItem = db.DelNoteItems.Find(item);
                        
                        UpdateDelNoteItem(dNoteItem, delNoteFile.Positions.First.Value);
                        delNoteFile.Positions.RemoveFirst();
                        
                    }
                    //delNoteFile.Positions.RemoveAll(p => p == null);
                    if (delNoteFile.Positions.Count != 0)
                    {
                        db.DelNoteItems.AddRange(AddDelNoteItems(ID, delNoteFile));
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    update.Stop();
                    Console.WriteLine(update.Elapsed);
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
