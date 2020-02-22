using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Core.eFolding.Classification
{
    /// <summary>
    /// Classe de controle do movimento de CrankShaft
    /// </summary>
    public class CrankShaft : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Verifica se o CrankShaft ocorre a esquerda
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsCrankShaftLeft(int index)
        {
            try
            {
                int len = (GCPS.chain.r.Count)-1;
                int idx01 = index + 1;
                int idx02 = index + 2;
                int idx03 = index + 3;

                if ((idx01 < 0 || idx02 < 0 || idx03 < 0) || (idx01 > len || idx02 > len || idx03 > len))
                    return false;

                Structs.BasicStructs.Point actual = GCPS.chain.r[index];
                Structs.BasicStructs.Point actualMoreOne = GCPS.chain.r[idx01];
                Structs.BasicStructs.Point actualMoreTwo = GCPS.chain.r[idx02];
                Structs.BasicStructs.Point actualMoreThree = GCPS.chain.r[idx03];

                bool testOne = (Maths4Simulation.DistanceBetweenPoints(actual, actualMoreTwo) == Consts.valueTwo);
                bool testTwo = (Maths4Simulation.DistanceBetweenPoints(actualMoreOne, actualMoreThree) == Consts.valueTwo);
                bool testThree = (Maths4Simulation.DistanceBetweenPoints(actual, actualMoreThree) == Consts.valueOne);

                return (testOne && testTwo && testThree);

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }

        /// <summary>
        /// Verifica se o CrankShaft ocorre ao centro a esquerda
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsCrankShaftLeftCenter(int index)
        {
            try
            {
                int len = (GCPS.chain.r.Count) - 1;
                int idx01 = index - 1;
                int idx02 = index + 1;
                int idx03 = index + 2;

                if ((idx01 < 0 || idx02 < 0 || idx03 < 0) || (idx01 > len || idx02 > len || idx03 > len))
                    return false;

                Structs.BasicStructs.Point actualMinusOne = GCPS.chain.r[idx01];
                Structs.BasicStructs.Point actual = GCPS.chain.r[index];
                Structs.BasicStructs.Point actualMoreOne = GCPS.chain.r[idx02];
                Structs.BasicStructs.Point actualMoreTwo = GCPS.chain.r[idx03];

                bool testOne = (Maths4Simulation.DistanceBetweenPoints(actual, actualMoreTwo) == Consts.valueTwo);
                bool testTwo = (Maths4Simulation.DistanceBetweenPoints(actualMinusOne, actualMoreOne) == Consts.valueTwo);
                bool testThree = (Maths4Simulation.DistanceBetweenPoints(actualMinusOne, actualMoreTwo) == Consts.valueOne);

                return (testOne && testTwo && testThree);

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }

        /// <summary>
        /// Verifica se o CrankShaft ocorre ao centro à direita
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsCrankShaftRightCenter(int index)
        {
            try
            {
                int len = (GCPS.chain.r.Count) - 1;
                int idx01 = index - 2;
                int idx02 = index - 1;
                int idx03 = index + 1;

                if ((idx01 < 0 || idx02 < 0 || idx03 < 0) || (idx01 > len || idx02 > len || idx03 > len))
                    return false;

                Structs.BasicStructs.Point actualMinusTwo = GCPS.chain.r[idx01];
                Structs.BasicStructs.Point actualMinusOne = GCPS.chain.r[idx02];
                Structs.BasicStructs.Point actual = GCPS.chain.r[index];
                Structs.BasicStructs.Point actualMoreOne = GCPS.chain.r[idx03];

                bool testOne = (Maths4Simulation.DistanceBetweenPoints(actual, actualMinusTwo) == Consts.valueTwo);
                bool testTwo = (Maths4Simulation.DistanceBetweenPoints(actualMinusOne, actualMoreOne) == Consts.valueTwo);
                bool testThree = (Maths4Simulation.DistanceBetweenPoints(actualMinusTwo, actualMoreOne) == Consts.valueOne);

                return (testOne && testTwo && testThree);

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }

        /// <summary>
        /// Verifica se o CrankShaft ocorre a direita
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsCrankShaftRight(int index)
        {
            try
            {
                int len = (GCPS.chain.r.Count) - 1;
                int idx01 = index - 3;
                int idx02 = index - 2;
                int idx03 = index - 1;

                if ((idx01 < 0 || idx02 < 0 || idx03 < 0) || (idx01 > len || idx02 > len || idx03 > len))
                    return false;

                Structs.BasicStructs.Point actualMoreThree = GCPS.chain.r[idx01];
                Structs.BasicStructs.Point actualMinusTwo = GCPS.chain.r[idx02];
                Structs.BasicStructs.Point actualMinusOne = GCPS.chain.r[idx03];
                Structs.BasicStructs.Point actual = GCPS.chain.r[index];


                bool testOne = (Maths4Simulation.DistanceBetweenPoints(actual, actualMinusTwo) == Consts.valueTwo);
                bool testTwo = (Maths4Simulation.DistanceBetweenPoints(actualMinusOne, actualMoreThree) == Consts.valueTwo);
                bool testThree = (Maths4Simulation.DistanceBetweenPoints(actual, actualMoreThree) == Consts.valueOne);

                return (testOne && testTwo && testThree);

            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }
    }
}


///// <summary>
//       /// Verifica se existe movimento de CrankShaft
//       /// </summary>
//       /// <param name="index">Monomero atual</param>
//       /// <param name="right">Se movimento existe da esquerda pra direita</param>
//       /// <param name="left">Se movimento existe da direita pra esquerda</param>
//       /// <returns>True de existir</returns>
//       protected static bool IsCrankShaft(int index, ref bool right, ref bool left)
//       {
//           right = IsCrankShaftToRight(index);
//           left = IsCrankShaftToLeft(index);
//           return (right || left);
//       }

//       /// <summary>
//       /// Testa o Movimento de CrankShaft sentido à direita (inicio para fim da cadeia)
//       /// </summary>
//       /// <param name="index">Posição atual no monomero</param>
//       /// <returns>True se existir</returns>
//       private static bool IsCrankShaftToRight(int index)
//       {
//           //bool posMinusTwo = IsCrankShaftPos(index - 2, index);
//           bool posMinusOne = IsCrankShaftPos(index - 1, index + 1);
//           bool posActual = IsCrankShaftPos(index, index + 2);
//           bool posMoreOne = IsCrankShaftPos(index + 1, index + 3);

//           bool ret = false;
//           try
//           {
//               //Para garantir eu pego o ponto anterior e ponto atual + 2 e verifica se a distancia entre ele é igual a 1.                
//               ret = (Maths4Simulation.Maths4Simulation.DistanceBetweenPoints(Members.monomero.r[index - 1], Members.monomero.r[index + 2]) == Consts.valueOne);
//           }
//           catch (System.ArgumentOutOfRangeException ex)
//           {
//               OutPut.WriteException(ex, Types.ErrorLevel.Warning);
//               return false;
//           }

//           return (ret && posMinusOne && posActual && posMoreOne);
//       }

//       /// <summary>
//       /// Testa o Movimento de CrankShaft sentido à esquerda (fim para inicio da cadeia)
//       /// </summary>
//       /// <param name="index">Posição atual no monomero</param>
//       /// <returns>True se existir</returns>
//       private static bool IsCrankShaftToLeft(int index)
//       {
//           bool posMinusThree = IsCrankShaftPos(index - 3, index - 1);
//           bool posMinusTwo = IsCrankShaftPos(index - 2, index);
//           bool posMinusOne = IsCrankShaftPos(index - 1, index + 1);
//           //bool posActual = IsCrankShaftPos(index, index + 2);

//           bool ret = false;
//           try
//           {
//               //Para garantir eu pego o ponto anterior e ponto atual - 2 e verifica se a distancia entre ele é igual a 1.                
//               ret = (Maths4Simulation.Maths4Simulation.DistanceBetweenPoints(Members.monomero.r[index + 1], Members.monomero.r[index - 2]) == Consts.valueOne);
//           }
//           catch (System.ArgumentOutOfRangeException ex)
//           {
//               OutPut.WriteException(ex, Types.ErrorLevel.Warning);
//               return false;
//           }

//           return (ret && posMinusThree && posMinusTwo && posMinusOne);
//       }

//       /// <summary>
//       /// Efetua o teste de KINK existente entre pois pontos do CrankShaft
//       /// </summary>
//       /// <param name="indexFirst">Monomero inicial</param>
//       /// <param name="indexSecond">Monomero final</param>
//       /// <returns></returns>
//       private static bool IsCrankShaftPos(int indexFirst, int indexSecond)
//       {
//           try
//           {
//               Structs.Point posteriorFirst = Members.monomero.r[indexFirst];
//               Structs.Point posteriorSecond = Members.monomero.r[indexSecond];

//               if (Kink.ExistKick(posteriorFirst, posteriorSecond))
//                   return true;
//               else
//                   return false;
//           }
//           catch (System.ArgumentOutOfRangeException ex)
//           {
//               OutPut.WriteException(ex, Types.ErrorLevel.Warning);
//               return false;
//           }
//       }