using System.DirectoryServices.Protocols;
using System.IO;
using System.Linq;

namespace Lab3.Copying.Copiers
{
    public abstract class FileCopier
    {
        protected static readonly string AnyFile = "*.*";


        public CopyingResult DeepCopying(string srcPath, string destPath, bool overwrite)
        {
            CheckSourceDirectoryPath(srcPath);
            ProcessDestinationDirectoryPath(destPath, overwrite);

            var copyingResult = new CopyingResult();
            copyingResult.EndCopying(CopyRecursive(srcPath, destPath), GetDirectorySize(destPath));

            return copyingResult;
        }

        public abstract int CopyRecursive(string srcPath, string destPath);


        private static long GetDirectorySize(string path)
        {
            var directory = new DirectoryInfo(path);
            var size = directory.EnumerateFiles(AnyFile, System.IO.SearchOption.AllDirectories).Sum(file => file.Length);

            return size;
        }


        private static void CheckSourceDirectoryPath(string srcPath)
        {
            if (!Directory.Exists(srcPath))
            {
                throw new DirectoryException($"{srcPath}: No such directory.");
            }
        }

        private static void ProcessDestinationDirectoryPath(string destPath, bool overwrite)
        {
            if (!Directory.Exists(destPath))
            {
                return;
            }
            if (overwrite)
            {
                Directory.Delete(destPath, true);
            }
            else
            {
                throw new DirectoryException($"{destPath}: Directory with this name already exists.");
            }
        }
    }
}