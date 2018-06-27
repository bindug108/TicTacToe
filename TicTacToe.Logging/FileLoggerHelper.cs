using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TicTacToe.Logging
{
    internal class FileLoggerHelper
    {
        private string _fileName;

        public FileLoggerHelper(string fileName)
        {
            _fileName = fileName;
        }

        static ReaderWriterLock locker = new ReaderWriterLock();
        internal void InsertLog(LogEntry logEntry)
        {
            var directory = System.IO.Path.GetDirectoryName(_fileName);

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                System.IO.File.AppendAllText(_fileName, $"{logEntry.CreatedTime} {logEntry.EventId} {logEntry.LogLevel} {logEntry.Message}" + Environment.NewLine);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
