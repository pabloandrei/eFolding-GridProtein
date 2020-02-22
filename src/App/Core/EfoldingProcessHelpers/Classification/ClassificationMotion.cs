using System;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Core.eFolding.Classification
{
    /// <summary>
    /// Classe responsável pela classificação do Monomero quanto ao tipo de movimento possível
    /// </summary>
    public class ClassificationMotion : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Classifica o Monomero por Monomero, incluindo E se Ends, K se Kink e C se CrankShaft na propriedade classificationMotion do Point
        /// </summary>
        public static void PreClassificationOfMotion()
        {
            for (int i = 0; i < GCPS.chain.r.Count; i++)
            {
                Structs.BasicStructs.Point actualPoint = GCPS.chain.r[i];

                //Verifica se é END
                actualPoint.classificationMotion.ends = Ends.IsEnds(i);

                //Verifica se é KINK
                actualPoint.classificationMotion.kink = Kink.IsKink(i);


                //Verifica se é CRANKSHAFT
                actualPoint.classificationMotion.crankShaft__R0 = CrankShaft.IsCrankShaftLeft(i);
                actualPoint.classificationMotion.crankShaft__R1 = CrankShaft.IsCrankShaftLeftCenter(i);
                actualPoint.classificationMotion.crankShaft__R2 = CrankShaft.IsCrankShaftRightCenter(i);
                actualPoint.classificationMotion.crankShaft__R3 = CrankShaft.IsCrankShaftRight(i);

                //stretched
                actualPoint.classificationMotion.stretched = Straight.IsStraight(i);

                GCPS.chain.r[i] = actualPoint;

            }
        }

        /// <summary>
        /// Save a classificação do tipo de Movimento possível
        /// </summary>
        public static void SaveClassificationOfMotion(string file)
        {
            string dir = IO.Directory.SubDirClassificationOfMotion;
            string pathFile = dir + @"\" + file + Consts.extensionOfFile;

            ExtendedStreamWriter sw = new ExtendedStreamWriter(Config.Crypt);
            sw.CreateText(pathFile, Config.CurrentGuid);

            for (int i = 0; i < GCPS.chain.r.Count; i++)
            {
                Structs.BasicStructs.Point temp = GCPS.chain.r[i];
                sw.Write("{0}\t", i);

                sw.Write("{0}", temp.classificationMotion.ends == true ? "E " : string.Empty);
                sw.Write("{0}", temp.classificationMotion.stretched == true ? "S " : string.Empty);
                sw.Write("{0}", temp.classificationMotion.kink == true ? "T " : string.Empty);

                sw.Write("{0}", temp.classificationMotion.crankShaft__R0 == true ? "CR0 " : string.Empty);
                sw.Write("{0}", temp.classificationMotion.crankShaft__R1 == true ? "CR1 " : string.Empty);
                sw.Write("{0}", temp.classificationMotion.crankShaft__R2 == true ? "CR2 " : string.Empty);
                sw.Write("{0}", temp.classificationMotion.crankShaft__R3 == true ? "CR3 " : string.Empty);

                sw.WriteLine();

            }

            sw.Flush();
            sw.Close();

        }
    }
}
