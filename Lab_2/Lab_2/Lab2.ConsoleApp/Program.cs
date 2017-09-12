using System;
using System.Threading;
using Mutex = Lab2.Threading.Mutex;
using ThreadPool = Lab2.Threading.ThreadPool;

namespace Lab2.ConsoleApp
{
    internal class Program
    {
        private const int ThreadsInPoolCount = 3;
        private const int RepeatsCount = 100;


        private static readonly Mutex Mutex = new Mutex();


        public static void TestMethodWithoutMutex()
        {
            var name = Thread.CurrentThread.Name;
            Console.WriteLine($"{name} start method");
            for (var i = 0; i < RepeatsCount; i++)
            {
                Console.WriteLine($"{name} is now working");
            }
            Console.WriteLine($"{name} end method");
        }

        public static void TestMethodWithMutex()
        {
            var name = Thread.CurrentThread.Name;
            Mutex.Lock();
            Console.WriteLine($"{name} start method");
            for (var i = 0; i < RepeatsCount; i++)
            {
                Console.WriteLine($"{name} is now working");
            }
            Console.WriteLine($"{name} end method");
            Mutex.Unlock();
        }


        private static void Main()
        {
            var threadPool = new ThreadPool(ThreadsInPoolCount, true);

            //threadPool.EnqueueTask(TestMethodWithoutMutex);
            //threadPool.EnqueueTask(TestMethodWithoutMutex);
            //threadPool.EnqueueTask(TestMethodWithoutMutex);

            threadPool.EnqueueTask(TestMethodWithMutex);
            threadPool.EnqueueTask(TestMethodWithMutex);
            threadPool.EnqueueTask(TestMethodWithMutex);

            Console.ReadKey();
            threadPool.AbortAllThreads();
        }
    }
}