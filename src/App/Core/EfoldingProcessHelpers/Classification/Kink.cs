using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GridProteinFolding.ProcessHelpers.MathsHelpers;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Core.eFolding.Classification
{
    /// <summary>
    /// Class Kink
    /// </summary>
    public class Kink : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Verifica se existe movimento de Kink
        /// </summary>
        /// <param name="index">Monomero atual</param>
        /// <returns>True de existir</returns>
        public static bool IsKink(int index)
        {
            try
            {
                int len = (GCPS.chain.r.Count) - 1;
                int idx01 = index - 1;
                int idx02 = index + 1;

                if ((idx01 < 0 || idx02 < 0) || (idx01 > len || idx02 > len))
                    return false;

                Structs.BasicStructs.Point previousPos = GCPS.chain.r[idx01];
                Structs.BasicStructs.Point actualPos = GCPS.chain.r[index];
                Structs.BasicStructs.Point posteriorPos = GCPS.chain.r[idx02];

                return Kink.ExistKick(previousPos, actualPos, posteriorPos);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }   

        /// <summary>
        /// Testa a existencia de um KINK apartir de uma posição do Monomero (usando produto escalar)
        /// </summary>
        /// <param name="previousPos">Referência o ponto anterior ao atual</param>
        /// <param name="actualPos">Referência o ponto atual</param>
        /// <param name="posteriorPos">Referência o ponto posterior ao atual</param>
        /// <returns>Retorno TRUE se o Kink existir.</returns>
        private static bool ExistKick(Structs.BasicStructs.Point previousPos, Structs.BasicStructs.Point actualPos, Structs.BasicStructs.Point posteriorPos)
        {
            return (ScalarProduct.Calc(previousPos, actualPos, posteriorPos) == 0);
        }

        /// <summary>
        /// Testa a existencia de um KINK apartir de uma posição do Monomero
        /// </summary>
        /// <param name="previousPos">Referência o ponto anterior ao atual</param>
        /// <param name="posteriorPos">Referência o ponto posterior ao atual</param>
        /// <returns>Retorno TRUE se o Kink existir.</returns>
        protected static bool ExistKick(Structs.BasicStructs.Point previousPos, Structs.BasicStructs.Point posteriorPos)
        {
            return (Maths4Simulation.DistanceBetweenPoints(previousPos, posteriorPos) == Consts.valueTwo);
        }
    }
}
