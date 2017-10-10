using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryNoteFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = new string[] { @"E:\Documents\C# Projects\GitHub\DeliveryNoteFile\DeliveryNoteFiles\DeliveryNoteFiles\bin\Debug\3002289842.txt" };
            List<DeliveryNoteFile> DelNoteFiles = new List<DeliveryNoteFile>();
            if (args != null && args.Length != 0)
            {
                foreach (var s in args)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        Console.WriteLine(s);
                        DelNoteFiles.Add(new DeliveryNoteFile(s));
                    }
                }
            }
            DelNoteFiles.Add(new DeliveryNoteFile(files[0]));
            Console.ReadLine();
        }
    }
}
