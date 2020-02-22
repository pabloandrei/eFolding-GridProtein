using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SIO = System.IO;
using System.Security;

namespace GridProteinFolding.Middle.Helpers.ConfigurationHelpers
{
    /// <summary>
    /// Class AppConfigClient
    /// </summary>
    public class AppConfigClient
    {

        ~AppConfigClient()
        {
            System.GC.SuppressFinalize(this);
        }


        public AppConfigClient()
        {
        }

        private static Param param;

        public static Param Param
        {
            get { return param; }
            set { param = value; }
        }

        public static bool Crypt
        {
            get { return AppConfigClient.Param.dataToProcess.crypt; }
        }

        public static Guid CurrentGuid
        {
            get { return param.dataToProcess.Guid; }
        }


        public static SIO.DirectoryInfo DirBaseClient
        {
            get
            {
                return new SIO.DirectoryInfo(DirBaseService.GetDirBaseService().dirBaseCliente + @"\" + param.dataToProcess.Guid.ToString());

            }
        }

        public static SIO.DirectoryInfo Debug
        {
            get
            {
                return new SIO.DirectoryInfo(DirBaseService.GetDirBaseService().dirBaseCliente + @"\" + param.dataToProcess.Guid.ToString() + @"\Debug");

            }
        }

        public static SIO.DirectoryInfo Result
        {
            get
            {
                return new SIO.DirectoryInfo(DirBaseService.GetDirBaseService().dirBaseCliente + @"\" + param.dataToProcess.Guid.ToString() + @"\Result");

            }
        }

        private static SIO.DirectoryInfo uploadFolder = null;
        public static SIO.DirectoryInfo UploadFolder
        {
            get
            {
                if (uploadFolder == null)
                {
                    uploadFolder = new SIO.DirectoryInfo(DirBaseService.GetDirBaseService().dirUploadFolder);
                    return uploadFolder;
                }
                else
                {
                    return uploadFolder;
                }
            }
        }

        private static SIO.DirectoryInfo loadFolder = null;
        public static SIO.DirectoryInfo LoadFolder
        {
            get
            {
                if (loadFolder == null)
                {
                    loadFolder = new SIO.DirectoryInfo(DirBaseService.GetDirBaseService().dirUploadFolder);
                    return loadFolder;
                }
                else
                {
                    return loadFolder;
                }
            }
        }
    }
}
