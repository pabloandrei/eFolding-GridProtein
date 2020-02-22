//using System;
//using System.Threading;
//using System.Resources;
//using System.Reflection;
//using System.Globalization;

//namespace GridProteinFolding.Logging.Exception
//{
//    /// <summary>
//    /// Summary description for CustomMessage.
//    /// </summary>
//    internal class CustomMessage
//    {
//        internal CustomMessage()
//        {
//        }
//        internal string GetString(string key)
//        {
//            // Cria o resource manager para obter resources.
//            ResourceManager rm = new ResourceManager(typeof(Exceptions));//"Exceptions", Assembly.GetExecutingAssembly());
            
//            //Cultura correntye
//            CultureInfo ci = Thread.CurrentThread.CurrentCulture;

//            return rm.GetString(key, ci);;
//        }
//    }
//}
