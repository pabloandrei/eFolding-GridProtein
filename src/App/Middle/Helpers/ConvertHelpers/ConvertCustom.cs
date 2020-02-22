using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using System.Globalization;

namespace GridProteinFolding.Middle.Helpers.ConvertHelpers
{
    //
    // Summary:
    //     Converts a base data type to another base data type.
    public static class ConvertCustom
    {
       
        public static int ToInt32(string value, IFormatProvider provider)
        {
            return System.Convert.ToInt32(value, provider);
        }

        public static int ToInt32(string value)
        {
            return System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        public static double ToDouble(string value, IFormatProvider provider)
        {
            return System.Convert.ToDouble(value, provider);
        }

        public static double ToDouble(string value)
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }



        public static string ToString(string value, IFormatProvider provider)
        {
            return System.Convert.ToString(value, provider);
        }

        public static string ToString(string value)
        {
            return System.Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}