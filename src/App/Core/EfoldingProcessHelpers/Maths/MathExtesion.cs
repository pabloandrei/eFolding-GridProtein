using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace GridProteinFolding.Core.eFolding.Maths
{
    public class MathExtesion
    {
        [SecuritySafeCritical]
        public static double Pow(double var1, double var2)
        {
            return Math.Pow(var1, var2);
        }

        [SecuritySafeCritical]
        public static double Exp(double var1)
        {
            return Math.Exp(var1);
        }

    }
}
