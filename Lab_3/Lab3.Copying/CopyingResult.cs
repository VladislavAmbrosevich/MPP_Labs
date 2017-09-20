using System;

namespace Lab3.Copying
{
    public class CopyingResult
    {
        private readonly DateTime _startTime;

        private DateTime _finishTime;
        private long _copiedFilesSize;


        internal CopyingResult()
        {
            _startTime = DateTime.Now;
            _finishTime = DateTime.Now;
        }

        internal void EndCopying(int copiedFilesCount, long copiedFileSize)
        {
            CopiedFilesCount = copiedFilesCount;
            _copiedFilesSize = copiedFileSize;
            _finishTime = DateTime.Now;;
        }

        public int CopiedFilesCount { get; set; }


        public string CopiedFilesSizeString
        {
            get
            {
                if (_copiedFilesSize > 1048576)
                {
                    return $"{Math.Round((double)_copiedFilesSize / 1048576, 3)} MB";
                }
                if (_copiedFilesSize > 1024)
                {
                    return $"{Math.Round((double)_copiedFilesSize / 1024, 3)} KB";
                }

                return $"{_copiedFilesSize} B";
            }
        }

        public TimeSpan CopyingElapsedTime => _finishTime - _startTime;
    }
}