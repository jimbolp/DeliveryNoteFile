using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using DelNoteItems;

namespace DeliveryNoteFiles
{
    partial class Program
    {
        private static Stopwatch update = new Stopwatch();
        private static bool UpdateExistingDelNote(int DelNoteID, DeliveryNoteFile delNoteFile)
        {
            bool InsertCompleted = false;
            DelNote dNote = db.DelNotes.Find(DelNoteID);
            if (dNote == null)
                return InsertCompleted;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    UpdateDelNote(dNote, delNoteFile);
                    List<int> items = db.DelNoteItems.Where(di => di.DelNoteID == DelNoteID).Select(di => di.ID).ToList();
                    if (delNoteFile.Positions == null || delNoteFile.Positions.Count == 0)
                    {
                        if (items != null && items.Count != 0)
                        {
                            db.Database.ExecuteSqlCommandAsync("delete from dbo.DelNoteItems where DelNoteID = @delnoteid", new SqlParameter("@delnoteid", DelNoteID));
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        InsertCompleted = true;
                    }
                    else
                    {
                        if (items != null && items.Count > delNoteFile.Positions.Count)
                        {
                            db.Database.ExecuteSqlCommandAsync("delete from dbo.DelNoteItems where DelNoteID = @delnoteid", new SqlParameter("@delnoteid", DelNoteID));
                        }
                        int count = (items == null ? 0 : items.Count);
                        for (int i = 0; i < count; ++i)
                        {
                            DelNoteItem dNoteItem = db.DelNoteItems.Find(items[i]);

                            UpdateDelNoteItem(dNoteItem, delNoteFile.Positions[i]);
                            delNoteFile.Positions[i] = null;
                        }//*/
                        delNoteFile.Positions.RemoveAll(p => p == null);
                        if (delNoteFile.Positions.Count != 0)
                        {
                            db.DelNoteItems.AddRange(CreateListOfDelNoteItems(DelNoteID, delNoteFile));
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        InsertCompleted = true;
                    }
                }
                catch (DbEntityValidationException e)
                {
                    InsertCompleted = false;
                    foreach (DbEntityValidationResult err in e.EntityValidationErrors)
                    {
                        foreach (DbValidationError err1 in err.ValidationErrors)
                        {
                            Console.WriteLine(err1.ErrorMessage);
                        }
                    }
                    transaction.Rollback();
                }
                catch (DbUpdateException ue)
                {
                    InsertCompleted = false;
                    Console.WriteLine(ue.ToString());
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    InsertCompleted = false;
                    Console.WriteLine(e.ToString()); 
                    transaction.Rollback();
                }
            }
            return InsertCompleted;
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
            delNoteItem.GroupID = GetOverrateGroupID(pos.ArticleNo.Value);//*/
        }

        private static void UpdateDelNote(DelNote dNote, DeliveryNoteFile delNote)
        {
            string creditNoteDescr = "";
            if (delNote.Header.CreditNoteType == "ФР" || delNote.Header.CreditNoteType == "FR")
            {
                creditNoteDescr += delNote.Positions.FirstOrDefault().ArticleName;
                if(!string.IsNullOrEmpty(delNote.Positions.FirstOrDefault().ArticleRemark))
                    creditNoteDescr += " / " + delNote.Positions.FirstOrDefault().ArticleRemark;
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
