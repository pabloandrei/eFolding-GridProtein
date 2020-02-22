using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.IOHelpers;

namespace GridProteinFolding.Core.eFolding
{
    /// <summary>
    /// Class Simulation
    /// </summary>
    public class Simulation : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Representação do MONOMERO em OBJETO
        /// </summary>
        public static Structs.BasicStructs.Chain chain;

        public static byte modelType;
        //public static List<Structs.BasicStructs.Model> model;

        /// <summary>
        /// Representação do MONOMERO em OBJETO o qual ainda não foi persistido. (pode ser descartado após movimento)
        /// </summary>
        public static Structs.BasicStructs.TemporaryChain tempChain;

        /// <summary>
        /// Interador de MonteCarlos
        /// </summary>
        private static int mcSteps = 0;

        public static int McSteps
        {
            get { return mcSteps; }
        }

        public static void McStepsDo()
        {
            mcSteps++;
        }

        public static void McStepsReset()
        {
            mcSteps = 0;
        }

        public static int mcStepsLoop02_LIMITE = 5;
        public static int mcStepsLoop01 = 0;
        public static int mcStepsLoop02 = 0;
        public static int mcStepsLoop03 = 0;

        /// <summary>
        ///  Matriz energética
        /// </summary>
        public static double[,] energeticMatrix;


        /// <summary>
        /// Limpa a lista de Monomeros para a próxima ISEM
        /// </summary>
        public static void Reset()
        {
            chain.r.Clear();
        }

        /// <summary>
        /// Isem inicial da Cadeia o qual será utilizada pelo gerador Randomico
        /// </summary>
        public static long initialIsem = long.MinValue;

        /// <summary>
        /// Caminha inteira da onde a aplicação esta sendo executada
        /// </summary>
        public string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

    }
}
