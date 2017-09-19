using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lab3.Copying;

namespace Lab3.ConsoleApp
{
    internal class Program
    {
        private const string SrcPath = "Src";
        private const string DestPath = "Dest";


        private static void Main(string[] args)
        {
            //ThreadPool.QueueUserWorkItem()
            var start = DateTime.Now;
            Console.WriteLine(start.ToLongTimeString());
            var filesCount = ParallelFileCopier.DeepCopying(SrcPath, DestPath, true);
            Console.WriteLine(filesCount);
            var finish = DateTime.Now;
            Console.WriteLine($"{finish.ToLongTimeString()}, {(finish - start).TotalMilliseconds}");
            Console.ReadKey();
        }
    }
}