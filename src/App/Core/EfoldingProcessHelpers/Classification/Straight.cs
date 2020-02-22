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
    /// Class Straight
    /// </summary>
    public class Straight : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Metodo retorno se posição monomero é um Straight
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsStraight(int index)
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

                return ExistStraight(previousPos, posteriorPos);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ArgumentOutOfRangeException(ex, Types.ErrorLevel.Warning, true);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posInit">Referência o ponto anterior ao atual</param>
        /// <param name="posEnd">Referência o ponto posterior ao atual</param>
        /// <returns>Retorno TRUE se o Kink existir.</returns>
        protected static bool ExistStraight(Structs.BasicStructs.Point previousPos, Structs.BasicStructs.Point posteriorPos)
        {
            return (Maths4Simulation.DistanceBetweenPoints(previousPos, posteriorPos) == Consts.valueFour);
        }
    }
}
