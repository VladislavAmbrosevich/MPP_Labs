using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Threading;

namespace Lab3.Copying
{
    public static class ParallelFileCopier
    {
        public static int DeepCopying(string srcPath, string destPath, bool overRide)
        {
            if (!Directory.Exists(destPath))
            {
                CreateDirectory(destPath);
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

            //var doneEvents = new List<ManualResetEvent>();

            foreach (var directory in Directory.GetDirectories(srcPath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                ThreadPool.QueueUserWorkItem(CreateDirectory, directory.Replace(srcPath, destPath));
                //CreateDirectory(directory.Replace(srcPath, destPath));
            }

            var copiedFilesCount = 0;

            foreach (var file in Directory.GetFiles(srcPath, "*.*", System.IO.SearchOption.AllDirectories))
            {

                ThreadPool.QueueUserWorkItem(CopyFile, new FileCopyingParameters{ SrcPath = file, DestPath = file.Replace(srcPath, destPath) });
                copiedFilesCount++;
            }
            //WaitHandle.WaitAll();

            return copiedFilesCount;
        }


        private static void CopyFile(Object pathParams)
        {
            var copiedParams = (FileCopyingParameters) pathParams;
            var srcPath = copiedParams.SrcPath;
            var destPath = copiedParams.DestPath;
            File.Copy(srcPath, destPath, true);
        }



        private static void CreateDirectory(object path)
        {
            Directory.CreateDirectory((string)path);
        }



        private class FileCopyingParameters
        {
            public string SrcPath { get; set; }
            public string DestPath { get; set; }
        }
    }
}