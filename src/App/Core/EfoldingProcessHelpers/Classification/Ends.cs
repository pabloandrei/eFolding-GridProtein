using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;

namespace GridProteinFolding.Core.eFolding.Classification
{
    /// <summary>
    /// Classe Ends
    /// </summary>
    public class Ends : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Verifica se existe movimento de Ends
        /// </summary>
        /// <param name="index">Monomero atual</param>
        /// <returns>True de existir</returns>
        public static bool IsEnds(int index)
        {
            return (index == 0 || index == (GCPS.chain.r.Count-1));
        }      
    }
}
