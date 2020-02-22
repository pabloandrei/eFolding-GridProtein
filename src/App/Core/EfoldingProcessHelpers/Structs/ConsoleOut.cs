using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Core.eFolding.Structs;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public partial class ConsoleOutEspecial : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Seta se será enviado ao console alguma informação
        /// </summary>
        public static bool Config = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ConsoleWrite"]);

        /// <summary>
        /// Imprime no CONSOLE as coordenadas por pontos
        /// </summary>
        /// <param name="tempPrint"></param>
        protected static void PrintPoints(BasicStructs.Point tempPrint, int lastValed)
        {
            if (Config)
                Console.WriteLine("{0} --> x:{1}, y:{2}, z:{3}", ++lastValed, tempPrint.x, tempPrint.y, tempPrint.z);
        }
    }


}
