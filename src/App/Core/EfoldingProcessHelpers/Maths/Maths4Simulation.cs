using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using System.Threading.Tasks;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Core.eFolding.IO;
using System.Security;

namespace GridProteinFolding.Core.eFolding.MathsHelpers
{
    /// <summary>
    /// Classe o qual promove calcula matemáticos vinculados ao Folding de Proteina.
    /// </summary>
    public class Maths4Simulation : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        internal static double rgMax = double.MinValue;
        internal static double rgMin = double.MinValue;

        /// <summary> 
        /// Retorna a distância entre dois pontos com Raiz Quadrada
        /// </summary> 
        /// <param name="p1">Ponto 1</param> 
        /// <param name="p2">Ponto 2</param> 
        /// <returns>Valor double do cálculo</returns> 
        public static double DistanceBetweenPointsWithSqrt(Structs.BasicStructs.Point p1, Structs.BasicStructs.Point p2)
        {
            //a distancia e igual a raiz quadrada da soma dos quadrados dos catetos formados pelos pontos 
            return Math.Sqrt(Math.Pow((p2.x - p1.x), 2) + Math.Pow((p2.y - p1.y), 2) + Math.Pow((p2.z - p1.z), 2));
        }

        /// <summary> 
        /// Retorna a distância entre dois pontos 
        /// </summary> 
        /// <param name="p1">Ponto 1</param> 
        /// <param name="p2">Ponto 2</param> 
        /// <returns>Valor double do cálculo</returns> 
        public static double DistanceBetweenPoints(Structs.BasicStructs.Point p1, Structs.BasicStructs.Point p2)
        {
            //a distancia e igual a raiz quadrada da soma dos quadrados dos catetos formados pelos pontos 
            return (Math.Pow((p2.x - p1.x), 2) + Math.Pow((p2.y - p1.y), 2) + Math.Pow((p2.z - p1.z), 2));
        }

