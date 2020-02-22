using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

namespace GridProteinFolding.Core.eFolding
{
    /// <summary>
    /// CLass EnergeticMatrix
    /// </summary>
    public class EnergeticMatrix : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Carrega em memória os valores da "Matriz energética"
        /// </summary>
        public static void LoadEnergeticMatrix(Param param)
        {        
            int N = GCPS.chain.r.Count; //número de residuos  

            GCPS.energeticMatrix = new double[N, N]; //Matriz M quadrada (NxN)

            for (int i = 0; i < (N - 1); i++)
            {
                for (int j = i + 1; j < N; j++)
                {

                    if (i == j)
                    {
                        GCPS.energeticMatrix[i, j] = 0;
                    }
                    else
                    {
                        double r = 0;

                        if (GCPS.modelType == 0)
                        {
                            //0-Randon

                            //Sorteio randomico
                            Randomic.Random();
                            r = (Randomic.randu * 2) - 1; //R´=(R*2)-1                           

                        }
                        else if (GCPS.modelType == 1)
                        {
                            //1-H (Hidrofobico)
                            r = -1;
                        }
                        else if (GCPS.modelType == 2)
                        {
                            //2-P (Polar)
                            r = 1;
                        }

                        else //if (GCPS.modelType == 2)
                        {
                            //3-HP;4-estero-quimico,5-GO

                            //O AJUSTE SOMENTE É VALIDO PARA MODELO HP
                            //Onde: H=-, P=+
                            //Sendo: Eij(H,H) = -1, Eij(H,P)=0, Eij(P,P)= +1

                            if (param.model[i].value == -1 && param.model[j].value == -1)
                            {
                                r = -1;
                            }
                            else if (param.model[i].value == 1 && param.model[j].value == 1)
                            {
                                r = 1;
                            }

                        }
                        GCPS.energeticMatrix[i, j] = r;
                        GCPS.energeticMatrix[j, i] = r;
                    }
                }
            }
        }

        /// <summary>
        /// Salva em arquivo os valores da "Matriz energética"
        /// </summary>
        public static void SaveEnergeticMatrix()
        {
            string file = GCPS.chain.isem.ToString();

            string dir = IO.Directory.SubDirNeighbors;
            string pathFile = dir + @"\Me_" + file + Consts.extensionOfFile;

            ExtendedStreamWriter sw = new ExtendedStreamWriter(Config.Crypt);
            sw.CreateText(pathFile,Config.CurrentGuid);

            for (int i = 0; i < GCPS.chain.r.Count; i++)
            {
                for (int j = 0; j < GCPS.chain.r.Count; j++)
                {
                    sw.Write(String.Format(@"{0}", GCPS.energeticMatrix[i, j]).ToString() + "\t");
                }
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();

        }

    }
}
