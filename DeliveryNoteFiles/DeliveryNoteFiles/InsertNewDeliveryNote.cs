﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace DeliveryNoteFiles
{
    partial class Program
    {
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
                return transactionCompleted;
            }//*/
        }
    }
}
