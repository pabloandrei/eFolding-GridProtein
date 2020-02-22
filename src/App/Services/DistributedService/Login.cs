//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GridProteinFolding;

//namespace GridProteinFolding.Services.DistributedService
//{
//    public class Login
//    {

//        ~Login() {

//        }

//        public static bool LogOn(string user, string password, string hostName, List<string> macAdress, string address, int port, ref Guid guid)
//        {
            
//            guid = Guid.NewGuid();

//            ConsoleColor oldColour = Console.ForegroundColor;
//            GICO.ForegroundColor(ConsoleColor.Yellow);

//            GICO.Write("User logged {0} ({1})",user, hostName);
//            foreach (string tempFile in macAdress)
//            {
//                GICO.Write("/{0}",tempFile);
//            }
//            GICO.Write(") with Guid {0} - ",guid.ToString());
//            GICO.WriteLine("address:({0}:{1})", address, port.ToString());
//            GICO.WriteLine();

//            GICO.ForegroundColor(oldColour);

//            return true;

//        }

//        public static void LogOff(Guid guid)
//        {
//            GICO.WriteLine("User logoff Guid {0}", guid.ToString());
//        }
        
//    }
//}
