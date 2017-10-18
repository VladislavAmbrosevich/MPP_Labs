using System;
using System.IO;
using System.Reflection;
using Lab5_1.Serialization;

namespace Lab5_1.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You must pass the assembly path in the first parameter.");
                return;
            }

            var assemblyPath = args[0];
            if (Path.GetExtension(assemblyPath) != FileExtensions.Dll && Path.GetExtension(assemblyPath) != FileExtensions.Exe)
            {
                Console.WriteLine("You must pass a path to dll or exe file.");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                AssemblyXmlSerializer.SerializeAssemblyToFile(assembly, assemblyPath + FileExtensions.Xml);
                Console.WriteLine($"Saved to: {assemblyPath + FileExtensions.Xml}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}