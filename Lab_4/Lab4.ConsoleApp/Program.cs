using System.IO;
using Lab4.OSHandling;

namespace Lab4.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var osHandle = new FileStreamOsHandle(() => File.Create("path2.txt"));
            //osHandle.Dispose();
            var file = File.Create("path2.txt");
            var handle = file.Handle;
            var safeHandle = new SafeFileHandle(handle, false);
            safeHandle.Dispose();
        }
    }
}