        /// <summary>
        /// Efetua o Calculo de Raio de Giração
        /// </summary>
        /// <returns>Retorno o valor do cálculo</returns>
        public static double CalcSpinningRay()
        {
            //Cálculo do raio de Giração
            double rg = 0;
            int x1, x2, y1, y2, z1, z2 = 0;

            for (int i = 0; i < (GCPS.chain.r.Count - 2); i++)
            {
                x1 = GCPS.chain.r[i].x;
                y1 = GCPS.chain.r[i].y;
                z1 = GCPS.chain.r[i].z;

                for (int j = (i + 1); j < (GCPS.chain.r.Count - 1); j++)
                {

                    x2 = GCPS.chain.r[j].x;
                    y2 = GCPS.chain.r[j].y;
                    z2 = GCPS.chain.r[j].z;

                    rg = rg + Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2) + Math.Pow((z2 - z1), 2);
                }

            }
            return (rg / (Math.Pow(GCPS.chain.r.Count, 2)));

        }

        /// <summary>
        /// Calcula a distancia entre o primeiro e o ultimo ponto da cadeia
        /// </summary>
        /// <returns></returns>
        public static double CalDistanceBetweenLastPointFirst()
        {
            double dpp2, dpp = 0;

            int init = 0;
            int last = GCPS.chain.r.Count - 1;

            int x1, xN, y1, yN, z1, zN = 0;

            x1 = GCPS.chain.r[init].x;
            y1 = GCPS.chain.r[init].y;
            z1 = GCPS.chain.r[init].z;

            xN = GCPS.chain.r[last].x;
            yN = GCPS.chain.r[last].y;
            zN = GCPS.chain.r[last].z;

            dpp2 = Math.Pow((xN - x1), 2) + Math.Pow((yN - y1), 2) + Math.Pow((zN - z1), 2);
            dpp = Math.Pow(dpp2, 0.5);

            return dpp;
        }

        public static StringBuilder FrequencyPeerInterval(string fileName, BasicEnums.HistTypes histType, BasicEnums.ChainTypes type)
        {
            double[] arr = null;

            if (type == BasicEnums.ChainTypes.Ideal)
            {
                //IDEAL
                if (histType == BasicEnums.HistTypes.RadiuosOfGyration)
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Ideal, BasicEnums.HistTypes.RadiuosOfGyration);
                else
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Ideal, BasicEnums.HistTypes.EndToEndDistance);
            }
            else if (type == BasicEnums.ChainTypes.Real)
            {
                //REAL
                if (histType == BasicEnums.HistTypes.RadiuosOfGyration)
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Real, BasicEnums.HistTypes.RadiuosOfGyration);
                else
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Real, BasicEnums.HistTypes.EndToEndDistance);
            }
            else if (type == BasicEnums.ChainTypes.Soft)
            {
                //SOFT
                if (histType == BasicEnums.HistTypes.RadiuosOfGyration)
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Soft, BasicEnums.HistTypes.RadiuosOfGyration);
                else
                    arr = CalcOfFrequency.LoadData(BasicEnums.ChainTypes.Soft, BasicEnums.HistTypes.EndToEndDistance);
            }


            StringBuilder sbHist;

            if (arr != null)
            {
                if (arr.Length > 0)
                {
                    double min;
                    double max;


                    int lengthArr = arr.Length;

                    arr = SorteDouble(arr);
                    min = arr[0];
                    max = arr[lengthArr - 1];

                    //Calcula valor de DELTA
                    AppConfigClient.Param.dataToProcess.valueOfDelta = CalcBIN(arr, lengthArr, min, max);

                    if (histType == BasicEnums.HistTypes.RadiuosOfGyration)
                    {
                        sbHist = histogramSpinningRayValues(fileName, type, min, max, arr);
                    }
                    else
                    {
                        sbHist = HistogramDistanceBetweenLastPointFirst(fileName, type, min, max, arr);
                    }
                }
                else
                    sbHist = null;
            }
            else
                sbHist = null;

            return sbHist;
        }



        private static double CalcBIN(double[] arr, int lengthArr, double min, double max)
        {
            double n;
            n = Math.Log10(lengthArr) * 3.32;

            return ((max - min) / n);
        }


        protected static double FrequencyAverageRG(double[] arrSpinning)
        {
            return arrSpinning.Average();
        }

        /// <summary>
        /// Sumariza o calculo de giração
        /// </summary>
        /// <param name="arrSpinning"></param>
        /// <returns></returns>
        protected static double SumRG(double[] arrSpinning)
        {
            return arrSpinning.Sum();
        }

        /// <summary>
        /// Efetua a contagem do Histograma para Raio de Giração
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static StringBuilder histogramSpinningRayValues(string fileName, BasicEnums.ChainTypes type, double min, double max, double[] arr)
        {
            double sum = SumRG(arr);
            double medium = FrequencyAverageRG(arr);

            StringBuilder sb = new StringBuilder();
            //sb.Append("Histograma\n");

            sb.Append("\t\t\t\t\tBIN = (max-min)/[3,32*LOG(#configs)]\n");
            sb.Append("#configs\tmin\tmax\tBIN\tsum RG\t<RG>\n");
            //sb.Append("lenght\tmin\tmax\tn\tsum RG\tmedium RG\n");
            sb.Append(arr.Length + "\t" + min + "\t" + max + "\t" + AppConfigClient.Param.dataToProcess.valueOfDelta + "\t" + sum + "\t" + medium + "\n");

            //sb.Append("lenght:  " + arr.Length + "\n");
            //sb.Append("min:     " + min + "\n");
            //sb.Append("max:     " + max + "\n");
            //sb.Append("n:   " + AppConfigClient.Process.numberOfDelta + "\n");            
            //sb.Append("sum RG:   " + sum + "\n");            
            //sb.Append("medium RG:   " + medium + "\n");

            //Montando as frequencias dos intervalos (# de BINS)
            List<double> freq = new List<double>();
            for (double i = min; i < max; i = i + AppConfigClient.Param.dataToProcess.valueOfDelta)
            {
                freq.Add(i);
                //GICO.WriteLine(i);
            }
            freq.Add(max);

            sb.Append("Frequencies\n");
            sb.Append("Initial\tFinal\tOccurrences\n");
            int lenght = freq.Count;
            double[] faixaInicial = new double[lenght - 1];
            double[] faixaFinal = new double[lenght - 1];
            double[] ocorrencias = new double[lenght - 1];

            List<int> coutFreq = new List<int>();
            for (int i = 0; i < (lenght - 1); i++)
            {
                int count = 0;
                double val_1 = Convert.ToDouble(freq[i]);
                double val_2 = Convert.ToDouble(freq[i + 1]);

                for (int k = 0; k < arr.Length; k++)
                {

                    if ((arr[k] >= val_1) && (arr[k] <= val_2))
                    {
                        count++;
                    }
                }
                faixaInicial[i] = val_1;
                faixaFinal[i] = val_2;
                ocorrencias[i] = count;
                sb.Append(val_1 + "\t" + val_2 + "\t" + count + "\n");

            }

            ////Gerar saida em EXCEL
            //Integration.ExcelIntegrator objExcel = new Integration.ExcelIntegrator();
            //objExcel.Execute(fileName+".xls", type, faixaInicial, faixaFinal, ocorrencias, arr.Length, min, max, 0, sum, medium);
            //objExcel = null;

            return sb;
        }

        /// <summary>
        /// Efetua a contagem do Histograma para Distancia entre primeiro e ultimo pontos
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static StringBuilder HistogramDistanceBetweenLastPointFirst(string fileName, BasicEnums.ChainTypes type, double min, double max, double[] arr)
        {
            double sum = SumRG(arr);
            double medium = FrequencyAverageRG(arr);

            StringBuilder sb = new StringBuilder();
            //sb.Append("Histograma\n");

            sb.Append("\t\t\t\t\tBIN = (max-min)/[3,32*LOG(#configs)]\n");
            sb.Append("#configs\tmin\tmax\tBIN\tsum DPP\t<DPP>\n");

            sb.Append(arr.Length + "\t" + min + "\t" + max + "\t" + AppConfigClient.Param.dataToProcess.valueOfDelta + "\t" + sum + "\t" + medium + "\n");

            //sb.Append("lenght:  " + arr.Length + "\n");
            //sb.Append("min:     " + min + "\n");
            //sb.Append("max:     " + max + "\n");
            //sb.Append("n:   " + AppConfigClient.Process.numberOfDelta + "\n");            
            //sb.Append("sum RG:   " + sum + "\n");            
            //sb.Append("medium RG:   " + medium + "\n");

            //Montando as frequencias dos intervalos (# de BINS)
            List<double> freq = new List<double>();
            for (double i = min; i < max; i = i + AppConfigClient.Param.dataToProcess.valueOfDelta)
            {
                freq.Add(i);
                //GICO.WriteLine(i);
            }
            freq.Add(max);

            sb.Append("Frequencies\n");
            sb.Append("Initial\tFinal\tOccurrences\n");
            int lenght = freq.Count;
            double[] faixaInicial = new double[lenght - 1];
            double[] faixaFinal = new double[lenght - 1];
            double[] ocorrencias = new double[lenght - 1];

            List<int> coutFreq = new List<int>();
            for (int i = 0; i < (lenght - 1); i++)
            {
                int count = 0;
                double val_1 = Convert.ToDouble(freq[i]);
                double val_2 = Convert.ToDouble(freq[i + 1]);


                ParallelLoopResult result = Parallel.For(0, arr.Length, k =>
                {
                    if ((arr[k] >= val_1) && (arr[k] <= val_2))
                    {
                        count++;
                    }

                });

                //for (int k = 0; k < arr.Length; k++)
                //{

                //    if ((arr[k] >= val_1) && (arr[k] <= val_2))
                //    {
                //        count++;
                //    }
                //}
                faixaInicial[i] = val_1;
                faixaFinal[i] = val_2;
                ocorrencias[i] = count;
                sb.Append(val_1 + "\t" + val_2 + "\t" + count + "\n");

            }

            ////Gerar saida em EXCEL
            //Integration.ExcelIntegrator objExcel = new Integration.ExcelIntegrator();
            //objExcel.Execute(fileName+".xls", type, faixaInicial, faixaFinal, ocorrencias, arr.Length, min, max, 0, sum, medium);
            //objExcel = null;

            return sb;
        }
        /// <summary>
        /// Ordena em ordem cescende a array de valores
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static double[] SorteDouble(double[] values)
        {
            Array.Sort(values);

            return values;
        }

    }
}
