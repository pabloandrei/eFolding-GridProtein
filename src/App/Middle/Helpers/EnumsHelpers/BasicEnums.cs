using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

[assembly: CLSCompliant(true)]
//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.EnumsHelpers
{
    /// <summary>
    /// Classe ENUM
    /// </summary>
    public class BasicEnums
    {
        /// <summary>
        /// Tipos de MODELOS possiveis
        /// Qdo IDEAL, ocorre sobreposição
        /// Qdo REAL, não ocorre sobreposição, entretanto existe vizinhos de contatos topologico de primeiros vizinhos
        /// Qdo SOFT, não ocorre sobreposição e não existe contatos topologicos de primeiros vizinhos
        /// Qdo ERROR, não corresponde a nenhuma das anteriores
        /// </summary>
        public enum ChainTypes { Error = -1, Ideal = 0, Real = 1, Soft = 2 };

        /// <summary>
        /// Tipos de calculos possveis
        /// </summary>
        public enum HistTypes { RadiuosOfGyration = 0, EndToEndDistance = 1 };

        ///// <summary>
        ///// Tipos monomeros os quais passaram nos testes de CHECK dentro da classe PROTEIN
        ///// </summary>
        //public enum CheckSeed { Ideal = 1, Real = 2, Soft = 3 };

        /// <summary>
        /// Estado do processomento de uma simulação
        /// </summary>
        public enum State
        {
            Created = 0, Waiting = 1, Processing = 2, Processed = 3, Upload = 4, BULK = 5,
            ClearTempClient = 6, ClearTempServer = 7, Finalized = 8, ResultsProcessed = 9, ETLError = 96,
            NoChangeStatus = 97, Declined = 98, Error = 99
        }

        /// <summary>
        /// Estado para notificacao de email
        /// </summary>
        public enum EmailNotification { NaoEnviar = 0, Enviar = 1 }

        public enum Lattices { None = -1, Crank = 0, Kink = 1, End = 2 }
    }
}
