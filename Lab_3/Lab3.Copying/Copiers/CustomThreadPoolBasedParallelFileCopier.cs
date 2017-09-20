using System.IO;
using Lab3.Threading;

namespace Lab3.Copying.Copiers
{
    public class CustomThreadPoolBasedParallelFileCopier : FileCopier
    {
        public override int CopyRecursive(string srcPath, string destPath)
        {
            var copiedFilesCount = 0;

            using (var threadPool = new ParameterizedThreadPool(ParameterizedThreadPool.MaxThreadsCount))
            {

                foreach (var directory in Directory.GetDirectories(srcPath, FileCopier.AnyFile, SearchOption.AllDirectories))
                {
                    threadPool.EnqueueTask(CreateDirectory, directory.Replace(srcPath, destPath));
                }
                threadPool.WaitAllDone();

                foreach (var file in Directory.GetFiles(srcPath, FileCopier.AnyFile, System.IO.SearchOption.AllDirectories))
                {
                    threadPool.EnqueueTask(CopyFile, new FileCopyingParams {SrcPath = file, DestPath = file.Replace(srcPath, destPath)});
                    copiedFilesCount++;
                }
                threadPool.WaitAllDone();
            }

            return copiedFilesCount;

        }


        private static void CopyFile(object fileCopyingParams)
        {
            var copiedParams = (FileCopyingParams) fileCopyingParams;
            var srcPath = copiedParams.SrcPath;
            var destPath = copiedParams.DestPath;
            File.Copy(srcPath, destPath, true);
        }

        private static void CreateDirectory(object destPath)
        {
            Directory.CreateDirectory((string) destPath);
        }
    }
}