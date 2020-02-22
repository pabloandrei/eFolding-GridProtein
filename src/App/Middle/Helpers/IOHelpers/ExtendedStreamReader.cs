using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GridProteinFolding.Middle.Helpers.CryptographyHelpers;
using System.Text.RegularExpressions;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public class ExtendedStreamReader : IDisposable
    {
        private Guid currentGui;
        private StreamReader _originalStreamReader;
        private bool doCryptor = true;

        ~ExtendedStreamReader()
        {
            System.GC.SuppressFinalize(this);

            if (_originalStreamReader != null)
            {
                _originalStreamReader.Dispose();
                _originalStreamReader = null;
            }
        }
        public ExtendedStreamReader() { }

        public ExtendedStreamReader(string path, Guid currentGui, bool doCryptor)
        {
            this.currentGui = currentGui;
            this.doCryptor = doCryptor;
            _originalStreamReader = new StreamReader(path);
        }

        public void Close()
        {
            _originalStreamReader.Close();
        }

        public void Dispose()
        {
            _originalStreamReader.Dispose();
        }

        public string ReadLine()
        {
            string data = _originalStreamReader.ReadLine();
            if (data != string.Empty && data != null)
                if (doCryptor)
                    return CryptorEngine.Decrypt(data, true, KeyGen.Generator(currentGui));
                else
                    return data;
            else
                return null;
        }

        public string ReadToEnd()
        {
            string data = _originalStreamReader.ReadToEnd();
            if (data != string.Empty && data != null)
                if (doCryptor)
                    return CryptorEngine.Decrypt(data, true, KeyGen.Generator(currentGui));
                else
                    return data;
            else
                return null;
        }

        public static int LinesOfFile(string fileName)
        {
            //string text = string.Empty;

            //using (StreamReader r = new StreamReader(fileName))
            //{
            //    text = r.ReadToEnd();
            //}

            //Regex RE = new Regex("\\n", RegexOptions.Multiline);
            //MatchCollection theMatches = RE.Matches(text);

            //return theMatches.Count;

            return System.IO.File.ReadLines(fileName).Count();
        }
    }
}
