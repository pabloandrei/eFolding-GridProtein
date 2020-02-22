using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Modelo para tipo de energia
    /// </summary>
    public class TypeE
    {
        /// <summary>
        /// Posição node movimentado
        /// </summary>
        public int selectNode;
        /// <summary>
        /// Posição i (sendo i vizinho do contato)
        /// </summary>
        public int i;
        /// <summary>
        /// Valor energia
        /// </summary>
        public double e;
    }

}
