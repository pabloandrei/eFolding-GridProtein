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
using GridProteinFolding.Middle.Helpers.IOHelpers;
using System.IO;
using static GridProteinFolding.Core.eFolding.Structs.BasicStructs;

namespace GridProteinFolding.Core.eFolding.MoteCarlo
{
    public class MontaCarloBase : IDisposable
    {
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        public static string SimulationResults = "SimulationResults";


        public static void Snapshot(ref LatticeMoves objLatticeMoves)
        {
            SaveData(ref objLatticeMoves);
        }

        public static void SaveData(ref LatticeMoves objLatticeMoves)
        {

            //Salva snapshot calculo da "variação de energia" e salva em arquivo o valor de "U"
            CalculatingEnergy.SaveValueOfDebugFile();
            //Salva o movimento da cadeia
            Recorder.RecMov(objLatticeMoves, AppConfigClient.Param.dataToProcess.splitFileEvery);
        }

        private static bool AcceptMoveBoltzmann()
        {
            double dE = Structs.Environment.deltaE;
            return ((dE <= 0) ||
                    (
                        (dE > 0) &&
                        (Structs.Environment.lastR <= Structs.Environment.transitionProbability)
                    ));
        }

        private static bool AcceptMoveTsallis()
        {
            return AcceptMoveBoltzmann();
        }

        public static bool AcceptMove()
        {
            if (true)
                return AcceptMoveBoltzmann(); //return Temperature.Do();
            else
                return AcceptMoveTsallis();
        }

        public static void FoundData(DateTime startDateTime)
        {
            DateTime nowDateTime = DateTime.Now;
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Found at:\t{0}", nowDateTime.ToString());
            Console.WriteLine("Nº contacts:\t{0}", GCPS.chain.numberNeighborTopological);
            Console.WriteLine("Dif Start:\t{0}", (nowDateTime - startDateTime).ToString());
            Console.WriteLine("RG:\t{0}", Structs.Environment.rg);
            Console.WriteLine("Steps:\t{0}", Simulation.McSteps);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

        }

        public static bool CheckStruct()
        {
            int maxX = int.MinValue, minX = int.MaxValue;
            int maxY = int.MinValue, minY = int.MaxValue;
            int maxZ = int.MinValue, minZ = int.MaxValue;

            for (int i = 0; i < GCPS.chain.r.Count(); i++)
            {
                //X
                if (GCPS.chain.r[i].x > maxX)
                    maxX = GCPS.chain.r[i].x;
                else if (GCPS.chain.r[i].x < minX)
                    minX = GCPS.chain.r[i].x;

                //Y
                if (GCPS.chain.r[i].y > maxY)
                    maxY = GCPS.chain.r[i].y;
                else if (GCPS.chain.r[i].y < minY)
                    minY = GCPS.chain.r[i].y;

                //Z
                if (GCPS.chain.r[i].z > maxZ)
                    maxZ = GCPS.chain.r[i].z;
                else if (GCPS.chain.r[i].z < minZ)
                    minZ = GCPS.chain.r[i].z;
            }

            int difX = maxX - minX;
            int difY = maxY - minY;
            int difZ = maxZ - minZ;

            return ((difX == 2) && (difY == 2) && (difZ == 2));
        }
    }
}
