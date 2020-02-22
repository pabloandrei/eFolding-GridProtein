using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using System.Configuration;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using AppConfigService = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigService;
using GridProteinFolding.Middle.Helpers.TypesHelpers;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Middle.Integration
{
    public class DecryptIntegrator : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private static ExtendedDirectoryInfo dirBaseServer = new ExtendedDirectoryInfo(ConfigurationManager.AppSettings["dirBaseServer"].ToString());

        public static string DirBaseServer
        {
            get { return DecryptIntegrator.dirBaseServer.FullName(); }
        }

        private static ExtendedDirectoryInfo dirBaseWeb = new ExtendedDirectoryInfo(ConfigurationManager.AppSettings["dirBaseWeb"].ToString());

        public static string DirBaseWeb
        {
            get { return DecryptIntegrator.dirBaseWeb.FullName(); }
        }

        private static string dirDebug;
        private static string dirTrajectory;
        private Param param;


        public DecryptIntegrator(Param param)
        {
            this.param = param;

        }

        public void Decrypt()
        {
            try
            {
                dirDebug = DirBaseServer + this.param.dataToProcess.Guid + @"\" + Resource.DirDebug + @"\" + Resource.DirDebug;
                dirTrajectory = DirBaseServer + this.param.dataToProcess.Guid + @"\" + Resource.DirResult + @"\" + Resource.SubDirTrajectory;
                string destination = DirBaseWeb + this.param.dataToProcess.Guid + @"\" + Resource.DirDebug + @"\";

                string[] diretories = { dirDebug, dirTrajectory };

                DecrypAllFiles(diretories, destination);

            }
            catch (Exception ex)
            {
                GICO.WriteLine(ex);
                throw;
            }

        }

        //static string Server = "Server";
        //static string Web = "Web";

        private void DecrypAllFiles(string[] diretories, string destination)
        {

            foreach (string path in diretories)
            {
                //string newWebFolder = path.Replace(Server, Web);
                Directory.CreateDirIfNotExist(destination);

                foreach (string tempFile in Directory.GetFilesOfDir(path))
                {
                    string newWebFile = string.Empty;

                    //newWebFile = tempFile.Replace(Server, Web);
                    newWebFile = destination + Path.GetFileName(tempFile);

                    string extensionFile = Path.GetExtension(tempFile);

                    if (extensionFile == ".xls")
                    {
                        File.Copy(tempFile, newWebFile, true);
                    }
                    else
                    {
                        if (param.dataToProcess.crypt)
                        {
                            //DADOS
                            string line;
                            using (ExtendedStreamReader fileReader = new ExtendedStreamReader(tempFile, this.param.dataToProcess.Guid, AppConfigClient.Crypt))
                            {
                                ExtendedStreamWriter fileWriter = new ExtendedStreamWriter(newWebFile, false, this.param.dataToProcess.Guid, false);
                                while ((line = fileReader.ReadLine()) != null)
                                {
                                    if (String.IsNullOrEmpty(line))
                                        break;

                                    fileWriter.WriteLine(line);

                                }
                                fileReader.Close();
                                //fileReader = null;
                                fileWriter.Flush();
                                fileWriter.Close();
                                fileWriter = null;
                                fileReader.Dispose();
                            }
                            //FIM DADOS
                        }
                        else
                        {
                            File.Copy(tempFile, newWebFile, true);
                        }
                    }

                }
            }
        }
    }
}
