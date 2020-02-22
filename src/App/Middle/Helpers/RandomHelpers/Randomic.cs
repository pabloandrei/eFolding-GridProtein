using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Middle.Helpers.RandomHelpers
{
    /// <summary>
    /// Classe responsável pelo gerador Randomico.
    /// </summary>
    public static class Randomic
    {

        /// <summary>
        /// Representa o valor do Randum atual.
        /// Ela esta protecteda e estatica para ser compartilhada por todas as rotinas e metodos do projeto
        /// </summary>
        public static double randu = 0;
        public static int magicNumber = 0; //453816693;
        public static double equal_one = 0;// 0.9999999996;

        /// <summary>
        /// Gerador randomico baseado em 32 bits
        /// </summary>
        public static void Random()
        {
            if (magicNumber == 0)
                throw new Exception("magicNumber deve ser informado!");

            const int MAX32 = 1103515245;
            const int MIN32 = -2147483648;
            

            AppConfigClient.Param.dataToProcess.isem = AppConfigClient.Param.dataToProcess.isem * MAX32 + magicNumber;
            if (AppConfigClient.Param.dataToProcess.isem == MIN32) randu = equal_one;
            randu = (double)(AppConfigClient.Param.dataToProcess.isem) / MIN32;
            if (randu < 0) randu = (randu * (-1));

            return;
        }
    }
}
