//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GridProteinFolding.Middle.Integration;
//using System.Configuration;
//using System.Diagnostics;
//using GridProteinFolding.Middle.Helpers.NetworkHelpers;
//using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
//using SIO = System.IO;
//using GridProteinFolding.Middle.Helpers.IOHelpers;

//namespace GridProteinFolding.Services.DistributedService
//{
//    public sealed class ApplicationState
//    {
//        /// <summary>
//        /// Metodo Dispose da Classe
//        /// </summary>
//        public void Dispose()
//        {
//            System.GC.SuppressFinalize(this);
//        }

//        private static ApplicationState instance = null;
//        private static readonly object padlock = new object();


//        public static ApplicationState Instance
//        {
//            get
//            {
//                lock (padlock)
//                {
//                    if (instance == null)
//                    {
//                        instance = new ApplicationState();
//                    }
//                    return instance;
//                }
//            }
//        }

//        public AppConfigService applicationConfig;



//        public ApplicationState()
//        {
//            applicationConfig = new AppConfigService();
//        }


//    }
   
//}

