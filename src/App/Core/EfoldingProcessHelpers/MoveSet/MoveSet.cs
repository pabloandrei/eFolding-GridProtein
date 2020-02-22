using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding;

namespace GridProteinFolding.Core.eFolding.MoveSet
{

    /// <summary>
    /// Classe o qual promove movimentação dos MONOMEROS dentro da cadeia.
    /// </summary>
    internal class MoveSet : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Construtor Default
        /// </summary>
        public MoveSet()
        {

        }

        /// <summary>
        /// Destrutor Default
        /// </summary>
        ~MoveSet()
        {

        }

 
        /// <summary>
        /// Movimento de Ends
        /// </summary>
        /// <returns>Return TRUE se o movimento ocorreu</returns>
        internal bool Ends()
        {

            return new MoveEnds().Do();
        }

        /// <summary>
        /// Movimento de Kink
        /// </summary>
        /// <returns>Return TRUE se o movimento ocorreu</returns>
        internal bool Kink()
        {
            return new MoveKink().Do();
        }

        /// <summary>
        /// Movimento de Crankshaft
        /// </summary>
        /// <returns>Return TRUE se o movimento ocorreu</returns>
        internal bool Crankshaft()
        {
            return new MoveCrankshaft().Do();
        }
                
    }

}
