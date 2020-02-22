using System;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

namespace GridProteinFolding.Core.eFolding.IO
{
    /// <summary>
    /// Class Directory
    /// </summary>
    public class Directory : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        public static string FileExtension = ".dat";


        /// <summary>
        /// Retorna caminho do diretório Seed
        /// </summary>
        public static string SubDirSeed
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirSeed; }

        }

        /// <summary>
        /// Retorna caminho do diretório Coord
        /// </summary>
        public static string SubDirCoord
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirCoord; }
        }

        /// <summary>
        /// Retorna caminho do diretório ClassificationOfMotion
        /// </summary>
        public static string SubDirClassificationOfMotion
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirClassificationOfMotion; }
        }

        /// <summary>
        /// Retorna caminho do diretório Neighbors
        /// </summary>
        public static string SubDirNeighbors
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirNeighbors; }
        }

        /// <summary>
        /// Retorna caminho do diretório DeadEnd
        /// </summary>
        public static string SubDirDeadEnd
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirDeadEnd; }
        }

        /// <summary>
        /// Retorna caminho do diretório Histogram
        /// </summary>
        public static string SubDirHistogram
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirHistogram; }
        }

        /// <summary>
        /// Retorna caminho do diretório MoveSet
        /// </summary>
        public static string SubDirTrajectorySet
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirResult + @"\" + Resource.SubDirTrajectorySet; }
        }

        /// <summary>
        /// Retorna caminho do diretório Excel
        /// </summary>
        public static string SubDirExcel
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirResult + @"\" + Resource.SubDirExcel; }
        }

        /// <summary>
        /// Retorna caminho do diretório DirUfile
        /// </summary>
        public static string SubDirDebug
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirDebug; }
        }

        public static string SubDirBlob
        {
            get { return AppConfigClient.DirBaseClient.FullName + @"\" + Resource.DirDebug + @"\" + Resource.SubDirBlob; }
        }
        

        string[] locationsDebug = new string[9] {
            SubDirSeed,
            SubDirCoord,
            SubDirClassificationOfMotion,
            SubDirNeighbors,
            SubDirDeadEnd,
            SubDirHistogram,
            SubDirTrajectorySet,
            SubDirDebug,
            SubDirBlob
        };

        string[] locationsResult = new string[1] {
            SubDirExcel
        };

        /// <summary>
        /// Função efetuado preparação dos folder nso quais a aplicação irá utilizar
        /// </summary>
        public void PreperFolderForLocationsDebug()
        {
            GridProteinFolding.Middle.Helpers.IOHelpers.Directory.PreperFolderForLocationsDebug(locationsDebug);
        }

        /// <summary>
        /// Função efetuado limpeza dos folder nos quais a aplicação cliente utilizou
        /// </summary>
        public void ClearProcessClientFolder()
        {
            GridProteinFolding.Middle.Helpers.IOHelpers.Directory.ClearProcessClientFolder(locationsDebug);
        }


        /// <summary>
        /// Deleta arquivos existentes de Debug, se existirem
        /// </summary>
        private void DeleteFileDebugIfExists()
        {
            GridProteinFolding.Middle.Helpers.IOHelpers.Directory.DeleteFileIfExists(locationsDebug);
        }

        /// <summary>
        /// Deleta arquivos existentes de Results, se existirem
        /// </summary>
        private void DeleteFileResultIfExists()
        {
            GridProteinFolding.Middle.Helpers.IOHelpers.Directory.DeleteFileIfExists(locationsResult);
        }

        /// <summary>
        /// Retorna array de string contendo nome de todos arquivos existentes no folder Seed
        /// </summary>
        /// <returns></returns>
        public static string[] FilesOfDirSeed()
        {
            return GridProteinFolding.Middle.Helpers.IOHelpers.Directory.GetFilesOfDir(SubDirSeed);
            //return .Directory.GetFiles(Directory.SubDirSeed);
        }
    }
}
