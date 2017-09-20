using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab3.Threading
{
    public delegate void ParameterizedTaskDelegate(object param);


    public class ParameterizedThreadPool : IDisposable
    {
        public static int MaxThreadsCount = 16;


        private readonly Thread[] _threadPool;
        private readonly Queue<TaskWithParam> _tasksWithParamsQueue;
        private readonly object _syncRoot = new object();

        private int _notYetCompletedTasksCount;


        public ParameterizedThreadPool(int threadsCount)
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
            _tasksWithParamsQueue = new Queue<TaskWithParam>();

            for (var i = 0; i < threadsCount; i++)
            {
                _threadPool[i] = new Thread(QueueProcessing);
                _threadPool[i].Start();
            }
        }


        public void EnqueueTask(ParameterizedTaskDelegate task, object param)
        {
            lock (_syncRoot)
            {
                _tasksWithParamsQueue.Enqueue(new TaskWithParam { Task = task, Param = param });
                Interlocked.Increment(ref _notYetCompletedTasksCount);
            }
        }

        public void WaitAllDone()
        {
            while (_notYetCompletedTasksCount > 0) { }
            _notYetCompletedTasksCount = 0;
        }

        public void Dispose()
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
                TaskWithParam taskWithParam;
                lock (_syncRoot)
                {
                    if (_tasksWithParamsQueue.Count.Equals(0))
                    {
                        continue;
                    }
                    taskWithParam = _tasksWithParamsQueue.Dequeue();
                }
                var task = taskWithParam?.Task;
                var param = taskWithParam?.Param;
                task?.Invoke(param);
                Interlocked.Decrement(ref _notYetCompletedTasksCount);
            }
        }



        private class TaskWithParam
        {
            public ParameterizedTaskDelegate Task { get; set; }
            public object Param { get; set; }
        }
    }
}