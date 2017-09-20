using System.IO;
using System.Threading;

namespace Lab3.Copying.Copiers
{
    public class ParallelFileCopier : FileCopier
    {
        private static int _notYetCompletedThreadsCount = 1;
        private static readonly ManualResetEvent DoneEvent = new ManualResetEvent(false);


        public override int CopyRecursive(string srcPath, string destPath)
        {
            foreach (var directory in Directory.GetDirectories(srcPath, FileCopier.AnyFile, SearchOption.AllDirectories))
            {
                Interlocked.Increment(ref _notYetCompletedThreadsCount);
                ThreadPool.QueueUserWorkItem(CreateDirectory, directory.Replace(srcPath, destPath));
            }

            var copiedFilesCount = 0;

            foreach (var file in Directory.GetFiles(srcPath, FileCopier.AnyFile, SearchOption.AllDirectories))
            {
                Interlocked.Increment(ref _notYetCompletedThreadsCount);
                ThreadPool.QueueUserWorkItem(CopyFile, new FileCopyingParams { SrcPath = file, DestPath = file.Replace(srcPath, destPath)});
                copiedFilesCount++;
            }
            if (Interlocked.Decrement(ref _notYetCompletedThreadsCount) == 0)
            {
                DoneEvent.Set();
            }
            DoneEvent.WaitOne();

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
                    DoneEvent.Set();
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
                    DoneEvent.Set();
                }
            }
        }
    }
}