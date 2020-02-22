//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace GridProteinFolding.Common.Exceptions
//{
//    /// <summary>
//    /// Classe o qual envia para o Console as Exception
//    /// </summary>
//    public class OutPut
//    {
//        public static void WriteException(System.Exception ex, Exceptions.Types.ErrorLevel typeOfException)
//        {
//            WriteException(string.Empty, ex, typeOfException);
//        }

//        /// <summary>
//        /// Imprime no CONSOLE um Exception
//        /// </summary>
//        /// <param name="ex"></param>
//        public static void WriteException(string personalMessage, System.Exception ex, Exceptions.Types.ErrorLevel typeOfException)
//        {
//            if (typeOfException != Exceptions.Types.ErrorLevel.Warning)
//            {

//                Console.WriteLine();
//                if (personalMessage != string.Empty)

//                    Console.WriteLine(Resource.PersonalMessage + ":{0}", personalMessage);
//                Console.WriteLine(Resource.TypeOfException + ":{0}", typeOfException.ToString());
//                Console.WriteLine(Resource.Exception + ":{0}", ex.Message);
//                Console.WriteLine(Resource.StackTrace + ":{0}", ex.StackTrace);
//                Console.WriteLine();
//            }

//        }

//        /// <summary>
//        /// Imprime no CONSOLE um ERRO
//        /// </summary>
//        /// <param name="tempPrint"></param>
//        protected static void PrintError(string exception)
//        {
//            Console.WriteLine(Resource.Error + ":{0}", exception);
//        }

//    }
//}
