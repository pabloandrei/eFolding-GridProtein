using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Core.eFolding.Parse;
using GridProteinFolding.Core.eFolding.IO;

namespace GridProteinFolding.Core.eFolding.MoveSet
{
    public class LatticeMoves : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        public LatticeMoves()
        {

        }

        ~LatticeMoves()
        {
            Disposed();
        }

        public void Disposed()
        {

        }

        /// <summary>
        /// Tenta efetuar o movimento de Ends
        /// </summary>
        /// <returns>Retorno True se o movimento for válido</returns>
        public bool LatticeTryModeSetEnds()
        {
            if (GCPS.chain.r[GCPS.chain.selectNode].classificationMotion.ends)
            {
                //Efetua MOVIMENTO de ENDS
                MoveSet objMove = new MoveSet();
                bool retTryEnds = objMove.Ends();
                objMove = null;

                return retTryEnds;
            }
            else
                return false;
        }

        /// <summary>
        /// Tenta efetuar o movimento de Kink
        /// </summary>
        /// <returns>Retorno True se o movimento for válido</returns>
        public bool LatticeTryModeSetKink()
        {
            if (GCPS.chain.r[GCPS.chain.selectNode].classificationMotion.kink)
            {
                //Efetua MOVIMENTO de ENDS
                MoveSet objMove = new MoveSet();
                bool retTryKink = objMove.Kink();
                objMove = null;

                return retTryKink;

            }
            return false;
        }


        /// <summary>
        /// Tenta efetuar o movimento de Crankshaft
        /// </summary>
        /// <returns>Retorno True se o movimento for válido</returns>
        public bool LatticeTryModeSetCrankshaft()
        {
            if (GCPS.chain.r[GCPS.chain.selectNode].classificationMotion.crankShaft__R1 ||
                GCPS.chain.r[GCPS.chain.selectNode].classificationMotion.crankShaft__R2
                )
            {
                //Efetua MOVIMENTO de Crankshaft
                MoveSet objMove = new MoveSet();
                bool retTryCrankshaft = objMove.Crankshaft();
                objMove = null;

                return retTryCrankshaft;

            }
            return false;
        }

        /// <summary>
        /// Sorteia o Monomero o qual ocorreá a tentiva de movimento
        /// </summary>
        /// <returns>Retorna a posição do Monomero</returns>
        public static void SortMoviment()
        {

            Randomic.Random();

            int selectNode = (int)(Randomic.randu * GCPS.chain.r.Count);

            GCPS.chain.selectNode = GCPS.tempChain.selectNode = selectNode;
        }

        /// <summary>
        /// Salva as posições do monomero em arquivo
        /// </summary>
        /// <param name="mList"></param>
        /// <param name="fileName"></param>
        /// <param name="guid"></param>
        public void RecTrajectoryFile(long mCStep, long totalMoviments, long numberOfMovementsApplied, List<Structs.BasicStructs.Point> mList, Guid guid, Int32 splitFileEvery, bool firstStep = false)
        {
            Stream.TrajectoryFile(mCStep, totalMoviments, numberOfMovementsApplied, mList, guid, splitFileEvery, firstStep);
        }

        public void RecTrajectoryFileSandBox(long mCStep, List<Structs.BasicStructs.Point> mList)
        {
            Stream.TrajectoryFileSandBox(mCStep, mList);
        }
        public void RecTrajectoryFileCalcSpinningRay_MIN(long mCStep, long totalMoviments, long numberOfMovementsApplied, List<Structs.BasicStructs.Point> mList, Guid guid)
        {
            Stream.RecTrajectoryFileCalcSpinningRay_MIN(mCStep, totalMoviments, numberOfMovementsApplied, mList, true, guid);
        }
        /// <summary>
        /// Grava qtos movimentos ocorreram por tipo e qual o número de tentativas dos mesmos.
        /// </summary>
        /// <param name="numberOfMove"></param>
        internal void RecMoveSet(Guid guid, string fileName)
        {
            Stream.RecMoveSet(guid, fileName);
        }


        /// <summary>
        /// Valida se o movimento ocorreu conforme regras de movimento!
        /// </summary>
        /// <returns></returns>
        public static bool Parse()
        {
            return new CheckContruction().Go();
        }

        //public void CheckFiles(string firstFile, string secondFile)
        //{
        //if (!CheckFile.FilesAreEqual(firstFile, secondFile))
        //{
        //    CustomMessage customMessage = new CustomMessage();

        //    throw new CustomException(SeverityLevel.Critical,
        //        LogLevel.Event,
        //        new Exception("ErroCheckFiles Exception"),
        //       customMessage.GetString("ErroCheckFiles"));
        //}
        //}
    }
}
