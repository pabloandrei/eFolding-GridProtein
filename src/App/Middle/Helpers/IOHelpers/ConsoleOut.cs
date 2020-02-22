using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public partial class ConsoleOutEspecial
    {

        ~ConsoleOutEspecial()
        {
            System.GC.SuppressFinalize(this);
        }

        ///// <summary>
        ///// Seta se será enviado ao console alguma informação
        ///// </summary>
        //public static bool Config = Convert.ToBoolean(System.AppConfigClient.ConfigurationManager.AppSettings["ConsoleWrite"]);

        ///// <summary>
        ///// Imprime no CONSOLE as coordenadas por pontos
        ///// </summary>
        ///// <param name="tempPrint"></param>
        //protected static void PrintPoints(Structs.Point tempPrint, int lastValed)
        //{
        //    if (Config)
        //        GICO.WriteLine("{0} --> x:{1}, y:{2}, z:{3}", ++lastValed, tempPrint.x, tempPrint.y, tempPrint.z);
        //}       
    }

    public class ConsoleOut
    {
        ~ConsoleOut()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Seta se será enviado ao console alguma informação
        /// </summary>
        public static bool ConsoleWrite = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ConsoleWrite"]);
        public static bool AppWrite = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AppWrite"]);

        ///// <summary>
        ///// Imprime no CONSOLE a ISEM em questão
        ///// </summary>
        ///// <param name="tempPrint"></param>
        //protected static void PrintIsem(int isem)
        //{
        //    if (Config)
        //        System.Console.WriteLine(Resource.Isem + ":{0}", isem);
        //}

        /// <summary>
        /// Imprime no CONSOLE uma linha em branco
        /// </summary>
        public static void WriteLine()
        {
            if (ConsoleWrite)
                System.Console.WriteLine();
        }

        ///// <summary>
        ///// Imprime no CONSOLE uma string
        ///// </summary>
        //public static void Write(string text)
        //{
        //    if (Config)
        //        System.Console.Write(text);
        //}

        public static void WriteLine(string text)
        {
            if (ConsoleWrite)
                System.Console.WriteLine(text);
        }

        public static void WriteLine(string masc, string text)
        {
            if (ConsoleWrite)
                System.Console.WriteLine(masc, text);
        }

        public static void WriteLine(Guid guid, string text)
        {
            if (ConsoleWrite)
                System.Console.WriteLine("{0}> {1}", guid.ToString(), text);
        }

        private static void WriteLine(string format, params object[] args)
        {

            if (ConsoleWrite)
                System.Console.WriteLine(string.Format(format, args));
        }

        //public static void Write(string format, params object[] args)
        //{

        //    if (Config)
        //        System.Console.Write(string.Format(format, args));
        //}

        public static void WriteLine(Exception exception)
        {
            if (ConsoleWrite)
            {
                if (exception != null)
                {
                    System.Console.WriteLine("InnerException: {0}", exception.InnerException != null ? exception.InnerException.ToString() : "InnerException not found!");
                    System.Console.WriteLine("Message: {0}", exception.Message != null ? exception.Message.ToString() : "Message not found!");
                    System.Console.WriteLine("StackTrace: {0}", exception.StackTrace != null ? exception.StackTrace.ToString() : "StackTrace not found!");
                }
                else
                {
                    System.Console.WriteLine("Message: {0}", exception.Message != null ? exception.Message.ToString() : "Message not found!");
                }
            }
        }

        public static void ForegroundColor(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
        }
    }
}
