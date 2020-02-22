using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using System.Configuration;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using SIO = System.IO;
using GridProteinFolding.Middle.Helpers.TypesHelpers;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Services.DocumentManagementService
{
    public class DocumentLibraryService : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        SIO.DirectoryInfo uploadFolder = new SIO.DirectoryInfo(ConfigurationManager.AppSettings["uploadFolder"].ToString());
        SIO.DirectoryInfo dirBaseServer = new SIO.DirectoryInfo(ConfigurationManager.AppSettings["dirBaseServer"].ToString());

        public bool DocumentEcho(string fileName)
        {
            string fileWithExt = Path.GetFileNameWithoutExtension(fileName);
            GICO.WriteLine(String.Format("{0}> {1} {2}:{3}", fileWithExt, DateTime.Now, "Download", fileName));
            return true;
        }


        public void UploadDocument(string fileName, byte[] data, bool append)
        {
            //GICO.Write("Receiving file {0}..", uploadFolder.FullName);
            //GICO.WriteLine(String.Format("{0} {1}:{2}", DateTime.Now, "Download", fileName));
            ExtendedFileStream fs = new ExtendedFileStream(uploadFolder.FullName + fileName, append ? SIO.FileMode.Append : SIO.FileMode.Create, SIO.FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            //GICO.WriteLine(" done!");
            System.Threading.Thread.Sleep(500);
        }

        public void ExtractDocument(string fileName)
        {
            //GICO.WriteLine(String.Format("{0} {1}:{2}", DateTime.Now, "ExtractDocument", fileName));
            SIO.FileInfo tempFile = new SIO.FileInfo(uploadFolder.FullName + fileName);
            ExtendedDirectoryInfo tempDir = new ExtendedDirectoryInfo(dirBaseServer.FullName + Path.GetFileNameWithoutExtension(tempFile.Name));
            Directory.CreateDirectory(tempDir.FullName());

            ExtractFile(tempFile, tempDir);
        }

        private void ExtractFile(SIO.FileInfo fileName, ExtendedDirectoryInfo targetDir)
        {

            Middle.Helpers.CompressionHelpers.Compression.Decompress(fileName, targetDir, string.Empty);
            //File.Delete(fileName.FullName);
        }


        //public byte[] DownloadDocument(string fileName)
        //{
        //    GICO.WriteLine("Download document {0}.", fileName);

        //    string filePath = fileName;

        //    // read the file and return the byte[
        //    using (ExtendedFileStream fs = new ExtendedFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        byte[] buffer = new byte[fs.Length];
        //        fs.Read(buffer, 0, (int)fs.Length);
        //        return buffer;
        //    }
        //}
    }
}
