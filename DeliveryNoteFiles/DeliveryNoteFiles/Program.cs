using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryNoteFiles
{
    class Program
    {
        private static List<DeliveryNoteFile> DelNoteFiles = new List<DeliveryNoteFile>();
        static void Main(string[] args)
        {
            /*string[] files = new string[] 
            {
                @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests\invoice-2027569-1003334907.txt",
                @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests\invoice-5025269-4002243257.txt",
                @"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests\invoice-2051214-1003340089.txt"
            };
            //*/
            string[] files = Directory.GetFiles(@"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171007");
            
            if (args != null && args.Length != 0)
            {
                foreach (var s in args)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        Console.WriteLine(s);
                        ProcessFile(s);
                    }
                }
            }
            else
            {
                int i = 0;
                foreach (var s in files)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        Console.WriteLine(s);
                        ProcessFile(s);
                        i++;
                        if (i > 100)
                            break;
                        /*Thread t = new Thread(() => ProcessFile(s));
                        t.Start();//*/
                    }
                }
            }
            DeliveryNoteFile checkInvoice = DelNoteFiles.FirstOrDefault();
            DeliveryNoteFile checkInvoice1 = DelNoteFiles.LastOrDefault();
            File.AppendAllText(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests\test positions.txt", checkInvoice.ToString() + checkInvoice1.ToString(), System.Text.Encoding.Default);
            //Thread.Sleep(1000);
            Console.WriteLine("End");
            Console.ReadLine();
        }

        public static void ProcessFile(string file)
        {
            DeliveryNoteFile DelNote = new DeliveryNoteFile(file);
            DelNoteFiles.Add(DelNote);
        }
    }
}
