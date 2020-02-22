using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.TypesHelpers
{
    public static class ExtendedString
    {
        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string representation
        //     of the corresponding objects in args.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     format or args is null.
        //
        //   T:System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        public static String Format(String format, params object[] args)
        {
            return System.String.Format(new CultureInfo("pt-BR"),format, args);
        }

        //
        // Summary:
        //     Replaces one or more format items in a specified string with the string representation
        //     of a specified object.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The object to format.
        //
        // Returns:
        //     A copy of format in which any format items are replaced by the string representation
        //     of arg0.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     format is null.
        //
        //   T:System.FormatException:
        //     The format item in format is invalid.-or- The index of a format item is not zero.
        public static String Format(String format, object arg0)
        {
            return System.String.Format(new CultureInfo("pt-BR"),format, arg0);
        }

        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array. A specified parameter supplies
        //     culture-specific formatting information.
        //
        // Parameters:
        //   provider:
        //     An object that supplies culture-specific formatting information.
        //
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string representation
        //     of the corresponding objects in args.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     format or args is null.
        //
        //   T:System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        [SecuritySafeCritical]
        public static String Format(IFormatProvider provider, String format, params object[] args)
        {
            return System.String.Format(provider, format, args);
        }

        //
        // Summary:
        //     Replaces the format items in a specified string with the string representation
        //     of two specified objects.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The first object to format.
        //
        //   arg1:
        //     The second object to format.
        //
        // Returns:
        //     A copy of format in which format items are replaced by the string representations
        //     of arg0 and arg1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     format is null.
        //
        //   T:System.FormatException:
        //     format is invalid.-or- The index of a format item is not zero or one.
        public static String Format(String format, object arg0, object arg1)
        {
            return System.String.Format(format, arg0, arg1);
        }

        //
        // Summary:
        //     Replaces the format items in a specified string with the string representation
        //     of three specified objects.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The first object to format.
        //
        //   arg1:
        //     The second object to format.
        //
        //   arg2:
        //     The third object to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string representations
        //     of arg0, arg1, and arg2.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     format is null.
        //
        //   T:System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than two.
        public static String Format(String format, object arg0, object arg1, object arg2)
        {
            return System.String.Format(format, arg0, arg1, arg2);
        }
    }
}
