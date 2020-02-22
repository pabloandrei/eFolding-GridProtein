using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security;

namespace GridProteinFolding.Middle.Helpers.ConfigurationHelpers
{

    public class DirBaseService : IDirBaseService
    {
        public string dirBaseCliente;
        public string dirBaseServer;
        public string dirBaseWeb;
        public string dirUploadFolder;

        private static DirBaseService _dirBaseService;
        public DirBaseService()
        {
            //dirBaseCliente = ConfigurationManager.AppSettings["dirBaseClient"] == null ? string.Empty : ConfigurationManager.AppSettings["dirBaseClient"].ToString();
            dirBaseCliente = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\efolding.fcfrp.usp.br\Client\";

            dirBaseServer = ConfigurationManager.AppSettings["dirBaseServer"] == null ? string.Empty : ConfigurationManager.AppSettings["dirBaseServer"].ToString();
            dirBaseWeb = ConfigurationManager.AppSettings["dirBaseWeb"] == null ? string.Empty : ConfigurationManager.AppSettings["dirBaseWeb"].ToString();

            //dirUploadFolder = ConfigurationManager.AppSettings["uploadFolder"] == null ? string.Empty : ConfigurationManager.AppSettings["uploadFolder"].ToString();
            dirUploadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\efolding.fcfrp.usp.br\Client\_temp\";
        }

        public static DirBaseService GetDirBaseService()
        {
            if (_dirBaseService == null)
            {
                _dirBaseService = new DirBaseService();
            }

            return _dirBaseService;
        }

    }

    public interface IDirBaseService
    {

    }
}