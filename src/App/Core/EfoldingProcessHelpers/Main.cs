using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GICOF = GridProteinFolding.Core.eFolding.IO;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.LoggingHelpers.User_Defined_Exceptions;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GridProteinFolding.Core.eFolding.MoveSet;
using GridProteinFolding.Core.eFolding.Classification;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using System.Security;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Core.eFolding
{
    public class Main : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        //Check do teste de sequencia
        BasicEnums.ChainTypes chainType = new BasicEnums.ChainTypes();
        //Variavel o qual contem o numero de sobreposicoes
        int cadeiaIdeal = 0;
        //Variavel o qual contem o numero de primeiros vizinhos topologicos
        int cadeiaReal = 0;
        int cadeiaSoft = 0;
        int cadeiaError = 0;

        public bool Process(ref Param param)
        {
            GridProteinFolding.Middle.Helpers.LoggingHelpers.Log.InformationLog("Process begin...");
            #region Set configuracao      
            AppConfigClient.Param = param;
            Middle.Helpers.RandomHelpers.Randomic.magicNumber = param.configApp.MagicNumber;
            Middle.Helpers.RandomHelpers.Randomic.equal_one = param.configApp.EqualOne;
            #endregion  

            GICO.WriteLine(AppConfigClient.CurrentGuid, "Connected...");

            //PREPARA os DIRETORIOS

            //ConsoleOut.WriteLine(AppConfigClient.CurrentGuid, "Working in temp folder(PreperFolderForLocationsDebug)...");
            new GridProteinFolding.Core.eFolding.IO.Directory().PreperFolderForLocationsDebug();

            //posição do último monomero válido
            int lastValed = int.MinValue;
            GCPS.initialIsem = AppConfigClient.Param.dataToProcess.isem;

            //Inicializa o Monomero da simulação
            GCPS.chain = new Structs.BasicStructs.Chain();

            GCPS.modelType = AppConfigClient.Param.dataToProcess.modelType;
            //GCPS.model = new List<Structs.BasicStructs.Model>();

            //if (GCPS.model.Count > 0)
            //{
            //    foreach (String tempSplit in param.dataToProcess.model.Split(';'))
            //    {
            //        Structs.BasicStructs.Model tempModel = new Structs.BasicStructs.Model();
            //        string[] tempSubSplit = tempSplit.Split(',');
            //        tempModel.Monomero = Convert.ToByte(tempSubSplit[0]);
            //        tempModel.Value = float.Parse(tempSubSplit[1]);
            //        GCPS.model.Add(tempModel);
            //    }
            //}

            //Seleciona o tipo de formação de estrutura a ser utilizada
            //Geração Randomica?
            //Geração por arquivo?
            //Geração apartir de um modelo?
            double cont = 0;
            SelectTypeOfStruct(ref lastValed, ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);

            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Total Monomeros: {0}", AppConfigClient.Param.dataToProcess.maxInterations));
            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Cadeia ideal   : {0}", cadeiaIdeal));
            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Cadeia real    : {0}", cadeiaReal));
            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Cadeia soft    : {0}", cadeiaSoft + "(" + (AppConfigClient.Param.dataToProcess.maxInterations - cadeiaIdeal - cadeiaReal) + ")"));
            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Cadeia error   : {0}", cadeiaError));

            GICO.WriteLine(AppConfigClient.CurrentGuid, "Salving results...");
            GICOF.CalcOfFrequency.SaveData(BasicEnums.ChainTypes.Ideal, Structs.Queue<QueueType>.valuesCadeiaIdeal);
            GICOF.CalcOfFrequency.SaveData(BasicEnums.ChainTypes.Real, Structs.Queue<QueueType>.valuesCadeiaReal);
            GICOF.CalcOfFrequency.SaveData(BasicEnums.ChainTypes.Soft, Structs.Queue<QueueType>.valuesCadeiaSoft);

            ConsoleOut.WriteLine(AppConfigClient.CurrentGuid, "Creating histogram...");
            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.RadiuosOfGyration, BasicEnums.ChainTypes.Ideal);
            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.EndToEndDistance, BasicEnums.ChainTypes.Ideal);

            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.RadiuosOfGyration, BasicEnums.ChainTypes.Real);
            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.EndToEndDistance, BasicEnums.ChainTypes.Real);

            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.RadiuosOfGyration, BasicEnums.ChainTypes.Soft);
            GICOF.CalcOfFrequency.WriteFrequencyPeerInterval(BasicEnums.HistTypes.EndToEndDistance, BasicEnums.ChainTypes.Soft);


            ConsoleOut.WriteLine(AppConfigClient.CurrentGuid, "MCSteps finished.");

            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GridProteinFolding.Middle.Helpers.LoggingHelpers.Log.InformationLog("Process completed...");
            return true;
        }

        private void SelectTypeOfStruct(ref int lastValed, ref double cont, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft, ref int cadeiaError)
        {

            if (AppConfigClient.Param.dataToProcess.targets != null)
            {
                ModelForTargets(ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);
            }
            else if (!AppConfigClient.Param.dataToProcess.loadDatFile)
            {
                ModelForRandom(ref lastValed, ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);
            }
            else
            {
                ModelForFile(ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);
            }
        }

        private void ModelForTargets(ref double cont, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft, ref int cadeiaError)
        {
            GCPS.chain.r = Targets.CreateTarget(AppConfigClient.Param.dataToProcess.targets.targetsCoordinates);

            ProcessMonomero(ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);

            //Prepara aplicação para gerar uma nova ISEM
            Protein.ClearAppToGenNewIsem();
        }


        private void ModelForRandom(ref int lastValed, ref double cont, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft, ref int cadeiaError)
        {
            for (cont = 0; cont < AppConfigClient.Param.dataToProcess.maxInterations; cont++)
            {

                GCPS.chain.r.Add(new Structs.BasicStructs.Point()
                {
                    x = 50,
                    y = 50,
                    z = 50,
                    deadEnd = 0,
                    deadEndPoints = string.Empty,
                    neighbors = new Structs.BasicStructs.Neighbor[6],
                    e = new List<TypeE>()
                });
                //Constroi a cadeia de monomeros
                Protein.CreateStruct(ref lastValed);

                ProcessMonomero(ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);


                //Para processo qual encontrar o primeiro do tipo REAL OU SOFT
                if ((GCPS.chain.chainTypes == BasicEnums.ChainTypes.Soft || GCPS.chain.chainTypes == BasicEnums.ChainTypes.Real) && AppConfigClient.Param.internalProcess.stopWhenSoft)
                {
                    cont = AppConfigClient.Param.dataToProcess.maxInterations;
                }

                //Prepara aplicação para gerar uma nova ISEM
                Protein.ClearAppToGenNewIsem();

            }
        }

        private void ModelForFile(ref double cont, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft, ref int cadeiaError)
        {
            string[] lines = Regex.Split(AppConfigClient.Param.dataToProcess.file, "\r\n");

            //foreach (string line in lines)
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                try
                {
                    Structs.BasicStructs.Point tempCoord = new Structs.BasicStructs.Point();
                    string[] coord = line.Split(new char[] { '\t' });
                    tempCoord.x = Convert.ToInt32(coord[0]);
                    tempCoord.y = Convert.ToInt32(coord[1]);
                    tempCoord.z = Convert.ToInt32(coord[2]);
                    GCPS.chain.r.Add(tempCoord);
                }
                catch (FormatException ex)
                {
                    GICO.WriteLine(ex);
                }
            }

            ProcessMonomero(ref cont, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft, ref cadeiaError);

            //Prepara aplicação para gerar uma nova ISEM
            Protein.ClearAppToGenNewIsem();
        }


        /// <summary>
        /// Essa rotina faz:
        /// a) Efetua calculo raio de giração
        /// b) Check sobreposição e primeiro vizinho
        /// c) Check n# contrucao das cadeias
        /// d) Se a cadeia for do tipo REAL, aplica movimentação da cadeia (MONTEC) 
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="cadeiaIdeal"></param>
        /// <param name="cadeiaReal"></param>
        private bool ProcessMonomero(ref double cont, ref int cadeiaIdeal, ref int cadeiaReal, ref int cadeiaSoft, ref int cadeiaError)
        {
            string isemName = GCPS.initialIsem.ToString();

            GCPS.chain.contMoves = new Structs.BasicStructs.MoveSetApplies();

            //Efetua o calculo do Raio de giração & Distancia entre primeiro e ultimo ponto
            QueueType tempQueueType = new QueueType(isemName, Maths4Simulation.CalcSpinningRay(), Maths4Simulation.CalDistanceBetweenLastPointFirst());

            //Check sobreposicao e Primeiro vizinho
            Protein.CheckOverRideAndFirstNeighbor(ref chainType, ref cadeiaIdeal, ref cadeiaReal, ref cadeiaSoft);

            //Check na construcao das cadeias
            Protein.QualificationChainTypes(ref chainType, ref tempQueueType);

            //Se for CADEIA REAL SOFT aplica movimento
            GCPS.chain.chainTypes = chainType;
            if (GCPS.chain.chainTypes == BasicEnums.ChainTypes.Soft || GCPS.chain.chainTypes == BasicEnums.ChainTypes.Real)
            {
                //CHECK CONSTRUCTION
                if (!LatticeMoves.Parse())
                {
                    GCPS.chain.chainTypes = BasicEnums.ChainTypes.Error;
                    cadeiaError++;
                    cadeiaSoft--;
                    //GICO.WriteLine("ErroParseMoves..." + GCPS.chain.isem.ToString());
                    new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ErroParseMoves(new ErroParseMovesException("ErroParseMoves..." + GCPS.chain.isem.ToString()), Types.ErrorLevel.None);
                    return false;
                }
                else
                {
                    //Rotina carrega e salva matriz energética
                    EnergeticMatrix.LoadEnergeticMatrix(AppConfigClient.Param);
                    EnergeticMatrix.SaveEnergeticMatrix();

                    //Inicializa o Monomero da simulação utilizado para movimentação (temporário)    
                    GCPS.tempChain = new Structs.BasicStructs.TemporaryChain();
                    GCPS.chain.CopyToTempMonomero();

                    //Rotina executa: Processa calculo da "variação de energia" e salva em arquivo o valor de "U"
                    GCPS.tempChain.NeighborsUpdateAll();
                    GCPS.tempChain.NeighborsSave(GCPS.tempChain.isem, ref GCPS.chain.numberNeighborTopological);

                    GCPS.chain.AcceptMotion();

                    //classificação do Monomero quanto ao tipo de movimento possível e Salva em disco
                    ClassificationMotion.PreClassificationOfMotion();

#if Debug
                    ClassificationMotion.SaveClassificationOfMotion(GCPS.chain.isem.ToString());
#endif

                    //Aplica Movimentação: CHAMA MONTE CARLO

                    //return MoteCarlo.MonteCarloB.Run();
                    return MoteCarlo.MonteCarloTsallis.Run();
                }
            }

            return false;
        }
    }
}

