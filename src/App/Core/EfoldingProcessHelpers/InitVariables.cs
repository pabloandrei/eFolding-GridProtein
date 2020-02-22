using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;

namespace GridProteinFolding.Core.eFolding
{
    public static class InitVariables
    {
        //Inicializa as variáveis necessárias para contagem de movimento
        public static void Do()
        {
            GCPS.McStepsReset();
            GCPS.mcStepsLoop01 = 0;
            GCPS.mcStepsLoop02 = 0;
            GCPS.mcStepsLoop03 = 0;


            GCPS.chain.contMoves.crankshaftAccept = 0;
            GCPS.chain.contMoves.endsAccept = 0;
            GCPS.chain.contMoves.kinkAccept = 0;
            GCPS.chain.contMoves.othersReject = 0;
            GCPS.chain.contMoves.crankshaftReject = 0;
            GCPS.chain.contMoves.endsReject = 0;
            GCPS.chain.contMoves.kinkReject = 0;

            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_1 = 0;
            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_2 = 0;
            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_3 = 0;
            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_1 = 0;
            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_2 = 0;
            GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_3 = 0;

            GCPS.chain.contMoves.moveSetEnd.endAccept_1 = 0;
            GCPS.chain.contMoves.moveSetEnd.endAccept_2 = 0;
            GCPS.chain.contMoves.moveSetEnd.endAccept_3 = 0;
            GCPS.chain.contMoves.moveSetEnd.endAccept_4 = 0;
            GCPS.chain.contMoves.moveSetEnd.endReject_1 = 0;
            GCPS.chain.contMoves.moveSetEnd.endReject_2 = 0;
            GCPS.chain.contMoves.moveSetEnd.endReject_3 = 0;
            GCPS.chain.contMoves.moveSetEnd.endReject_4 = 0;
        }
    }
}
