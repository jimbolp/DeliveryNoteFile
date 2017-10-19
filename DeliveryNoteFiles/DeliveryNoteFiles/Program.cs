﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Home
            string[] files = Directory.GetFiles(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

            //Work - Server 22
            //string[] files = Directory.GetFiles(@"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171011");

            //Work - Special files for tests
            //string[] files = Directory.GetFiles(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");

            if (args != null && args.Length != 0)
            {
                foreach (var s in args)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        Console.WriteLine(s);
                        try
                        {
                            ProcessFile(s);
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
                foreach (var s in files)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        //ProcessFile(s);
                        i++;
                        if (i >= 500)
                            break;
                        if (i % 50 == 0)
                        {
                            Console.WriteLine(i);
                            Thread.Sleep(2);
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
            //Files containing positions with InvoicedQty == 0;
            DeliveryNoteFile[] test = DelNoteFiles.Where(d => d.Positions != null).Where(d => d.Positions.Where(p => p.InvoicedQty == 0).Any()).ToArray();
            DeliveryNoteFile[] test2 = DelNoteFiles.Where(d => d.Header.OrderType == "FC").ToArray();

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
