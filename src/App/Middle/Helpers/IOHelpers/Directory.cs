using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using SIO = System.IO;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    public static class Directory
    {
        public static string FileExtension = ".dat";

        /// <summary>
        /// Deleta o arquivo se o mesmo existir
        /// </summary>
        /// <param name="pathFile">Arquivo mais o caminho fisico</param>
        public static void DeleteFileIfExists(string pathFile)
        {
            if (SIO.File.Exists(pathFile))
                SIO.File.Delete(pathFile);
        }

        public static void Delete(string pathFile)
        {
            SIO.Directory.Delete(pathFile);
        }

        public static void DeleteFileAndDirIfExists(string location, Guid? guid = null)
        {
            if (SIO.Directory.Exists(location))
            {
                string[] filePaths = SIO.Directory.GetFiles(location);
                foreach (string filePath in filePaths)
                {
                    if (guid != null)
                    {
                        if (filePath.Contains(guid.ToString()))
                            SIO.File.Delete(filePath);
                    }
                    else
                    {
                        SIO.File.Delete(filePath);
                    }
                }

                //SIO.Directory.Delete(location);
            }
        }

        /// <summary>
        /// Cria diretório se o mesmo não existir
        /// </summary>
        /// <param name="dir"></param>
        public static bool CreateDirIfNotExist(string dir)
        {

            if (!SIO.Directory.Exists(dir))
            {
                SIO.Directory.CreateDirectory(dir);
                return true;
            }
            return false;
        }

        public static System.IO.DirectoryInfo CreateDirectory(string path)
        {
            return SIO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Função efetuado preparação dos folder nso quais a aplicação irá utilizar
        /// </summary>
        public static void PreperFolderForLocationsDebug(string[] locationsDebug)
        {

            CreateDirIfNotExist(AppConfigClient.LoadFolder.FullName);
            CreateDirIfNotExist(AppConfigClient.UploadFolder.FullName);

            //GICO.WriteLine(configuration.dirBaseClient.FullName);
            if (CreateDirIfNotExist(AppConfigClient.DirBaseClient.FullName))
            {
                if (CreateDirIfNotExist(AppConfigClient.Debug.FullName))
                {
                    for (int i = 0; i < locationsDebug.Length; i++)
                    {
                        CreateDirIfNotExist(locationsDebug[i]);
                    }

                }
                else
                    DeleteFileIfExists(locationsDebug);
            }
        }

        //public static void PreperFolderForLocationsResult(string[] locationsResult)
        //{

        //    CreateDirIfNotExist(AppConfigClient.LoadFolder.FullName);
        //    CreateDirIfNotExist(AppConfigClient.UploadFolder.FullName);

        //    //GICO.WriteLine(configuration.DirBaseClient.FullName);
        //    if (CreateDirIfNotExist(AppConfigClient.DirBaseClient.FullName))
        //    {
        //        if (CreateDirIfNotExist(AppConfigClient.Result.FullName))
        //        {
        //            for (int i = 0; i < locationsResult.Length; i++)
        //            {
        //                CreateDirIfNotExist(locationsResult[i]);
        //            }

        //        }
        //        else
        //            DeleteFileResultIfExists(locationsResult);
        //    }
        //}

        public static void ClearProcessClientFolder(string[] locationsDebug)
        {
            for (int i = 0; i < locationsDebug.Length; i++)
            {
                DeleteFilesOfFolder(locationsDebug[i]);
            }

        }

        public static void DeleteFilesOfFolder(string path)
        {
            if (SIO.Directory.Exists(path))
            {
                SIO.DirectoryInfo directory = new SIO.DirectoryInfo(path);

                GridProteinFolding.Middle.Helpers.IOHelpers.File.DeleteAllFilesOfDirectory(directory);
            }
        }


        /// <summary>
        /// Deleta arquivos existentes de Debug, se existirem
        /// </summary>
        public static void DeleteFileIfExists(string[] locationsDebug)
        {

            for (int i = 0; i < locationsDebug.Length; i++)
            {
                if (SIO.Directory.Exists(locationsDebug[i]))
                    DeleteFileAndDirIfExists(locationsDebug[i], null);
                else
                    CreateDirIfNotExist(locationsDebug[i]);
            }

        }


        /// <summary>
        /// Retorna array de string contendo nome de todos arquivos existentes no folder
        /// </summary>
        /// <returns></returns>
        public static string[] GetFilesOfDir(string dirSeed)
        {
            return SIO.Directory.GetFiles(dirSeed);
        }

        public static string[] GetFilesOfDir(string path, string searchPattern)
        {
            return SIO.Directory.GetFiles(path, searchPattern);
        }

        public static bool Exists(string path)
        {
            return SIO.Directory.Exists(path);
        }

        public static string[] GetFiles(string path)
        {
            return SIO.Directory.GetFiles(path);
        }
    }
}
