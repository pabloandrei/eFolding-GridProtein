using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using Randomizer = GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Core.eFolding.MathsHelpers;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Medio ou Ambiente (Representa)
    /// </summary>
    public static class Environment
    {
        /// <summary>
        /// Constante de  Boltzmann
        /// </summary>
        public static double k = 1;

        /// <summary>
        /// Valor do último R válido
        /// </summary>
        public static double lastR = 0;

        /// <summary>
        /// Valor do último randomico gerado para o ambiente
        /// </summary>
        private static void LastR()
        {
            lastR = RandomR();
        }

        /// <summary>
        /// Valor do último calculo de DeltaE
        /// </summary>
        public static double deltaE = 0;

        //Calcula o valor atual de DelatE
        private static void DeltaE()
        {
            deltaE = GCPS.tempChain.U - GCPS.chain.U;
        }


        /// <summary>
        /// Beta representa (1 / (k * t);)
        /// </summary>
        private static double beta = 1;

        /// <summary>
        /// valor do cálculo  do último TransitionProbability
        /// </summary>
        public static double transitionProbability = 0;

        /// <summary>
        /// valor do cálculo  e^-deltaE*beta
        /// </summary>
        private static void TransitionProbability()
        {
            if (deltaE > 0)
            {
                transitionProbability = Math.Exp((-1 * deltaE) * beta);

            }
            else
            {
                transitionProbability = 1;
            }
        }


        /// <summary>
        /// Sorteia o randomico para R
        /// </summary>
        /// <returns>Retorna a posição do Monomero</returns>
        private static double RandomR()
        {

            Randomic.Random();

            return (Randomizer.Randomic.randu * 1);

        }

        /// <summary>
        /// Valor do último calculo para RG
        /// </summary>
        public static double rg = 0;

        /// <summary>
        /// Valor do último calculo para DPP
        /// </summary>
        public static double dpp = 0;

        /// <summary>
        /// RG: Calcula do raio de giração (evolução)
        /// </summary>
        private static void RG()
        {
            rg = Maths4Simulation.CalcSpinningRay();
        }

        /// <summary>
        /// RG: Calcula do raio de giração (evolução)
        /// </summary>
        private static void DPP()
        {
            dpp = Maths4Simulation.CalDistanceBetweenLastPointFirst();
        }

        /// <summary>
        /// Metodo que efetua calculo do ambiente: DeltaE, LastR, TransitionProbability e RG
        /// </summary>
        public static void CalculateEnvironment()
        {
            DeltaE();
            LastR();
            TransitionProbability();
            RG();
            DPP();
        }

    }
}
