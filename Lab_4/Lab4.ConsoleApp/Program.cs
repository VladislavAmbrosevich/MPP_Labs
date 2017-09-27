using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab4.OSHandling;

namespace Lab4.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var osHandle = new FileStreamOsHandle(() => File.Create("path2.txt"));
        }
    }
}