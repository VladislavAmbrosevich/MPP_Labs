using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3.Copying
{
    public static class SyncFileCopier
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
                    return -1;
                }
            }

            foreach (var directory in Directory.GetDirectories(srcPath, "*.*", SearchOption.AllDirectories))
            {
                CreateDirectory(directory.Replace(srcPath, destPath));
            }

            var copiedFilesCount = 0;

            foreach (var file in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                CopyFile(file, file.Replace(srcPath, destPath));
                copiedFilesCount++;
            }

            return copiedFilesCount;
        }

        private static void CopyFile(string srcPath, string destPath)
        {
            File.Copy(srcPath, destPath, true);
        }



        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}