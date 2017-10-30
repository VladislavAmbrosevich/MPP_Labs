using System;
using Lab6.Serialization;

namespace Lab6.TestConsoleApp
{
    internal class Program
    {
        private const string FilePath = @"D:\БГУИР\СПП\5сем\MPP_Labs\Lab_6\Lab6.TestConsoleApp\bin\Debug\Lab5_1.TestAssembly.dll.xml";


        private static void Main(string[] args)
        {
            var info = AssemblyXmlSerializer.ParseFromXmlFile(FilePath);


            Console.ReadLine();
        }
    }
}