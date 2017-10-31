using System;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DelNoteItems;
using Settings = DelNoteItems.Properties.Settings;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

namespace DeliveryNoteFiles
{
    partial class Program
    {
        private static List<DeliveryNoteFile> DelNoteFiles = new List<DeliveryNoteFile>();
        private static DeliveryNoteEntities db = new DeliveryNoteEntities();
        static void Main(string[] args)
        {            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Home
            //string dir = @"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests";

            //Work - Server 22
            //string dir = @"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171026";

            //Work - Special files for tests
            //string dir = @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests";

            //Work - One file only
            string dir = @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\One File";

            //File.WriteAllText(Settings.Default.ChangedPosFilePath, "");
            try
            {
                if (args != null && args.Length == 1)
                {
                    OpenDirectory(args[0]);
                }
                else
                {
                    OpenDirectory(dir);
                }
            }
            catch(ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());
            /*foreach(DeliveryNoteFile file in DelNoteFiles)
            {
                Console.WriteLine(file);
            }//*/
#if DEBUG
            //Files selected for debigging purposes...
            //DeliveryNoteFile[] test2 = DelNoteFiles.Where(d => d.Header.OrderType == "FC").ToArray();
            //DeliveryNoteFile[] test3 = DelNoteFiles.Where(d => d.Header.isNZOKOrder ?? false).ToArray();            
            //DeliveryNoteFile[] test4 = DelNoteFiles.Where(d => d.Positions != null && d.Positions.Any(pos => pos.isNZOKArticle ?? false)).ToArray();            
            //DeliveryNoteFile[] test6 = DelNoteFiles.Where(d => d.Positions != null).Where(d => d.Positions.Any(p => p.MaxPharmacySalesPrice != null)).ToArray();
            //DeliveryNoteFile[] test7 = DelNoteFiles.Where(d => d.Mail != null).Where(d => (d.Mail.ValueOfFieldInSK17 != null && d.Mail.ValueOfFieldInSK17 != "0" && d.Mail.ValueOfFieldInSK17 != "5")).ToArray();
            //DeliveryNoteFile[] test8 = DelNoteFiles.Where(d => d.Tour.TourDate == d.Footer.DueDate).ToArray();
            //DeliveryNoteFile[] test9 = DelNoteFiles.Where(d => !string.IsNullOrEmpty(d.Header.NarcoticsFormID)).ToArray();
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
            Console.WriteLine("End");
            Console.ReadLine();
        }

        private static void OpenDirectory(string dirPath)
        {
            if(!Directory.Exists(dirPath))
            {
                throw new ArgumentException($"Directory {dirPath}, does not exist!");
            }
            if(Path.HasExtension(dirPath))
            {
                if (Path.GetExtension(dirPath) != ".txt")
                    throw new ArgumentException("Invalid file type! Only Text files (txt) are allowed!");
            }

            int i = 0;
            string[] files = Directory.GetFiles(dirPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) != ".txt")
                {
                    continue;
                }
                i++;
                if (i >= 600)
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
                    Thread t = new Thread(() => ProcessFile(file));
                    t.Start();
                    t.Join();//*/
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public static void ProcessFile(string file)
        {
            DeliveryNoteFile delNote = new DeliveryNoteFile(file);
            int? existingEntryID;
            if (EntryExists(delNote, out existingEntryID))
            {
                Console.WriteLine("Updating entry...");
                UpdateExistingDelNote(existingEntryID.Value, delNote);
            }
            else
            {
                bool InsertCompleted = false;
                try
                {
                    Console.WriteLine("Inserting new entry...");
                    //Insert DeliveryNote in Database
                    InsertCompleted = InsertNewDeliveryNote(delNote);
                }
                catch (EntityException eex)
                {
                    Console.WriteLine(eex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                if (InsertCompleted)
                {
                    if (MoveFile(file))
                    {
                        Console.WriteLine($"File {Path.GetFileName(file)} is moved to {Settings.Default.SaveFilesPath}, successfully!");
                        string fileToSend = Settings.Default.SaveFilesPath + "\\" + file;
                        if (SendFile(fileToSend))
                        {
                            Console.WriteLine($"File {Path.GetFileName(fileToSend)} sent successfully!");
                        }

                    }
                }
            }
            DelNoteFiles.Add(delNote);
        }

        private static bool EntryExists(DeliveryNoteFile delNote, out int? ID)
        {
            DelNote[] delNotes = db.DelNotes.Where(d => d.DocNo == delNote.Header.DeliveryNoteNumber.ToString()).ToArray();
            if (delNotes == null || delNotes.Length == 0)
            {
                ID = 0;
                return false;
            }
            else
            {
                ID = delNotes.Where(d => d.DocDate == delNote.Header.DeliveryNoteDate).Select(d => d.ID).FirstOrDefault();
                if (ID == null || ID == 0)
                    return false;
                return true;
            }
        }

        private static bool SendFile(string fileToSend) { return false; }
        private static bool MoveFile(string file) { return false; }

        private static DelNoteItem CreateDelNoteItem(int DelNoteID, Position pos)
        {
            string expiryDate = "";
            if (pos.ExpiryDate != null)
            {
                expiryDate = pos.ExpiryDate.Value.ToString("yyyyMMdd");
            }
            try
            {
                DelNoteItem item = new DelNoteItem
                {
                    DelNoteID = DelNoteID,
                    ArticlePZN = pos.ArticleNo,
                    ArticleLongName = pos.ArticleLongName,
                    DelQty = pos.DeliveryQty,
                    BonusQty = pos.BonusQty,
                    PharmacyPurchasePrice = pos.PharmacyPurchasePrice,
                    DiscountPercentage = pos.DiscountPercentage,
                    InvoicedPrice = pos.InvoicedPrice,
                    InvoicedPriceExclVAT = pos.InvoicedPriceExclVAT,
                    InvoicedPriceInclVAT = pos.InvoicedPriceInclVAT,
                    ParcelNo = pos.Batch,
                    Certification = pos.ArticleCertification,
                    ExpiryDate = expiryDate,
                    PharmacySellPrice = pos.PharmacySellPrice,
                    BasePrice = pos.WholesalePurchasePrice,
                    InvoicePriceNoDisc = pos.InvoicedPriceInclVATNoDiscount,
                    RetailerMaxPrice = pos.MaxPharmacySalesPrice,
                    GroupID = GetOverrateGroupID(pos.ArticleNo.Value)
                };
                return item;
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
                            items.Add(CreateDelNoteItem(ID, position));
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
                if (temp <= 255 && temp != 0)
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
