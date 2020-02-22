using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;

[assembly: CLSCompliant(true)]
//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.TypesHelpers
{
    public class ExtendedDirectoryInfo
    {
        private DirectoryInfo _originalDirectoryInfo;

        public ExtendedDirectoryInfo(string path)
        {
            _originalDirectoryInfo = new DirectoryInfo(path);
        }

        public string FullName()
        {
            return _originalDirectoryInfo.FullName;
        }
    }
}
