using System;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Settings = DelNoteItems.Properties.Settings;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DeliveryNoteFiles
{
    class Program
    {
        private static List<DeliveryNoteFile> DelNoteFiles = new List<DeliveryNoteFile>();
        private static DeliveryNoteEntities db = new DeliveryNoteEntities();
        static void Main(string[] args)
        {            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Home
            //string[] files = Directory.GetFiles(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

            //Work - Server 22
            //string[] files = Directory.GetFiles(@"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171026");

            //Work - Special files for tests
            string[] files = Directory.GetFiles(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

            //Work - One file only
            //string[] files = Directory.GetFiles(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\One File");

            File.WriteAllText(Settings.Default.ChangedPosFilePath, "");
            if (args != null && args.Length != 0)
            {
                int i = 0;
                foreach (var s in args)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        Console.WriteLine(s);
                        i++;
                        if (i >= 100)
                        {
                            break;
                            //DelNoteFiles = new List<DeliveryNoteFile>();
                            //i = 0;
                        }
                        
                        if (i % 50 == 0)
                        {
                            Console.WriteLine(i);
                            Thread.Sleep(1);
                        }
                        try
                        {
                            Thread t = new Thread(() => ProcessFile(s));
                            t.Start();
                            t.Join();//*/
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }
            else
            {
                int i = 0;
                int output = i;
                foreach (var s in files)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        //ProcessFile(s);
                        i++;
                        output++;
                        if (i >= 200)
                        {
                            break;
                            //DelNoteFiles = new List<DeliveryNoteFile>();
                            //i = 0;
                        }
                        
                        if (i % 50 == 0)
                        {
                            Console.WriteLine(output);
                            Thread.Sleep(1);
                        }
                        //    break;
                        Thread t = new Thread(() => ProcessFile(s));
                        t.Start();
                        t.Join();//*/
                    }
                }
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());
#if DEBUG
            //Files selected for debigging purposes...
            DeliveryNoteFile[] test2 = DelNoteFiles.Where(d => d.Header.OrderType == "FC").ToArray();
            DeliveryNoteFile[] test3 = DelNoteFiles.Where(d => d.Header.isNZOKOrder ?? false).ToArray();

            var withPositiions = DelNoteFiles.Where(d => d.Positions != null);
            DeliveryNoteFile[] test4 = withPositiions.Where(p => p.Positions.Any(pos => pos.isNZOKArticle ?? false)).ToArray();
            
            DeliveryNoteFile[] test6 = DelNoteFiles.Where(d => d.Positions != null).Where(d => d.Positions.Any(p => p.MaxPharmacySalesPrice != null)).ToArray();
            DeliveryNoteFile[] test7 = DelNoteFiles.Where(d => d.Mail != null).Where(d => (d.Mail.ValueOfFieldInSK17 != null && d.Mail.ValueOfFieldInSK17 != "0" && d.Mail.ValueOfFieldInSK17 != "5")).ToArray();
            DeliveryNoteFile[] test8 = DelNoteFiles.Where(d => d.Tour.TourDate == d.Footer.DueDate).ToArray();
#endif

            //Home
            //File.WriteAllText(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", "", Encoding.Default);
            //foreach (var pos in DelNoteFiles)
            //{
            //    File.AppendAllText(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", pos.ToString(), Encoding.Default);
            //}

            //Work
            //File.WriteAllText(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", "", Encoding.Default);
            //foreach (var pos in DelNoteFiles)
            //{
            //    File.AppendAllText(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", pos.ToString(), Encoding.Default);
            //}
            //Thread.Sleep(1000);
            Console.WriteLine("End");
            Console.ReadLine();
        }

        public static void ProcessFile(string file)
        {
            if (!(file.Trim().EndsWith(".txt")))
                return;

            DeliveryNoteFile delNote = new DeliveryNoteFile(file);
            /*
            //Add DeliveryNote
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DelNote dNote = new DelNote();
                    dNote = AddDelNote(delNote);
                    dNote = db.DelNotes.Add(dNote);
                    db.SaveChanges();

                    List<DelNoteItem> delNoteItems = AddDelNoteItems(dNote.ID, delNote);
                    db.DelNoteItems.AddRange(delNoteItems);
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
                catch(DbUpdateException ue)
                {
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }//*/
            DelNoteFiles.Add(delNote);
        }
        
        private static List<DelNoteItem> AddDelNoteItems(int ID, DeliveryNoteFile delNote)
        {
            try
            {                
                List<DelNoteItem> items = new List<DelNoteItem>();
                if (delNote.Positions != null && delNote.Positions.Count > 0)
                {
                    try
                    {                        
                        foreach (var position in delNote.Positions)
                        {
                            string expiryDate = "";
                            if (position.ExpiryDate != null)
                            {
                                expiryDate = position.ExpiryDate.Value.ToString("yyyyMMdd");
                            }
                            items.Add(new DelNoteItem
                            {
                                DelNoteID = ID,
                                ArticlePZN = position.ArticleNo,
                                ArticleLongName = position.ArticleLongName,
                                DelQty = position.DeliveryQty,
                                BonusQty = position.BonusQty,
                                PharmacyPurchasePrice = position.PharmacyPurchasePrice,
                                DiscountPercentage = position.DiscountPercentage,
                                InvoicedPrice = position.InvoicedPrice,
                                InvoicedPriceExclVAT = position.InvoicedPriceExclVAT,
                                InvoicedPriceInclVAT = position.InvoicedPriceInclVAT,
                                ParcelNo = position.Batch,
                                Certification = position.ArticleCertification,
                                ExpiryDate = expiryDate,
                                PharmacySellPrice = position.PharmacySellPrice,
                                BasePrice = position.WholesalePurchasePrice,
                                InvoicePriceNoDisc = position.InvoicedPriceInclVATNoDiscount,
                                RetailerMaxPrice = position.MaxPharmacySalesPrice,
                                GroupID = GetOverrateGroupID(position.ArticleNo.Value)
                            });
                        }
                    }
                    catch (DbEntityValidationException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }//*/

        private static DelNote AddDelNote(DeliveryNoteFile delNote)
        {
            string creditNoteDescr = "";
            if (delNote.DocType.isCreditNote && (delNote.Positions != null && delNote.Positions.Count > 0))
            {
                creditNoteDescr = delNote.Positions.FirstOrDefault().ArticleName;
            }
            DelNote dNote = new DelNote
            {
                FileName = delNote.FileName,
                ProcessTime = DateTime.Now,
                CustomerIDF = delNote.Header.PharmacyID.ToString(),
                DocNo = delNote.Header.DeliveryNoteNumber.ToString(),
                DocDate = delNote.Header.DeliveryNoteDate,
                PaymentSum = delNote.Footer.InvoiceTotal,
                TotalDiscounts = delNote.Footer.TotalDiscounts,
                CreditNoteType = delNote.Header.CreditNoteType,
                CreditNoteDescr = creditNoteDescr,
                ShipmentDate = delNote.Tour.TourDate,
                RouteID = delNote.Tour.TourID,
                VatPercent = delNote.VATTable.TotalPercent,
                PaymentTimeID = delNote.PaymentTimeID,
                PaymentConsignDate = delNote.Footer.DueDate,
                isNZOK = delNote.Header.isNZOKOrder,
                isRebateDiscount = delNote.Header.RebateInKindOrder,
                KSCOrderNo = delNote.Header.CSCOrderNumber.ToString()
            };
                                    
            return dNote;
        }
        private static string TranslateCreditType(string CreditNoteType)
        {
            switch (CreditNoteType)
            {
                case "БР":
                case "NO":
                    return "Без Рекламация";
                case "НК":
                case "RM":
                    return "Нормално Кредитно";
                case "ДК":
                case "MW":
                    return "ДДС Кредитно";
                case "ПК":
                case "HE":
                    return "Производител Кредитно";
                case "СК":
                case "FI":
                    return "Склад Кредитно";
                case "РК":
                case "RG":
                    return "Рабат Кредитно";
                case "УК":
                case "RB":
                    return "Утежняване кредитно";
                case "ПС":
                case "HW":
                    return "Стойностно Кредитно";
                case "НР":
                case "NR":
                    return "Натурален Рабат";
                case "ФР":
                case "FR":
                    return "Финансов Рабат";
                case "MP":
                    return "Ръчна Промоция";
                default:
                    return CreditNoteType;                    
            }
        }

        private static byte? GetOverrateGroupID(int articlePZN)
        {
            string sql = $"select OverrateGroupID from LibraCentral.dbo.Article with (nolock) where id = {articlePZN/10}";
            try
            {
                int temp = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                if (temp <= 255)
                    return (byte)temp;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
