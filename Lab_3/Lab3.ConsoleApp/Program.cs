using System;
using Lab3.Copying.Copiers;

namespace Lab3.ConsoleApp
{
    internal class Program
    {
        private static readonly string ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        private static bool _overwrite;
        private static string _srcPath;
        private static string _destPath;
        private static FileCopier _fileCopier = new SyncFileCopier();

        private static int Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine($"{ProgramName}: Incorrect arguments. Please use <src> <dest> or -<flags> <src> <dest>");

                return -1;
            }
            switch (args.Length)
            {
                case 2:
                    _srcPath = args[0];
                    _destPath = args[1];
                    break;
                case 3:
                    ParseFlags(args[0]);
                    _srcPath = args[1];
                    _destPath = args[2];
                    break;
            }

            try
            {
                Console.WriteLine($"{ProgramName}: Copying with {_fileCopier.GetType().Name} started.");
                var result = _fileCopier.DeepCopying(_srcPath, _destPath, _overwrite);
                Console.WriteLine($"{ProgramName}: Copying finished.");
                Console.WriteLine($"    Copied files count: {result.CopiedFilesCount}");
                Console.WriteLine($"    Size of copied files: {result.CopiedFilesSizeString}");
                Console.WriteLine($"    Elapsed time: {result.CopyingElapsedTime.TotalMilliseconds} milliseconds");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ProgramName}: {e.Message}");
            }

            return 0;
        }

        private static void ParseFlags(string flags)
        {
            if (flags.Length > 3 || flags.Length < 2 || flags[0] != '-')
            {
                return;
            }
            switch (flags.Length)
            {
                case 2:
                {
                    var flag = flags[1];
                    switch (flag)
                    {
                        case 'o':
                            _overwrite = true;
                            break;
                        case 'p':
                            _fileCopier = new ParallelFileCopier();
                            break;
                        case 'c':
                            _fileCopier = new CustomThreadPoolBasedParallelFileCopier();
                            break;
                    }
                    break;
                }
                case 3:
                {
                    var owerwrite = flags[1];
                    if (owerwrite == 'o')
                    {
                        _overwrite = true;
                    }
                    var copierType = flags[2];
                    {
                        switch (copierType)
                        {
                            case 'p':
                                _fileCopier = new ParallelFileCopier();
                                break;
                            case 'c':
                                _fileCopier = new CustomThreadPoolBasedParallelFileCopier();
                                break;
                        }
                    }
                    break;
                }
            }
        }
    }
}