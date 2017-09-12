using System.Collections.Generic;
using System.Threading;

namespace Lab2.Threading
{
    public delegate void TaskDelegate();


    public class ThreadPool
    {
        private readonly Thread[] _threadPool;
        private readonly Queue<TaskDelegate> _tasksQueue;
        private readonly object _syncRoot = new object();


        public ThreadPool(int threadsCount)
            : this(threadsCount, false)
        {

        }

        public ThreadPool(int threadsCount, bool withNames)
        {
            _threadPool = new Thread[threadsCount];
            _tasksQueue = new Queue<TaskDelegate>();

            for (var i = 0; i < threadsCount; i++)
            {
                _threadPool[i] = new Thread(QueueProcessing);
                if (withNames)
                {
                    _threadPool[i].Name = $"Thread_{i}";
                }
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
                task.Invoke();
            }
        }
    }
}