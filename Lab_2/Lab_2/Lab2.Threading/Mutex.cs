using System.Threading;

namespace Lab2.Threading
{
    public class Mutex
    {
        private int _semaphore;


        public void Lock()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref _semaphore, 1, 0) == 0)
                {
                    return;
                }
            }
        }

        public void Unlock()
        {
            Interlocked.CompareExchange(ref _semaphore, 0, 1);
        }
    }
}