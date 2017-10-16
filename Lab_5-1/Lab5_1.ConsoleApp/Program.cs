using System;
using System.Linq;
using System.Reflection;
using Lab5_1.Common;
using Lab5_1.Serialization;

namespace Lab5_1.ConsoleApp
{
    internal class Program
    {
        //        private const string AssemblyName = "Lab5_1.TestAssembly.dll";
        private const string AssemblyName = @"D:\БГУИР\СПП\5сем\MPP_Labs\Lab_5-1\Lab5_1.TestAssembly\bin\Debug\Lab5_1.TestAssembly.dll";

        private static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom(AssemblyName);
            var types = assembly.GetTypes().Where(type => type.IsDefined(typeof(ExportXmlAttribute), false)).ToList();
            foreach (var member in types)
            {
                Console.WriteLine(member.Name);
            }

            var typeInfo = TypeParser.GetTypeInfo(types[1]);
            Console.ReadLine();
        }
    }
}