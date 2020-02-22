using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

namespace GridProteinFolding.Core.eFolding
{
    public static class PrinterMessages
    {
        public static int lastNumber = -1;
        public static void PrintHeader(Guid guid, long maxInterations)//, int condStop)
        {
            //GICO.WriteLine(guid, String.Format("Condition to Stop (topological contacts): {0}", condStop.ToString()));
            //GICO.WriteLine(guid, String.Format("Max mcStepsLoop01: {0}", maxInterations.ToString()));
            GICO.WriteLine(guid, String.Format("MCSteps: {0}", (maxInterations * 5 * GCPS.chain.r.Count)));
            //GICO.WriteLine(guid, String.Format("Interations/Max mcStepsLoop01: {0}", GCPS.mcStepsLoop01 + @"/" + maxInterations));
            lastNumber = -1;
        }

        public static void PrintPercentCompleted(Guid guid)
        {
            long maxInterations = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient.Param.dataToProcess.maxInterations;

            if (GCPS.mcStepsLoop01 != 0)
            {
                double percent = GCPS.mcStepsLoop01;
                percent = ((percent * 100) / maxInterations);

                int value = Convert.ToInt32(percent.ToString("00.##").Substring(0, 2));

                switch (value)
                {
                    case 10:
                    case 20:
                    case 30:
                    case 40:
                    case 50:
                    case 60:
                    case 70:
                    case 80:
                    case 90:
                    case 100:
                        if (lastNumber < value)
                        {
                            lastNumber = value;
                            GICO.WriteLine(guid, lastNumber + "%.. ");
      

                           // IO.Stream.StreamRecBlob(Marshalling.WriteToBinaryFile<Simulation>(string.Empty, GCPS.chain., false));
                        
                        }
                        break;
                }
            }
            else
                GICO.WriteLine(guid, "0%.. ");
        }

        public static void PrintBootomHeader(Guid guid, long maxInterations)
        {
            GICO.WriteLine(guid, String.Format("MC step:{0}", GCPS.McSteps));// maxInterations * GCPS.mcStepsLoop02_LIMITE * GCPS.chain.r.Count);
            GICO.WriteLine(guid, String.Format("NumberOfMovementsApplied {0}.", GCPS.chain.contMoves.NumberOfMovementsApplied.ToString()));
            GICO.WriteLine(guid, String.Format("NumberOfMovementsRejected {0}.", GCPS.chain.contMoves.NumberOfMovementsRejected.ToString()));
            GICO.WriteLine(guid, String.Format("SumOfAcceptedAndRejectedMovement {0}.", GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement.ToString()));

        }
    }
}
