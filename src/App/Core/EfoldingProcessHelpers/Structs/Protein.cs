using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using System.Text.RegularExpressions;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Core.eFolding.Parse;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Protein
    /// </summary>
    public static class Protein
    {

        /// <summary>
        /// Cria um Monometro
        /// </summary>
        /// <param name="tempCoord"></param>
        public static void CreateMono(ref BasicStructs.Point tempCoord)
        {
            tempCoord = GCPS.chain.r[GCPS.chain.r.Count - 1]; //Copia o último ponto válido para a coordenada temporária

            //Sorteira X,Y ou Z
            Randomic.Random();
            if (Randomic.randu < Consts.rXYZ) //X
            {
                Randomic.Random();
                tempCoord.x = Randomic.randu < 0.5 ? tempCoord.x - 1 : tempCoord.x + 1; //escolhe +/- 1
            }
            else if (Randomic.randu < (Consts.rXYZ * 2)) //Y
            {

                Randomic.Random();

                tempCoord.y = Randomic.randu < 0.5 ? tempCoord.y - 1 : tempCoord.y + 1; //escolhe +/- 1
            }
            else
            { //Z                    
                Randomic.Random();
                tempCoord.z = Randomic.randu < 0.5 ? tempCoord.z - 1 : tempCoord.z + 1; //escolhe +/- 1
            }
        }

        /// <summary>
        /// Prepara aplicação para gerar uma nova ISEM
        /// </summary>
        public static void ClearAppToGenNewIsem()
        {
            GCPS.chain.r.Clear();
            Randomic.Random();
            GCPS.initialIsem = AppConfigClient.Param.dataToProcess.isem < 0 ? (AppConfigClient.Param.dataToProcess.isem * -1) : AppConfigClient.Param.dataToProcess.isem;
            GCPS.chain.isem = GCPS.initialIsem;
            GCPS.chain.chainTypes = null;
        }

        /// <summary>
        /// Rotina o qual costroi a cadeia de monomeros
        /// </summary>
        /// <param name="lastValed">posição do último monomero válido</param>
        public static void CreateStruct(ref int lastValed)
        {

            while (GCPS.chain.r.Count < AppConfigClient.Param.dataToProcess.totalSitio)
            {
                BasicStructs.Point tempCoord = new BasicStructs.Point(); //Crio uma coordenada temporária  

                lastValed = GCPS.chain.r.Count - 1;

                tempCoord.x = GCPS.chain.r[lastValed].x; //Copia o último ponto válido para a coordenada temporária
                tempCoord.y = GCPS.chain.r[lastValed].y;
                tempCoord.z = GCPS.chain.r[lastValed].z;

                tempCoord.deadEndPoints = string.Empty;
                tempCoord.deadEnd = 0;

                //Sorteira X,Y ou Z
                Randomic.Random();
                if (Randomic.randu < Consts.rXYZ) //X
                {
                    Randomic.Random();
                    tempCoord.x = Randomic.randu < 0.5 ? tempCoord.x - 1 : tempCoord.x + 1; //escolhe +/- 1
                }
                else if (Randomic.randu < (Consts.rXYZ * 2)) //Y
                {

                    Randomic.Random();
                    tempCoord.y = Randomic.randu < 0.5 ? tempCoord.y - 1 : tempCoord.y + 1; //escolhe +/- 1
                }
                else
                { //Z                    
                    Randomic.Random();
                    tempCoord.z = Randomic.randu < 0.5 ? tempCoord.z - 1 : tempCoord.z + 1; //escolhe +/- 1
                }


                //aidiconar valor de enegia inicial
                tempCoord.e = new List<TypeE>();

                //adiciona o novo ponto
                GCPS.chain.r.Add(tempCoord);
            }
        }

        /// <summary>
        /// Check sobreposicao e Primeiro vizinho
        /// </summary>
        /// <param name="chainType"></param>
        /// <param name="cadeiaIdeal"></param>
        /// <param name="cadeiaReal"></param>
        /// <param name="cadeiaSoft"></param>
        public static void CheckOverRideAndFirstNeighbor(ref BasicEnums.ChainTypes chainType, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft)
        {
            if (CheckContruction.CheckOverRide()) //Checagem de sobreposição
            {
                cadeiaIdeal++;
                chainType = BasicEnums.ChainTypes.Ideal;
            }
            else
            {
                if (CheckContruction.FirstNeighbor()) //Primeiro Vizinho
                {
                    cadeiaReal++;
                    chainType = BasicEnums.ChainTypes.Real;
                }
                else
                {
                    cadeiaSoft++;
                    chainType = BasicEnums.ChainTypes.Soft;
                }
            }

        }

        /// <summary>
        /// Check na construcao das cadeias
        /// </summary>
        /// <param name="chainType"></param>
        /// <param name="tempQueueType"></param>
        public static void QualificationChainTypes(ref BasicEnums.ChainTypes chainType, ref QueueType tempQueueType)
        {

            if ((chainType == BasicEnums.ChainTypes.Ideal))  //1-	Cadeia IDEAL inclui Cadeias REAL e SOFT, portanto devemos considerar neste grupo, para todos os efeitos, todas as 10000 cadeia geradas, ou seja todas as 10000                
            {
                Queue<QueueType>.valuesCadeiaIdeal.Enqueue(tempQueueType);

#if Debug
                SaveSeedCoordAtDirSeed();
#endif
            }

            if ((chainType == BasicEnums.ChainTypes.Real)) //2-	Cadeia REAL inclui CADEIA SOFT , portanto temos que considerar neste grupo 38+3=41 cadeias para todos os efeitos; e finalmente
            {

                Queue<QueueType>.valuesCadeiaReal.Enqueue(tempQueueType);

#if Debug
                SaveSeedCoordAtDirSeed();
#endif

                //}
            }

            if (chainType == BasicEnums.ChainTypes.Soft) //3-	Cadeia SOFT está com a contagem correta.
            {
                Queue<QueueType>.valuesCadeiaSoft.Enqueue(tempQueueType);

#if Debug
                SaveSeedCoordAtDirSeed();
#endif
            }
        }

#if Debug
        /// <summary>
        /// Save as coordenadas geradas da SEMENTE
        /// </summary>
        private static void SaveSeedCoordAtDirSeed()
        {

                string dir = GridProteinFolding.Process.IO.Directory.DirSeed;
                string pathFile = dir + @"\" + GCPS.chain.isem + Consts.extensionOfFile;

                ExtendedStreamWriter sw = new ExtendedStreamWriter();
                sw.CreateText(pathFile, Config.currentGuid);

                sw.WriteLine("{0}", GCPS.chain.isem);

                for (int i = 0; i < GCPS.chain.r.Count; i++)
                {
                    BasicStructs.Point temp = GCPS.chain.r[i];
                    sw.WriteLine("{0}\t{1}\t{2}", temp.x, temp.y, temp.z);
                }

                sw.Flush();
                sw.Close();


        }
#endif

        /// <summary>
        /// Carrega as coordenadas geradas da SEMENTE
        /// </summary>
        public static void LoadSeedCoord(string file, bool firstLineIsIsemNUmber)
        {
            //string dir = Members.workSpace + @"\" + Resource.SubDirSeed;
            //string pathFile = dir + @"\" + file + Consts.extensionOfFile;

            using (ExtendedStreamReader sr = new ExtendedStreamReader(file, Config.CurrentGuid, true))
            {

                string line = string.Empty;
                string[] temp = new string[3];

                GCPS.Reset();

                bool first = true;
                while ((line = sr.ReadLine()) != null)
                {

                    if (first && firstLineIsIsemNUmber)
                    {
                        GCPS.chain.isem = Convert.ToInt32(line);
                        first = false;
                    }
                    else
                    {

                        temp = Regex.Split(line, "\t"); //temp = line.Split(new char[] { '\t' });

                        GCPS.chain.r.Add(new BasicStructs.Point()
                        {
                            x = Convert.ToInt32(temp[0]),
                            y = Convert.ToInt32(temp[1]),
                            z = Convert.ToInt32(temp[2]),
                            deadEnd = 0,
                            deadEndPoints = string.Empty
                        });
                    }
                }

                sr.Close();
                sr.Dispose();
            }

        }

    }
}
