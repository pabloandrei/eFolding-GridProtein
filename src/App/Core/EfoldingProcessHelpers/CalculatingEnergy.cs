using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Core.eFolding
{
    /// <summary>
    /// Classe responsável pelo calculo energético do monómero
    /// </summary>
    public static class CalculatingEnergy
    {

        /// <summary>
        /// Atribui valor energético a um nó
        /// </summary>
        /// <param name="energy"></param>
        /// <param name="selectNode"></param>
        /// <param name="i"></param>
        public static void AddEnergy(double energy, int selectNode, int i)
        {
            Structs.BasicStructs.Point tempPoint = GCPS.tempChain.r[selectNode];
            Structs.BasicStructs.Point tempPointInverted = GCPS.tempChain.r[i];

            int count = tempPoint.e.Count(item => (item.i == i && item.selectNode == selectNode));
            count += tempPointInverted.e.Count(item => (item.i == selectNode && item.selectNode == i));

            //int idx = tempPoint.e.FindIndex(item => (item.i == i && item.selectNode == selectNode) || (item.i == selectNode && item.selectNode == i));
            if (count == 0)
            {
                //aplica nova energia na posição do node
                tempPoint.e.Add(new TypeE() { e = energy, selectNode = selectNode, i = i });
                GCPS.tempChain.r[selectNode] = tempPoint;

                GCPS.chain.interationEnergy += "Add;";
            }

        }

        /// <summary>
        /// Remove ou deleta valor de energia contido em E
        /// </summary>
        /// <param name="selectNode"></param>
        public static void DelEnergyFWD(int selectNode)
        {
            //nó principal
            if (GCPS.tempChain.r[selectNode].e.Count > 0)
            {
                while (GCPS.tempChain.r[selectNode].e.Count > 0)
                {
                    if (GCPS.tempChain.r[selectNode].e[0].selectNode == selectNode)
                    {
                        GCPS.tempChain.r[selectNode].e.RemoveAt(0);
                        GCPS.chain.interationEnergy += "DelFwd;";
                    }
                }
            }

        }


        /// <summary>
        /// Salva em arquivo o valor pertinentes ao movimento no DebugFile
        /// </summary>
        /// 
        public static bool existFileForSaveDebug = false;
        public static void SaveValueOfDebugFile()
        {
            string dir = IO.Directory.SubDirDebug;
            string pathFile = dir + @"\" + AppConfigClient.Param.files.Debug;

            if (!existFileForSaveDebug)
                existFileForSaveDebug = File.Exists(pathFile);

            if (existFileForSaveDebug)
            {
                long? len = File.Length(pathFile);
                if (len != null)
                {
                    if (len >= (1024 * 1024 * 1024)) //1 GB
                    {
                        AppConfigClient.Param.files.count++;
                        AppConfigClient.Param.files.Debug = "Debug" + (AppConfigClient.Param.files.count - 1) + Directory.FileExtension;
                    }
                }
            }

            bool append = true;
            ExtendedStreamWriter tw = new ExtendedStreamWriter(pathFile, append, Config.CurrentGuid, AppConfigClient.Crypt);
            StringBuilder output = new StringBuilder();

            //Se arquivo de saída tiver tamanho igual zero (0), significa que é um novo arquivo.
            if (tw.BaseStream().Length == 0)
            {
                //Portanto, monta-se o header do arquivo de saída
                output.Append(
                    "McSteps\t" +
                    "contMoves:NumberOfMovementsApplied\t" +

                    "environment:DeltaE\t" +
                    "environment:RG\t" +
                    "environment:DPP\t" +
                    "environment:R\t" +
                    "environment:TransitionProbability\t" +

                    "chain:UPrevious\t" +
                    "chain:U\t" +

                    "chain:TotalNeighborTopological\t" +
                    "chain:selectNode\t" +
                    "chain:typeOfLattice\t" +
                    "chain:neighborContacts\t" +
                    "chain:numberNeighborTopological");
                tw.WriteLine(output.ToString());
                output.Clear();
            }


            output.Append(
                GCPS.McSteps + "\t" +
                GCPS.chain.contMoves.NumberOfMovementsApplied + "\t" +

                Structs.Environment.deltaE + "\t" +
                Structs.Environment.rg + "\t" +
                Structs.Environment.dpp + "\t" +
                Structs.Environment.lastR + "\t" +
                Structs.Environment.transitionProbability + "\t" +

                GCPS.chain.UPrevious + "\t" +
                GCPS.chain.U + "\t" +
                GCPS.chain.TotalNeighborTopological() + "\t" +
                GCPS.chain.selectNode + "\t" +
                GCPS.chain.typeOfLattice + "\t" +
                GCPS.chain.neighborContacts + "\t" +
                GCPS.chain.numberNeighborTopological);
            tw.WriteLine(output.ToString());


            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }

    }
}


///// <summary>
///// Exclui valor enegértico de um nó
///// </summary>
///// <param name="selectNode"></param>
//public static void DelEnergy(int selectNode) {


//    if (GCPS.tempMonomero.typeOfLattice != Enums.Lattices.Crank)
//    {
//        DelEnergyFWD(selectNode, true);
//    }
//    else
//    {
//        int internalSelectNode = GCPS.monomero.selectNode;

//        DelEnergyFWD(selectNode, false);

//        ////Movimento de crankShaft é sempre em função de R1
//        ////Abaixo efetuaremos o ajustes do Ri
//        //if (GCPS.monomero.r[internalSelectNode].classificationMotion.crankShaft__R0)
//        //{
//        //    DelEnergyFWD(selectNode + 1, false);
//        //    DelEnergyFWD(selectNode + 2, false);
//        //    DelEnergyFWD(selectNode + 3, false);
//        //}
//        //else if (GCPS.monomero.r[internalSelectNode].classificationMotion.crankShaft__R1)
//        //{
//        //    //DelEnergyFWD(selectNode - 1, false);
//        //    DelEnergyFWD(selectNode + 1, false);
//        //    DelEnergyFWD(selectNode + 2, false);
//        //}
//        //else if (GCPS.monomero.r[internalSelectNode].classificationMotion.crankShaft__R2)
//        //{
//        //    //DelEnergyFWD(selectNode - 2, false);
//        //    //DelEnergyFWD(selectNode - 1, false);
//        //    DelEnergyFWD(selectNode + 1, false);
//        //}
//        ////else if (GCPS.monomero.r[internalSelectNode].classificationMotion.crankShaft__R3)
//        ////{

//        ////    //DelEnergyFWD(selectNode - 3, false);
//        ////    //DelEnergyFWD(selectNode - 2, false);
//        ////    //DelEnergyFWD(selectNode - 1, false);
//        ////}
//    }
//}

//private static void DelEnergyFWD(int selectNode, bool executeDelEnergyREV)
//{
//    //nó principal
//    DelEnergyFWD(int selectNode)

//    //nó secundário
//    if (executeDelEnergyREV)
//    {
//        DelEnergyREV(selectNode);
//    }
//}

//private static void DelEnergyREV(int selectNode)
//{
//    //nó associados ao principal
//    for (int i = 0; i < GCPS.tempMonomero.r.Count; i++)
//    {
//        int count = GCPS.tempMonomero.r[i].e.Count;

//        if (count > 0)
//        {
//            for (int j = count-1; j >-1 ; j--)
//            {
//                if (GCPS.tempMonomero.r[i].e[j].i == selectNode)
//                {
//                    GCPS.tempMonomero.r[i].e.RemoveAt(j);
//                    GCPS.monomero.typeOfMove += "DelRev;";
//                }
//            }
//        }
//    }
//}