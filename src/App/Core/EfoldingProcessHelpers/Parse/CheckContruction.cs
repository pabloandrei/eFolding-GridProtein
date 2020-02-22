using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using static GridProteinFolding.Core.eFolding.Structs.BasicStructs;

namespace GridProteinFolding.Core.eFolding.Parse
{
    /// <summary>
    /// Classe o responsável pela checagem das politicas de construção da cadeia de monomeros
    /// </summary>
    internal class CheckContruction : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }


        internal bool Go()
        {
            return (PolicyOne() && PolicyTwo() && PolicyThree());
        }


        /// <summary>
        /// 1# regra
        /// Equação: 1 MENOR OU IGUAL d^2 i,j MENOR OU IGUAL (N-1)^2 
        /// </summary>
        /// <returns></returns>
        private bool PolicyOne() {

            return (Maths4Simulation.CalcSpinningRay() <= Math.Pow(GCPS.chain.r.Count - 1, 2));

        }        
        
        /// <summary>
        /// 2# regra
        /// Equação: d^2 i, i+1 = 1 
        /// A DISTÂNCIA ENTRE O MONÔMERO "i"  E O MONÔMERO "i+1" , ELEVADO AO QUADRADO, SÓ PODE SER IGUAL A 1. 
        /// QUALQUER OUTRO VALOR RESULTANTE  É INDICATIVO DE ERRO. 
        /// </summary>
        /// <returns></returns>
        private bool PolicyTwo()
        {
            for (int i = 0; i < GCPS.chain.r.Count - 1; i++)
            {
                if (!(Maths4Simulation.DistanceBetweenPoints(((Structs.BasicStructs.Point)GCPS.chain.r[i]),
                    ((Structs.BasicStructs.Point)GCPS.chain.r[i + 1])) == Consts.valueOne))
                    return false;
            }

            return true;
        }

        
        /// <summary>
        /// 3# regra
        /// Equação: d^2 i, [j+2 ->  i+2 ] = 4||2 
        /// A DISTÂNCIA ENTRE O MONÔMERO "i"  E O MONÔMERO "i+2" , ELEVADO AO QUADRADO, 
        /// SÓ PODE SER IGUAL A 2, OU IGUAL A 4. QUALQUER OUTRO VALOR É UM INDICATIVO DE  ERRO.
        /// </summary>
        /// <returns></returns>
        private bool PolicyThree()
        {
            for (int i = 0; i < GCPS.chain.r.Count - 2; i++)
            {
                double tempCalc = Maths4Simulation.DistanceBetweenPointsWithSqrt(((Structs.BasicStructs.Point)GCPS.chain.r[i]), ((Structs.BasicStructs.Point)GCPS.chain.r[i + 2]));
                if (!((tempCalc == Consts.valueSqrtTwo) || (tempCalc == Consts.valueSqrtFour)))
                    return false;                
            }

            return true;
        }

        ///// <summary>
        ///// Funcao que check se existe sobreposição do ponto temporário com o vetor "r"
        ///// </summary>
        ///// <param name="tempCoord">Ponto temporário</param>
        ///// <returns>True se ocorrer</returns>
        //public static bool CheckOverRide(Structs.Point tempCoord)
        //{
        //    foreach (Structs.Point temp in GCPS.chain.r)
        //    {
        //        if (temp.x==tempCoord.x && temp.y==tempCoord.y && temp.z==tempCoord.z)
        //            return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Funcao que check se existe sobreposição entre todos os pontos com o vetor "r"
        /// </summary>
        /// <returns>True se ocorrer</returns>
        public static bool CheckOverRide()
        {
            for (int i = 0; i < (GCPS.chain.r.Count - 1); i++)
            {
                for (int k = (i + 2); k < (GCPS.chain.r.Count); k++)
                {
                    if (GCPS.chain.r[i].Equals(GCPS.chain.r[k]))
                        return true;
                }
            }
            return false;
        }


        public static bool CheckOverRidWithConstains()
        {
            int lenghtArray = GCPS.chain.r.Count;

            for (int i = 0; i < (lenghtArray - 2); i++)
            {
                Structs.BasicStructs.Point[] temp01 = new Structs.BasicStructs.Point[lenghtArray - i];
                GCPS.chain.r.CopyTo(i, temp01, 0, (lenghtArray - i));

                Structs.BasicStructs.Point[] temp02 = new Structs.BasicStructs.Point[lenghtArray - i - 1];
                GCPS.chain.r.CopyTo((i + 1), temp02, 0, (lenghtArray - i - 1));

                if (temp01.Any(temp02.Contains))
                    return true;

            }
            return false;
        }

        /// <summary>
        /// Funcao que check a existencia de primeiro vizinho
        /// </summary>
        /// <param name="tempCoord">Ponto temporário</param>
        /// <returns>True se ocorrer</returns>
        public static bool FirstNeighbor(Structs.BasicStructs.Point tempCoord)
        {
            for (int i = 0; i < GCPS.chain.r.Count - 2; i++)
            {
                if (Maths4Simulation.DistanceBetweenPoints(GCPS.chain.r[i], tempCoord) == Consts.valueOne)
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Funcao que check a existencia de primeiro vizinho em toda a cadeia (ponto por ponto)
        /// </summary>
        /// <returns>True se ocorrer</returns>
        public static bool FirstNeighbor()
        {
            for (int i = 0; i < (GCPS.chain.r.Count - 2); i++)
            {
                for (int k = (i + 2); k < (GCPS.chain.r.Count - 1); k++)
                {
                    if (Maths4Simulation.DistanceBetweenPoints(GCPS.chain.r[i], GCPS.chain.r[k]) == Consts.valueOne)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Verificação da geração de DeadEnd
        /// </summary>
        /// <param name="tempCoord">Ponto temporário</param>
        /// <param name="lastValed">Último monomero válido</param>
        /// <returns>True se existir</returns>
        public static bool GenCountDeadEndPoints(Point tempCoord, int lastValed)
        {
            Structs.BasicStructs.Point changeValue = GCPS.chain.r[lastValed];
            changeValue.deadEnd++;
            changeValue.deadEndPoints = tempCoord.x.ToString() + "," + tempCoord.y.ToString() + "," + tempCoord.z.ToString();
            GCPS.chain.r[lastValed] = changeValue;

            return (GCPS.chain.r[lastValed].deadEnd == Consts.deadEnd);
        }

    }
}
