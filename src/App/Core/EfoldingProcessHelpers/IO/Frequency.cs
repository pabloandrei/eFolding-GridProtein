using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
//using SIO = System.IO;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;


namespace GridProteinFolding.Core.eFolding.IO
{
    public class CalcOfFrequency : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        //Nome comum dos arquivos
        protected static string firstNameFiles = "file";
        const string spinningRayValues = "RadiuosOfGyration";
        const string calDistanceBetweenLastPointFirst = "EndToEndDistance";
        const string histogram = "Histogram";

        public static void WriteFrequencyPeerInterval(BasicEnums.HistTypes histType, BasicEnums.ChainTypes chainType)
        {

            string fileName = firstNameFiles + chainType.ToString();

            if (histType == BasicEnums.HistTypes.RadiuosOfGyration)
                WriteFrequencyPeerInterval(fileName, spinningRayValues, histType, chainType);
            else
            {
                if (histType == BasicEnums.HistTypes.EndToEndDistance)
                    WriteFrequencyPeerInterval(fileName, calDistanceBetweenLastPointFirst, histType, chainType);
            }
        }

        private static void WriteFrequencyPeerInterval(string fileName, string extFileName, BasicEnums.HistTypes histType, BasicEnums.ChainTypes chainType)
        {
            StringBuilder sb = Maths4Simulation.FrequencyPeerInterval(fileName + extFileName, histType, chainType);

            if (sb != null)
            {
                string dir = IO.Directory.SubDirHistogram;
                string pathFile = dir + @"\" + fileName + histogram + extFileName + Directory.FileExtension;

                string[] ret = Regex.Split(sb.ToString(), "\n");

                ExtendedStreamWriter sw = new ExtendedStreamWriter(Config.Crypt);
                sw.CreateText(pathFile, Config.CurrentGuid);

                for (int i = 0; i < ret.Count(); i++)
                {
                    string retPrinter = ret[i];
                    sw.WriteLine(retPrinter);
                }

                sw.Flush();
                sw.Close();
            }

            return;
        }



        public static double[] LoadData(BasicEnums.ChainTypes chainType, BasicEnums.HistTypes histType)
        {
            string file = firstNameFiles + chainType.ToString();

            string dir = IO.Directory.SubDirCoord;
            string pathFileSpinningRayValue = dir + @"\" + file + histType.ToString() + Consts.extensionOfFile;

            ExtendedStreamReader srSpinningRay = new ExtendedStreamReader(pathFileSpinningRayValue, Config.CurrentGuid, Config.Crypt);

            string allText = srSpinningRay.ReadToEnd();

            if (allText != null)
            {
                string[] arrString = Regex.Split(allText, ";");
                //Array.Resize(ref arrString, arrString.Length -1); //tira o bug do ENTER

                srSpinningRay.Close();
                srSpinningRay.Dispose();

                double[] arrDouble = new double[arrString.Length - 1];
                for (int i = 0; i < arrString.Length - 1; i++)
                {
                    arrDouble[i] = Convert.ToDouble(arrString[i]);
                }

                return arrDouble;
            }
            else
                return null;
        }

        public static void SaveData(BasicEnums.ChainTypes chainType, Structs.Queue<QueueType> queue)
        {
            string file = firstNameFiles + chainType.ToString();

            string dir = IO.Directory.SubDirCoord;
            string pathFileIsem = dir + @"\" + file + Consts.extensionOfFile;
            string pathFileSpinningRayValue = dir + @"\" + file + spinningRayValues + Consts.extensionOfFile;
            string pathCalDistanceBetweenLastPointFirst = dir + @"\" + file + calDistanceBetweenLastPointFirst + Consts.extensionOfFile;


            ExtendedStreamWriter swIsem = new ExtendedStreamWriter(Config.Crypt);
            swIsem.AppendText(pathFileIsem, Config.CurrentGuid);

            ExtendedStreamWriter swSpinningRay = new ExtendedStreamWriter(Config.Crypt);
            swSpinningRay.AppendText(pathFileSpinningRayValue, Config.CurrentGuid);

            ExtendedStreamWriter swCalDistanceBetweenLastPointFirst = new ExtendedStreamWriter(Config.Crypt);
            swCalDistanceBetweenLastPointFirst.AppendText(pathCalDistanceBetweenLastPointFirst, Config.CurrentGuid);

            StringBuilder sbIsem = new StringBuilder();
            StringBuilder sbSpinningRay = new StringBuilder();
            StringBuilder sbCalDistanceBetweenLastPointFirst = new StringBuilder();

            while (queue.Count > 0)
            {
                QueueType type = queue.Dequeue();

                sbIsem.AppendFormat("{0}{1}", type.isemName, ";");
                sbSpinningRay.AppendFormat(@"{0}{1}", type.spinningRayValue, ";");
                sbCalDistanceBetweenLastPointFirst.AppendFormat(@"{0}{1}", type.calDistanceBetweenLastPointFirst, ";");

                swSpinningRay.Write(sbSpinningRay.ToString());
                swCalDistanceBetweenLastPointFirst.Write(sbCalDistanceBetweenLastPointFirst.ToString());
                swIsem.Write(sbIsem.ToString());

                sbIsem.Clear();
                sbSpinningRay.Clear();
                sbCalDistanceBetweenLastPointFirst.Clear();

            }

            swSpinningRay.Close();
            swCalDistanceBetweenLastPointFirst.Close();
            swIsem.Close();
        }

        //protected static void DirSpinningRay()
        //{
        //    string dir = Members.workSpace + @"\" + Resource.SubDirCoord;
        //}
    }
}
