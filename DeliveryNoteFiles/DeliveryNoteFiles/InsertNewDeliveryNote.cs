using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace DeliveryNoteFiles
{
    partial class Program
    {
        /// <summary>
        /// Create a new DeliveryNote from the object and then inserts the DeliveryNoteItems
        /// </summary>
        /// <param name="delNote"></param>
        /// <returns></returns>
        private static bool InsertNewDeliveryNote(DeliveryNoteFile delNote)
        {
            bool transactionCompleted = false;
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
                    transactionCompleted = true;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var err in e.EntityValidationErrors)
                    {
                        foreach (var err1 in err.ValidationErrors)
                        {
                            DeliveryNoteFile.WriteExceptionToLog(err1.ErrorMessage);
                        }
                    }

                    transaction.Rollback();
                }
                catch (DbUpdateException ue)
                {
                    DeliveryNoteFile.WriteExceptionToLog(ue);
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    DeliveryNoteFile.WriteExceptionToLog(e);
                    transaction.Rollback();
                }
                return transactionCompleted;
            }//*/
        }
    }
}
