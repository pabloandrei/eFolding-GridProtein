//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GridProteinFolding.Middle.Helpers.NetworkHelpers;
//using System.Configuration;
//using GridProteinFolding.Middle.Helpers.CompressionHelpers;
//using GridProteinFolding.Middle.Helpers.IOHelpers;
//using SIO = System.IO;
//using GridProteinFolding.Middle.Helpers.TypesHelpers;

//namespace SandBoxDocument
//{
//    class Program
//    {

//        static void Main(string[] args)
//        {
//            Upload();
//        }

//        static void Upload()
//        {

//            string path = ConfigurationManager.AppSettings["dirBaseClient"].ToString() + "00000000-0000-0000-0000-000000000000";
//            string destPath = ConfigurationManager.AppSettings["uploadFolder"].ToString();

//            SIO.FileInfo sourceFile = new SIO.FileInfo(path + ".zip");
//            SIO.FileInfo destFileName = new SIO.FileInfo(destPath + sourceFile.Name);

//            //Compress FILE
//            Compression.Compress(sourceFile, new ExtendedDirectoryInfo(path), true, string.Empty);

//            if (File.Exists(destFileName.FullName))
//                File.Delete(destFileName.FullName);

//            File.Move(sourceFile.FullName, destFileName.FullName);

//            byte[] buffer = null;

//            // read the file and return the byte[
//            using (ExtendedFileStream fs = new ExtendedFileStream(destFileName.FullName, SIO.FileMode.Open, SIO.FileAccess.Read, SIO.FileShare.Read))
//            {
//                buffer = new byte[fs.Length()];
//                fs.Read(buffer, 0, (int)fs.Length());

//                fs.Close();
//                fs.Dispose();
//            }

//            if (buffer != null)
//            {
//                DocumentReference.DocumentManagmentClient objDoc = new DocumentReference.DocumentManagmentClient();
//                objDoc.Open();

//                //objDoc.DocumentEcho(destFileName);
//                objDoc.UploadDocument(destFileName.Name, buffer);

//                objDoc.Close();
//                objDoc = null;

//                File.Delete(destFileName.FullName);
//            }
//        }

//        //static void Download()
//        //{

//        //    filepath = @"C:\GridProteinFolding\_temp\download\00000000-0000-0000-0000-000000000000.zip";
//        //    serverfilepath = @"C:\GridProteinFolding\_temp\upload\00000000-0000-0000-0000-000000000000.zip";

//        //    DocumentReference.DocumentManagmentClient objDoc = new DocumentReference.DocumentManagmentClient();
//        //    objDoc.Open();

//        //    int size = 0;
//        //    byte[] data = objDoc.DownloadDocument(serverfilepath);

//        //    objDoc.Close();
//        //    objDoc = null;

//        //    ExtendedFileStream fs = new ExtendedFileStream(filepath, FileMode.Create, FileAccess.Write);
//        //    fs.Write(data, 0, data.Length);
//        //    fs.Close();

//        //}
//    }
//}
