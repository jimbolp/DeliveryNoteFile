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
        private static int _threadCount = 0;
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

            //Work - For adding in the test database
            string dir = @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\Files To Add In Test";

            //File.WriteAllText(Settings.Default.ChangedPosFilePath, "");
            try
            {
                if (args != null && args.Length == 1)
                {
                    if(IsValidFilePath(args[0]))
                        OpenDirectory(args[0]);
                }
                else
                {
                    if (IsValidFilePath(dir))
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

            Console.WriteLine("End");
            Console.ReadLine();
        }

        private static void OpenDirectory(string dirPath)
        {
            string tempDir = Path.GetDirectoryName(dirPath);
            string[] files = null;
            //If the given path has extention, it's a file and we process only that file
            if (Path.HasExtension(dirPath))
            {
                files = new string[1] { dirPath };
            }
            else
            {
                files = Directory.GetFiles(tempDir);
            }

            int i = 0;
            
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
                    while(_threadCount >= Settings.Default.PermittedNumberOfThreads)
                    {
                        Thread.Sleep(1);
                    }
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
            try
            {
                _threadCount++;
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
                        DeliveryNoteFile.WriteExceptionToLog(eex);
#if DEBUG
                        Console.WriteLine(eex.Message);
#endif
                    }
                    catch (Exception e)
                    {
                        DeliveryNoteFile.WriteExceptionToLog(e);
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
                    else
                    {
                        Console.WriteLine(file + "->" + Environment.NewLine + delNote.Header.DeliveryNoteNumber);
                    }
                }//*/
                DelNoteFiles.Add(delNote);
                _threadCount--;
            }
            catch (Exception e)
            {
                _threadCount--;
                DeliveryNoteFile.WriteExceptionToLog(e);
                throw;
            }
        }

        private static bool EntryExists(DeliveryNoteFile delNote, out int? ID)
        {
            DelNote[] delNotes = db.DelNotes.Where(d => d.DocNo.Trim() == delNote.Header.DeliveryNoteNumber.ToString()).ToArray();
            if (delNotes == null || delNotes.Length == 0)
            {
                ID = null;
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

        private static List<DelNoteItem> AddDelNoteItems(int DelNoteID, DeliveryNoteFile delNote)
        {
            try
            {                
                List<DelNoteItem> items = new List<DelNoteItem>();
                if (delNote.Positions != null && delNote.Positions.Count > 0)
                {
                    try
                    {                        
                        foreach (Position position in delNote.Positions)
                        {                            
                            items.Add(CreateDelNoteItem(DelNoteID, position));
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
            if (delNote.Header.CreditNoteType == "ФР" || delNote.Header.CreditNoteType == "FR")
            {
                creditNoteDescr += delNote.Positions.FirstOrDefault().ArticleName;
                if (!string.IsNullOrEmpty(delNote.Positions.FirstOrDefault().ArticleRemark))
                    creditNoteDescr += " / " + delNote.Positions.FirstOrDefault().ArticleRemark;
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
        /// <summary>
        /// The field (CreditNoteType) in the table is limited to 10 characters. This method is not used for now!
        /// </summary>
        /// <param name="CreditNoteType"></param>
        /// <returns></returns>
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

        public static byte? GetOverrateGroupID(int articlePZN)
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

        private static bool IsValidFilePath(string filePath)
        {
            bool isValid = false;
            try
            {
                string temp = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(temp))
                {
                    isValid = false;
                }
                else if(Path.HasExtension(filePath))
                {
                    if (Path.GetExtension(filePath) == ".txt")
                    {
                        isValid = true;
                    }
                }
                else
                {
                    isValid = true;
                }
            }
            catch(Exception e)
            {
                DeliveryNoteFile.WriteExceptionToLog(e);
                isValid = false;
            }

            return isValid;
        }
    }
}
