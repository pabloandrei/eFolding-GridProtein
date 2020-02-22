using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.MoveSet;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
using GridProteinFolding.Core.eFolding.IO;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using System;

namespace GridProteinFolding.Core.eFolding
{
    public static class Recorder
    {
        public static void RecMov(LatticeMoves objLatticeMoves, Int32 splitFileEvery)
        {
            RecTrajectoryFile(objLatticeMoves, splitFileEvery);
        }

        public static void RecTrajectoryFileCalcSpinningRay(Guid guid, long maxInterations, LatticeMoves objLatticeMoves)
        {
            PrinterMessages.PrintBootomHeader(guid, maxInterations);

            //Calculo do ultimo raio de giração
            double calcSpinningRay = RecWriteCalcSpinningRay();

            if (calcSpinningRay > 2.1412 && calcSpinningRay < 2.1413)
            {
                RecTrajectoryFileCalcSpinningRay_MIN(objLatticeMoves);
            }


        }

        public static void RecTrajectoryFile(LatticeMoves objLatticeMoves, Int32 splitFileEvery, bool firstStep = false)
        {
            long mCStep = GCPS.McSteps;
            long totalMoviments = GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement;
            long numberOfMovementsApplied = GCPS.chain.contMoves.NumberOfMovementsApplied;

            objLatticeMoves.RecTrajectoryFile(mCStep, totalMoviments, numberOfMovementsApplied, GCPS.chain.r, Config.Param.dataToProcess.Guid, splitFileEvery, firstStep);
        }

        public static void RecTrajectoryFileSandBox(LatticeMoves objLatticeMoves)
        {
            long mCStep = GCPS.McSteps;
            objLatticeMoves.RecTrajectoryFileSandBox(mCStep, GCPS.chain.r);
        }

        public static void RecTrajectoryFileCalcSpinningRay_MIN(LatticeMoves objLatticeMoves)
        {
            long mCStep = GCPS.McSteps;
            long totalMoviments = GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement;
            long numberOfMovementsApplied = GCPS.chain.contMoves.NumberOfMovementsApplied;

            objLatticeMoves.RecTrajectoryFileCalcSpinningRay_MIN(mCStep, totalMoviments, numberOfMovementsApplied, GCPS.chain.r, Config.Param.dataToProcess.Guid);

        }

        public static double RecWriteCalcSpinningRay()
        {
            long sumOfAcceptedAndRejectedMovement = GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement;
            string fileName = "CalcSpinningRay" + Directory.FileExtension;

            double calcSpinningRay = Maths4Simulation.CalcSpinningRay();

            Stream.StreamRecCalcSpinningRay(sumOfAcceptedAndRejectedMovement, calcSpinningRay, fileName);

            return calcSpinningRay;
        }
    }
}
