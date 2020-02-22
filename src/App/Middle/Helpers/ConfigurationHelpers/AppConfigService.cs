using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SIO = System.IO;
using System.Security;

namespace GridProteinFolding.Middle.Helpers.ConfigurationHelpers
{
    public class AppConfigService
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        ~AppConfigService()
        {
            System.GC.SuppressFinalize(this);
        }

        public AppConfigService()
        { }

        public AppConfigService(Param Params)
        {
            param = Params;
        }

        private static Param param;

        public static Param Param
        {
            get { return param; }
            set { param = value; }
        }

        public static Guid CurrentGuid
        {
            get { return param.dataToProcess.Guid; }
        }

    }
}
