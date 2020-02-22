using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public class ExtendedFileStream : IDisposable
    {
        ~ExtendedFileStream()
        {
            System.GC.SuppressFinalize(this);

            if (_originalFileStream != null)
            {
                _originalFileStream.Dispose();
                _originalFileStream = null;
            }
        }

        private FileStream _originalFileStream;

        public long Length() { return _originalFileStream.Length; }

        public ExtendedFileStream(string path, FileMode mode, FileAccess access)
        {
            _originalFileStream = new FileStream(path, mode, access);
        }

        public ExtendedFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        {
            _originalFileStream = new FileStream(path, mode, access, share);
        }

        public void Close()
        {
            _originalFileStream.Close();
        }

        public void Dispose()
        {
            _originalFileStream.Dispose();
        }

        public int Read(byte[] array, int offset, int count)
        {
            return _originalFileStream.Read(array, offset, count);
        }

        public void Write(byte[] array, int offset, int count)
        {
            _originalFileStream.Write(array, offset, count);
        }

        public void Flush()
        {
            _originalFileStream.Flush();
        }
    }
}
