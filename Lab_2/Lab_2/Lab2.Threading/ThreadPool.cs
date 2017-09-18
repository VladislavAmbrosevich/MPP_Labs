using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab2.Threading
{
    public delegate void TaskDelegate();


    public class ThreadPool
    {
        private const int MaxThreadsCount = 100;

        private readonly Thread[] _threadPool;
        private readonly Queue<TaskDelegate> _tasksQueue;
        private readonly object _syncRoot = new object();


        public ThreadPool(int threadsCount)
        {
            if (threadsCount <= 0)
            {
                throw new ArgumentException(nameof(threadsCount));
            }
            if (threadsCount > MaxThreadsCount)
            {
                threadsCount = MaxThreadsCount;
            }

            _threadPool = new Thread[threadsCount];
            _tasksQueue = new Queue<TaskDelegate>();

            for (var i = 0; i < threadsCount; i++)
            {
                _threadPool[i] = new Thread(QueueProcessing);
                _threadPool[i].Start();
            }
        }


        public void EnqueueTask(TaskDelegate task)
        {
            lock (_syncRoot)
            {
                _tasksQueue.Enqueue(task);
            }
        }

        public void AbortAllThreads()
        {
            foreach (var thread in _threadPool)
            {
                thread.Abort();
            }
        }


        private void QueueProcessing()
        {
            while (true)
            {
                TaskDelegate task;
                lock (_syncRoot)
                {
                    if (_tasksQueue.Count.Equals(0))
                    {
                        continue;
                    }
                    task = _tasksQueue.Dequeue();
                }
                task?.Invoke();
            }
        }
    }
}