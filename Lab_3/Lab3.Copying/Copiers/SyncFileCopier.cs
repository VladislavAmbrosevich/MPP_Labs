using System.IO;
using SearchOption = System.IO.SearchOption;

namespace Lab3.Copying.Copiers
{
    public class SyncFileCopier : FileCopier
    {
        public override int CopyRecursive(string srcPath, string destPath)
        {
            foreach (var directory in Directory.GetDirectories(srcPath, FileCopier.AnyFile, SearchOption.AllDirectories))
            {
                CreateDirectory(directory.Replace(srcPath, destPath));
            }

            var copiedFilesCount = 0;

            foreach (var file in Directory.GetFiles(srcPath, FileCopier.AnyFile, SearchOption.AllDirectories))
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