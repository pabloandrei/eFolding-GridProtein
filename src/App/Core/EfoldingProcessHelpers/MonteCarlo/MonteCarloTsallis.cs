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
    public class MonteCarloTsallis : MontaCarloBase, IDisposable
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

            //StateControl.condStop = (GCPS.chain.r.Count + 1) * 2;
            StateControl.maxInterations = AppConfigClient.Param.dataToProcess.maxInterations; //numero maximo de tentativas 
            StateControl.recPathEvery = 0;

            PrinterMessages.PrintHeader(AppConfigClient.CurrentGuid, StateControl.maxInterations);//, StateControl.condStop);

            //Salva a primeira configuração do monomero
            Recorder.RecTrajectoryFile(objLatticeMoves, AppConfigClient.Param.dataToProcess.splitFileEvery, true);

            //Salva snapshot calculo da "variação de energia" e salva em arquivo o valor de "U"
            CalculatingEnergy.existFileForSaveDebug = false;
            CalculatingEnergy.SaveValueOfDebugFile();

            //int maxValConTop = 0;
            bool stoped = false;

            DateTime startDateTime = DateTime.Now;
            Console.WriteLine("Start: {0}", startDateTime.ToString());

            while (!stoped)
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
                    //Reclassifica TODA A CADEIA
                    GCPS.tempChain.NeighborsUpdateAll();

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

                        //Condição de parada
                        if (GCPS.chain.numberNeighborTopological >= (GCPS.chain.r.Count))
                        {
                            FoundData(startDateTime);
                            Snapshot(ref objLatticeMoves);

                            if (CheckStruct())
                            {
                                stoped = true;
                            }
                        }
                    }

                    GCPS.chain.interationEnergy = string.Empty;
                    GCPS.chain.typeOfLattice = BasicEnums.Lattices.None;

                    //Se movimento ocorreu, é necessário pré qualificar novamente toda a cadeia
                    ClassificationMotion.PreClassificationOfMotion();
                }
                GCPS.McStepsDo();
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



            //Compara estruturas com as TARGETS
            //ComparerTargets(GCPS.chain.r);
            #endregion

            //MoveSet de saída do movimento para SANDBOX
            Recorder.RecTrajectoryFileSandBox(objLatticeMoves);

            objLatticeMoves.Disposed();
            objLatticeMoves = null;

            return true;
        }

    }
}
