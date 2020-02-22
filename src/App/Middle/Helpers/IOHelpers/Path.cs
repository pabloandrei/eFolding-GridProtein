using System
;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public class Path
    {

        public static string GetFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public static string GetDirectoryName(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        public static string GetExtension(string path)
        {
            return System.IO.Path.GetExtension(path);
        }

        public static string GetFileName(string path) {
            return System.IO.Path.GetFileName(path);
        }
    }
}
