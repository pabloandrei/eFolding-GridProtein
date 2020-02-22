using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using SIO = System.IO;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public class File
    {
        public static long Length(string path, bool inKb = true)
        {
            if (inKb)
                return (new SIO.FileInfo(path).Length / 1024);
            else
                return (new SIO.FileInfo(path).Length / 1048576);

        }
        public static bool Exists(string path)
        {
            return SIO.File.Exists(path);
        }

        public static void Delete(string path)
        {
            SIO.File.Delete(path);
        }

        public static void Copy(string source, string destine, bool overwrite)
        {
            SIO.File.Copy(source, destine, overwrite);
        }

        public static void Move(string souceFileName, string destFileName)
        {
            SIO.File.Move(souceFileName, destFileName);
        }

        public SIO.FileInfo FileInfos(string fileName)
        {
            return new SIO.FileInfo(fileName);
        }

        public static void DeleteAllFilesOfDirectory(SIO.DirectoryInfo directory)
        {
            foreach (SIO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (SIO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static string ReadLastLineOfFile(string path)
        {
            return System.IO.File.ReadLines(path).Last();
        }

    }
}
