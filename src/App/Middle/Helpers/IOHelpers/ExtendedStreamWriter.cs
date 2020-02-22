using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GridProteinFolding.Middle.Helpers.CryptographyHelpers;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public class ExtendedStreamWriter
    {
        private Guid currentGui;
        private StreamWriter _originalStreamWriter;
        private bool doCryptor = true;

        ~ExtendedStreamWriter()
        {

            System.GC.SuppressFinalize(this);

            if (_originalStreamWriter != null)
            {
                _originalStreamWriter.Dispose();
                _originalStreamWriter = null;
            }
        }

        
        public virtual bool AutoFlush
        {
            set
            {
                _originalStreamWriter.AutoFlush = value;
            }
        }

        public ExtendedStreamWriter(StreamWriter originalStreamWriter, bool doCryptor)
        {
            this.doCryptor = doCryptor;
            _originalStreamWriter = originalStreamWriter;
        }

        public ExtendedStreamWriter(string path, bool append, Guid currentGui, bool doCryptor)
        {
            this.doCryptor = doCryptor;
            try
            {
                this.currentGui = currentGui;
                _originalStreamWriter = new StreamWriter(path, append);
            }
            catch
            {
                throw;
            }
        }

        public ExtendedStreamWriter()
        {

        }

        public ExtendedStreamWriter(bool doCryptor)
        {
            this.doCryptor = doCryptor;
        }

        public void CreateText(string pathFile, Guid currentGui)
        {
            try
            {
                this.currentGui = currentGui;
                _originalStreamWriter = System.IO.File.CreateText(pathFile);
            }
            catch
            {
                throw;
            }
        }

        public StreamWriter AppendText(string path, Guid currentGui)
        {
            try
            {
                this.currentGui = currentGui;
                _originalStreamWriter = System.IO.File.AppendText(path);
                return _originalStreamWriter;
            }
            catch
            {
                throw;
            }
        }

        public void Write(string value)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.Write(CryptorEngine.Encrypt(value, true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.Write(value);
            }
            catch
            {
                throw;
            }
        }

        public void Write(byte[] value)
        {
            try
            {
                _originalStreamWriter.Write(value);
            }
            catch
            {
                throw;
            }
        }

        public void Write(string format, object arg0)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.Write(format, CryptorEngine.Encrypt(arg0.ToString(), true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.Write(format, arg0.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void WriteLine(string format, object arg0)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.WriteLine(format, CryptorEngine.Encrypt(arg0, true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.WriteLine(format, arg0.ToString());
            }
            catch
            {
                throw;
            }
        }

        public virtual void WriteLine(string format, Object arg0, Object arg1, Object arg2)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.WriteLine(format,
                        CryptorEngine.Encrypt(arg0, true, KeyGen.Generator(currentGui)),
                        CryptorEngine.Encrypt(arg1, true, KeyGen.Generator(currentGui)),
                        CryptorEngine.Encrypt(arg2, true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.WriteLine(format,
                    arg0,
                    arg1,
                    arg2);
            }
            catch
            {
                throw;
            }
        }


        public void WriteLine(string value)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.WriteLine(CryptorEngine.Encrypt(value, true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.WriteLine(value);
            }
            catch
            {
                throw;
            }
        }

        public void WriteLine(int value)
        {
            try
            {
                if (doCryptor)
                    _originalStreamWriter.WriteLine(CryptorEngine.Encrypt(value, true, KeyGen.Generator(currentGui)));
                else
                    _originalStreamWriter.WriteLine(value);
            }
            catch
            {
                throw;
            }
        }

        public void WriteLine()
        {
            _originalStreamWriter.WriteLine();
        }

        public void Flush()
        {
            _originalStreamWriter.Flush();
        }

        public void Close()
        {
            _originalStreamWriter.Close();
        }

        public Stream BaseStream()
        {
            return _originalStreamWriter.BaseStream;
        }
    }
}
