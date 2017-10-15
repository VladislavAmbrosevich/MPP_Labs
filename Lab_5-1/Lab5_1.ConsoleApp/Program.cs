using System;
using System.Linq;
using System.Reflection;
using Lab5_1.Common;

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

            //            var ctors = types[1].GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            var members = types[1].GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var member in members)
            {
                Console.WriteLine(member.Name + member.IsVirtual);
            }
            //            var obj = ctor.Invoke(new object[] { });

            var methods = types[1].GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
////            var result = method.Invoke(obj, new object[]{10});
////            Console.WriteLine((int)result);
//            foreach (var method in methods)
//            {
//                Console.WriteLine(method.Name);
//            }

            //            foreach (var type in types)
            //            {
            //                    Console.WriteLine($"{type.FullName} {type.IsPublic}");
            //                foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance |
            //                                                     BindingFlags.Public))
            //                {
            //                    if (field.IsPrivate)
            //                    {
            //                        Console.WriteLine($"private:    {field.Name} {field.FieldType}");
            //                    }
            //                    if (field.IsAssembly)
            //                    {
            //                        Console.WriteLine($"internal:    {field.Name} {field.FieldType}");
            //                    }
            //                    if (field.IsFamily)
            //                    {
            //                        Console.WriteLine($"protected:    {field.Name} {field.FieldType}");
            //                    }
            //                    if (field.IsFamilyOrAssembly)
            //                    {
            //                        Console.WriteLine($"protected internal:    {field.Name} {field.FieldType}");
            //                    }
            //                    if (field.IsPublic)
            //                    {
            //                        Console.WriteLine($"public:    {field.Name} {field.FieldType}");
            //                    }
            ////                    if (field.IsFamily)
            ////                    {
            ////                        Console.WriteLine($"public:    {field.Name} {field.FieldType}");
            ////                    }
            //                    //                    foreach (var method in type.GetMethods())
            //                    //                    {
            //                    //                        
            //                    //                    }
            //                }
            //            }
            Console.ReadLine();
        }
    }
}