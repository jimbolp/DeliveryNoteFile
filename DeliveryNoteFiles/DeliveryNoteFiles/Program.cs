using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Settings = DelNoteItems.Properties.Settings;
using System.Threading;

namespace DeliveryNoteFiles
{
    class Program
    {
        private static List<DeliveryNoteFile> DelNoteFiles = new List<DeliveryNoteFile>();
        static void Main(string[] args)
        {
            Settings.Default.Save();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Home
            //string[] files = Directory.GetFiles(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

            //Work - Server 22
            string[] files = Directory.GetFiles(@"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171025");

            //Work - Special files for tests
            //string[] files = Directory.GetFiles(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

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
                        if (i >= 500)
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
                        if (i >= 500)
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
            
            DelNoteFiles.Add(new DeliveryNoteFile(file));
        }
    }
}
