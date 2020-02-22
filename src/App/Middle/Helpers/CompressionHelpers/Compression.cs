using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using SIO = System.IO;
using GridProteinFolding.Middle.Helpers.TypesHelpers;
using System.Security;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.CompressionHelpers
{
    //Referencia
    //http://wiki.sharpdevelop.net/SharpZipLib_FastZip.ashx

    public class Compression
    {
        ~Compression()
        {
            System.GC.SuppressFinalize(this);
        }

        public static void Compress(Guid guid, SIO.FileInfo zipFile, ExtendedDirectoryInfo baseDiretory, bool recursive, string fileFilter)
        {
            FastZip fastZip = new FastZip();
            fastZip.CreateEmptyDirectories = true;
            fastZip.CreateZip(zipFile.FullName, baseDiretory.FullName(), recursive, fileFilter);
            fastZip = null;
        }


        public static void Decompress(SIO.FileInfo zipFile, ExtendedDirectoryInfo targetDir, string fileFilter)
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(zipFile.FullName, targetDir.FullName(), fileFilter);
            fastZip = null;
        }
    }

}
