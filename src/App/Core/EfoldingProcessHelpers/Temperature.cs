//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GridProteinFolding.Middle.Helpers.RandomHelpers;
//using GridProteinFolding.Core.eFolding.Maths;
//using GCPS = GridProteinFolding.Core.eFolding.Simulation;
//using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

//namespace GridProteinFolding.Core.eFolding
//{
//    public class Temperature
//    {
//        public static bool Do()
//        {
//            //CITAÇÃO: http://scitation.aip.org/docserver/fulltext/aip/journal/jcp/21/6/1.1699114.pdf?expires=1448642841&id=id&accname=2113586&checksum=5FEB92448028A12FFFA3BB5510E40243
//            //PAGE: 4
//            //Introduçaõ a "Temperatura"
//            //
//            // Premissa: Configurações fisicamente aceitas
//            //
//            // ALGORITMO:
//            //
//            // variável deltaE = En - Ea;  //En: Energia da nova configuração (ainda não aceita), Ea: Energia da configuração anterior ao movimento
//            // Se deltaE <= 0 Então ACEITA;
//            // Senão deltaE>0 Então
//            // INICIO
//            //    Gerar "R" (# randomico  0<=E<1)
//            //    SE R<=EXP(-deltaE/k) Então ACEITA; //(k=Boltzmann)
//            //    SE R<=EXP(-deltaE/k) Então REJEITA;
//            // FIM
//            //
//            // Observações: {E} -> -1,0,1; {T} -> T~=1; Exemplo: 0,9; 1,2 e etc

//            double kt = Convert.ToDouble(Structs.Environment.k * AppConfigClient.Param.dataToProcess.temperature);
//            double En = GCPS.tempChain.U;
//            double Ea = GCPS.chain.U;
//            double deltaE = En - Ea;

//            //Nova configuração já esta aceita por definição, valor energético da nova configuração é melhor que o anterior
//            if (deltaE <= 0)
//            {
//                return true;
//            }
//            else if (deltaE > 0) //Senão teremos peso estatístico para a aceitação
//            {
//                Randomic.Random();
//                double r = Randomic.randu;

//                if (r <= MathExtesion.Exp(-deltaE / kt))
//                    return true;
//                else
//                    return false;
//            }

//            return false;
//        }
//    }
//}
