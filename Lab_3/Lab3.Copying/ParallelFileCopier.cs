using System.DirectoryServices.Protocols;
using System.IO;
using System.Threading;

namespace Lab3.Copying
{
    public static class ParallelFileCopier
    {
        private static int _notYetCompletedThreadsCount = 1;
        private static ManualResetEvent _doneEvent = new ManualResetEvent(false);


        public static int DeepCopying(string srcPath, string destPath, bool overRide)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            else
            {
                if (overRide)
                {
                    Directory.Delete(destPath, true);
                }
                else
                {
                    throw new DirectoryException(nameof(destPath));
                }
            }


            foreach (var directory in Directory.GetDirectories(srcPath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                Interlocked.Increment(ref _notYetCompletedThreadsCount);
                ThreadPool.QueueUserWorkItem(CreateDirectory, directory.Replace(srcPath, destPath));
            }

            var copiedFilesCount = 0;

            foreach (var file in Directory.GetFiles(srcPath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                Interlocked.Increment(ref _notYetCompletedThreadsCount);
                ThreadPool.QueueUserWorkItem(CopyFile, new FileCopyingParams { SrcPath = file, DestPath = file.Replace(srcPath, destPath)});
                copiedFilesCount++;
            }

            if (Interlocked.Decrement(ref _notYetCompletedThreadsCount) == 0)
            {
                _doneEvent.Set();
            }
            _doneEvent.WaitOne();

            return copiedFilesCount;
        }


        private static void CopyFile(object fileCopyingParams)
        {
            try
            {
                var copiedParams = (FileCopyingParams) fileCopyingParams;
                var srcPath = copiedParams.SrcPath;
                var destPath = copiedParams.DestPath;
                File.Copy(srcPath, destPath, true);
            }
            finally
            {
                if (Interlocked.Decrement(ref _notYetCompletedThreadsCount) == 0)
                {
                    _doneEvent.Set();
                }
            }
        }



        private static void CreateDirectory(object destPath)
        {
            try
            {
                Directory.CreateDirectory((string) destPath);
            }
            finally
            {
                if (Interlocked.Decrement(ref _notYetCompletedThreadsCount) == 0)
                {
                    _doneEvent.Set();
                }
            }
        }



        private class FileCopyingParams
        {
            public string SrcPath { get; set; }
            public string DestPath { get; set; }
        }
    }
}