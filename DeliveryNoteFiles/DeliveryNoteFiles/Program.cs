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
            //string[] files = Directory.GetFiles(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\files for tests");
            string[] files = Directory.GetFiles(@"\\bgsf2s022\c$\Phoenix\XML\delnote.old\171011");

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
                        if (i >= 50)
                            break;
                        if (i % 20 == 0)
                            Console.WriteLine(s);
                        //    break;
                        Thread t = new Thread(() => ProcessFile(s));
                        t.Start();
                        t.Join();//*/
                    }
                }
            }
                        
            //Home
            //File.WriteAllText(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", "", Encoding.Default);
            //foreach (var pos in DelNoteFiles)
            //{
            //    File.AppendAllText(@"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", pos.ToString(), Encoding.Default);
            //}

            //Work
            File.WriteAllText(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", "", Encoding.Default);
            foreach (var pos in DelNoteFiles)
            {
                File.AppendAllText(@"D:\Documents\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\test positions.txt", pos.ToString(), Encoding.Default);
            }
            //Thread.Sleep(1000);
            Console.WriteLine("End");
            Console.ReadLine();
        }

        public static void ProcessFile(string file)
        {
            //DeliveryNoteFile DelNote = new DeliveryNoteFile(file);
            DelNoteFiles.Add(new DeliveryNoteFile(file));
        }
    }
}
