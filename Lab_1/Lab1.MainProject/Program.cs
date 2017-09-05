using System;
using Lab1.Collections.Generic;

namespace Lab1.MainProject
{
    internal class Program
    {
        private static void Main()
        {
            var list = new DynamicList<int>{1, 2, 3, 4, 5};
            Console.WriteLine($"Initial length: { list.Count }");
            Console.WriteLine("Initial items:");
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            list.Add(6);
            list.Add(8);
            Console.WriteLine("Items after adding:");
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            list.RemoveAt(0);
            list.RemoveAt(0);
            Console.WriteLine("Items after removing by index:");
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            list.Remove(6);
            list.Remove(8);
            Console.WriteLine("Items after removing by value:");
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            list.Clear();
            Console.WriteLine($"Length after clearing: { list.Count }");
            Console.ReadKey();
        }
    }
}