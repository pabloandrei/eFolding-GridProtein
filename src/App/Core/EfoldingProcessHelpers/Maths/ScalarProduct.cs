using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Core.eFolding.Structs;

namespace GridProteinFolding.ProcessHelpers.MathsHelpers
{
    public class ScalarProduct : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Calcula o produto escalar de 2 vetores
        /// </summary>
        /// <returns>Se retorno for 0, então ocorre.</returns>
        public static int Calc(BasicStructs.Point point1, BasicStructs.Point point2, BasicStructs.Point point3)
        {

            int x1, y1, z1;
            int x2, y2, z2;
            int x3, y3, z3;
            int v1ov2;

            x1 = point1.x;
            y1 = point1.y;
            z1 = point1.z;

            x2 = point2.x;
            y2 = point2.y;
            z2 = point2.z;

            x3 = point3.x;
            y3 = point3.y;
            z3 = point3.z;

            v1ov2 = ((x2 - x1) * (x3 - x2)) + ((y2 - y1) * (y3 - y2)) + ((z2 - z1) * (z3 - z2));

            return v1ov2;
        }

    }
}
