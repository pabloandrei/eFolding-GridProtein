using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.LoggingHelpers.User_Defined_Exceptions;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Core.eFolding.MoveSet;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GridProteinFolding.Core.eFolding.Classification;
using GridProteinFolding.Core.eFolding.IO;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Core.eFolding.MoteCarlo
{
    /// <summary>
    /// Classe MonteCarlo
    /// </summary>
    public class MonteCarloBoltzmann : MontaCarloBase, IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            base.Dispose();
        }

        public static class StateControl
        {
            public static int condStop = 0;
            public static long maxInterations = 0;
            public static int recPathEvery = 0;
        }

        /// <summary>
        /// Aplica TEMPO de passo em MONTE CARLO!
        /// </summary>
        public static bool Run()
        {
            LatticeMoves objLatticeMoves = new LatticeMoves();

            InitVariables.Do();

            #region Criação Movimentação MoveSet

            StateControl.condStop = (GCPS.chain.r.Count + 1) * 2;
            StateControl.maxInterations = AppConfigClient.Param.dataToProcess.maxInterations; //numero maximo de tentativas 
            StateControl.recPathEvery = 0;

            PrinterMessages.PrintHeader(AppConfigClient.CurrentGuid, StateControl.maxInterations);//, StateControl.condStop);

            //Salva a primeira configuração do monomero
            Recorder.RecTrajectoryFile(objLatticeMoves, AppConfigClient.Param.dataToProcess.splitFileEvery, true);

            //Salva snapshot calculo da "variação de energia" e salva em arquivo o valor de "U"
            CalculatingEnergy.existFileForSaveDebug = false;
            CalculatingEnergy.SaveValueOfDebugFile();


            int maxValConTop = 0;

            DateTime startDateTime = DateTime.Now;
            Console.WriteLine("Start: {0}", startDateTime.ToString());

            for (GCPS.mcStepsLoop01 = 0; GCPS.mcStepsLoop01 < StateControl.maxInterations; GCPS.mcStepsLoop01++)
            {
                //PRINT %
                PrinterMessages.PrintPercentCompleted(AppConfigClient.CurrentGuid);

                for (GCPS.mcStepsLoop02 = 0; GCPS.mcStepsLoop02 < GCPS.mcStepsLoop02_LIMITE; GCPS.mcStepsLoop02++) //loop externo de 5 passos
                {
                    for (GCPS.mcStepsLoop03 = 0; GCPS.mcStepsLoop03 < GCPS.chain.r.Count; GCPS.mcStepsLoop03++) //loop para tamanha da cadeia
                    {
                        GCPS.chain.CopyToTempMonomero();

                        //Sorteiro o residuo para a tentativa de movimento
                        LatticeMoves.SortMoviment();

                        //var occuredMotion armazena o boleano da ocorrência de movimentos dos três (3) tipos abaixo:
                        bool occuredMotion = objLatticeMoves.LatticeTryModeSetCrankshaft();
                        if (!occuredMotion)
                        {
                            occuredMotion = objLatticeMoves.LatticeTryModeSetEnds();
                            if (!occuredMotion)
                            {
                                occuredMotion = objLatticeMoves.LatticeTryModeSetKink();
                                if (!occuredMotion)
                                {
                                    GCPS.chain.contMoves.othersReject++;
                                }
                            }
                        }

                        //Verifica se ocorreu um movimento na cadeia temporária
                        if (occuredMotion)
                        {
                            //Excluir valores energia (e) anterior se existentes ao node
                            //CalculatingEnergy.DelEnergy(GCPS.tempMonomero.selectNode);                            
                            //Rotina executa: Processa calculo da "variação de energia" e salva em arquivo o valor de "U"  Reclassifica a cadeia TEMPORARIA
                            //GCPS.tempMonomero.NeighborsUpdatePeerPoint(GCPS.tempMonomero.selectNode);


                            //Reclassifica TODA A CADEIA
                            GCPS.tempChain.NeighborsUpdateAll();


                            #region
                            //Condição de Teste:
                            //(1) deltaE <=0 -> "Aceita a nova configuração"
                            //(2) deltaE >0 -> Então
                            //      - Calcula e^-deltaE*beta (calcE)
                            //      - Gerar um número randôico (r) entre 0 e 1
                            //      Se r<=calcE então "Aceita a nova configuração"
                            //          Senão "Rejeita a nova configuração"
                            #endregion
                            //Efetua calcula das varívais do meio ambiente (deltaE, lastR e lastTransitionProbability, RG, DPP e etc)
                            Structs.Environment.CalculateEnvironment();

                            if (AcceptMove())
                            {

                                //Salva classificaca da cadeia
                                GCPS.tempChain.NeighborsSave(GCPS.chain.isem, ref GCPS.chain.numberNeighborTopological);

                                //Aceita o movimento
                                GCPS.chain.AcceptMotion();

                                if (GCPS.chain.ECount() != (GCPS.chain.TotalNeighborTopological() / 2))
                                {
                                    new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ErrorNeighborTopological(new ErrorNeighborTopological("GCPS.chain.ECount() != (GCPS.chain.TotalNeighborTopological() / 2"), Types.ErrorLevel.Warning);
                                }

                                //Snapshot(ref objLatticeMoves);
                                //REC
                                if (GCPS.chain.numberNeighborTopological >= (GCPS.chain.r.Count - 1))
                                {
                                    FoundData(startDateTime);
                                    Snapshot(ref objLatticeMoves);
                                }

                                //Condição de parada
                                if (GCPS.chain.numberNeighborTopological >= (GCPS.chain.r.Count))
                                {
                                    FoundData(startDateTime);

                                    Snapshot(ref objLatticeMoves);

                                    if (Structs.Environment.rg == 1.41)
                                        break;
                                }
                                else
                                {
                                    if (maxValConTop < GCPS.chain.numberNeighborTopological)
                                    {
                                        maxValConTop = GCPS.chain.numberNeighborTopological;

                                        FoundData(startDateTime);
                                    }

                                }
                            }

                            GCPS.chain.interationEnergy = string.Empty;
                            GCPS.chain.typeOfLattice = BasicEnums.Lattices.None;

                            //Se movimento ocorreu, é necessário pré qualificar novamente toda a cadeia
                            ClassificationMotion.PreClassificationOfMotion();

                            if (StateControl.condStop == GCPS.chain.TotalNeighborTopological())
                            {
                                break;
                            }
                        }
                        GCPS.McStepsDo();
                    }
                }
            }

            //Salva o movimento da cadeia
            Recorder.RecTrajectoryFileCalcSpinningRay(AppConfigClient.CurrentGuid, StateControl.maxInterations, objLatticeMoves);

            GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("MCSteps: {0}", GCPS.McSteps));
            //Check Parse do ultimo arquivo valido
            if (!LatticeMoves.Parse())
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ErroParseMoves(new ErroParseMovesException("ErroParseMoves"), Types.ErrorLevel.None);

            //MoveSet de saída do movimento do ultimo arquivo valido
            Recorder.RecTrajectoryFile(objLatticeMoves, AppConfigClient.Param.dataToProcess.splitFileEvery);

            //MoveSet (RESULT)
            objLatticeMoves.RecMoveSet(AppConfigClient.CurrentGuid, SimulationResults + Directory.FileExtension);

            objLatticeMoves.Disposed();
            objLatticeMoves = null;

            #endregion

            return true;
        }

        private static void Snapshot(ref LatticeMoves objLatticeMoves)
        {
            //Valida processo de Snapshot
            if (AppConfigClient.Param.dataToProcess.recPathEvery == 1 || AppConfigClient.Param.dataToProcess.recPathEvery == 0)
            {
                SaveData(ref objLatticeMoves);
            }
            else
            {
                if (StateControl.recPathEvery < (AppConfigClient.Param.dataToProcess.recPathEvery - 1))
                {
                    StateControl.recPathEvery++;
                }
                else
                {
                    SaveData(ref objLatticeMoves);
                    StateControl.recPathEvery = 0;
                }

            }

        }

    }
}
