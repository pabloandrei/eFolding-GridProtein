using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding
{
  
    /// <summary>
    /// Classe responsável pelas Constantes da Aplicação
    /// </summary>
    public class Consts : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Posição do ponto inicial
        /// </summary>
        public const int initialPosition = 50;

        /// <summary>
        /// 
        /// </summary>
        public const double rXYZ = 0.3333333;
        
        /// <summary>
        /// Valor constante da distancia entre pontos
        /// </summary>
        public const double valueOne = 1.0;

        /// <summary>
        /// Número máximo de tentativas para DEAD END
        /// </summary>
        public const int deadEnd = 60;

        /// <summary>
        /// Valor constante da distancia entre dois pontos quando igual 2
        /// </summary>
        public const int valueTwo = 2;

        /// <summary>
        /// Valor constante da distancia entre dois pontos quando igual 4
        /// </summary>
        public const int valueFour = 4;
 
        /// <summary>
        /// Constante que representa a TABULAÇÃO para arquivos textos
        /// </summary>
        public const string tab = @"\t";

        /// <summary>
        /// Constante que representa o nome da extensão dos arquivos gerados
        /// </summary>
        public const string extensionOfFile = ".dat";

        /// <summary>
        /// Valor constante da distancia para calculo entre pontos no Mapa de Contatyo
        /// </summary>
        internal const double mapContact = 1.0;

        /// <summary>
        /// Valor constante da distancia entre dois pontos
        /// </summary>
        internal const double valueSqrtTwo = 1.4142135623730951;

        /// <summary>
        /// Valor constante da distancia entre três pontos
        /// </summary>
        internal const double valueSqrtThree = 1.73205081;

        /// <summary>
        /// Valor constante da distancia entre quatro pontos
        /// </summary>
        internal const double valueSqrtFour = 2;

    }
}
