using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using ClosedXML.Excel;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.TypesHelpers;
using GridProteinFolding.Middle.Helpers.ConvertHelpers;
using System.Globalization;

namespace GridProteinFolding.Middle.Integration
{

    public class Trajectory
    {
        public int MCStep;
        public double rg;
    }

    /// <summary>
    /// Classe responsável pela integração com OFFICE EXCEL
    /// </summary>
    public class ExcelIntegrator : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private Param param;

        private static ExtendedDirectoryInfo dirBaseServer = new ExtendedDirectoryInfo(ConfigurationManager.AppSettings["dirBaseServer"].ToString());
        private static ExtendedDirectoryInfo dirBaseWeb = new ExtendedDirectoryInfo(ConfigurationManager.AppSettings["dirBaseWeb"].ToString());

        public static string DirBaseServer
        {
            get { return ExcelIntegrator.dirBaseServer.FullName(); }
        }

        public static string DirBaseWeb
        {
            get { return ExcelIntegrator.dirBaseWeb.FullName(); }
        }

        private string dirServerExcel;
        private string dirServerNeighbors;
        private string dirServerHistogram;
        private string dirServerDebug;
        private string dirWebDebug;
        private string dirWebExcel;

        private string excelFileName;

        private int valueDivResult = 0;
        long maxInterations = 0;
        int sites = 0;
        long MCSteps = 0;
        long delta = 0;

        private string debugFile = "Debug" + Directory.FileExtension;

        public ExcelIntegrator(Param param)
        {
            this.param = param;

        }

        public void Execute(Helpers.ConfigurationHelpers.Output output)
        {
            //Config.CurrentGuid = param.dataToProcess.Guid;

            //try
            //{
            this.maxInterations = param.dataToProcess.maxInterations; //numero maximo de tentativas                    
            this.sites = param.dataToProcess.totalSitio; //número total de sítios do monomero

            this.dirServerExcel = DirBaseServer + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirResult + @"\" + Resource.SubDirExcel;
            this.dirWebExcel = DirBaseWeb + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirResult;// + @"\" + Resource.SubDirExcel;
            this.dirServerNeighbors = DirBaseServer + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirDebug + @"\" + Resource.SubDirNeighbors;
            this.dirServerHistogram = DirBaseServer + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirDebug + @"\" + Resource.SubDirHistogram;
            this.dirServerDebug = DirBaseServer + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirDebug + @"\" + Resource.SubDirDebug;
            this.dirWebDebug = DirBaseWeb + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirDebug + @"\" + Resource.SubDirDebug;

            excelFileName = param.dataToProcess.Guid.ToString() + ".xlsx";
            valueDivResult = param.dataToResults.valueDivResult;

            this.MCSteps = maxInterations * sites * 5;
            this.delta = MCSteps / valueDivResult;

            //IF EXIST, EXECUTE INTEGRATOR
            if (Directory.Exists(dirServerDebug))
            {
                GICO.WriteLine(String.Format("  Excel: Integrator..."));
                Integrator(param, output);

                GICO.WriteLine(String.Format("  Excel: CopyFile..."));
                CopyFile(param);
            }
            else
            {
                GICO.WriteLine(AppConfigClient.CurrentGuid, String.Format("Directory not found: {0}  Integrator of Excel not executed...", dirServerDebug));
            }
            //}
            //catch (Exception ex)
            //{
            //    GICO.WriteLine(ex);
            //}
        }

        private void CopyFile(Param param)
        {

            string dirSouce = DirBaseServer + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirResult + @"\" + Resource.SubDirExcel + @"\" + excelFileName;
            string dirDestine = DirBaseWeb + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirResult + @"\" + excelFileName; ;// + @"\" + Resource.SubDirExcel + @"\" + excelFileName;

            GridProteinFolding.Middle.Helpers.IOHelpers.File.Copy(dirSouce, dirDestine, true);
        }

        private void Save(ref XLWorkbook workbook)
        {
            if (workbook.Worksheets.Count() > 0)
                workbook.SaveAs(dirServerExcel + @"\" + excelFileName);

            if (File.Exists(dirServerExcel + @"\" + excelFileName))
            {
                workbook = new XLWorkbook(dirServerExcel + @"\" + excelFileName);
            }
            else
            {
                workbook.SaveAs(dirServerExcel + @"\" + excelFileName);
            }

        }

        private void SaveDebug(ref XLWorkbook workbook)
        {
            if (workbook.Worksheets.Count() > 0)
                workbook.SaveAs(dirServerExcel + @"\" + excelFileName);
        }


        private void Integrator(Param param, Helpers.ConfigurationHelpers.Output output)
        {
            //deleta arquivo, se existir
            if (File.Exists(dirServerExcel + @"\" + excelFileName))
                File.Delete(dirServerExcel + @"\" + excelFileName);

            GridProteinFolding.Middle.Helpers.IOHelpers.Directory.CreateDirIfNotExist(dirServerExcel);

            XLWorkbook workbook = new XLWorkbook();


            if (output.distribution)
            {
                GICO.WriteLine(String.Format("  Excel: SheetDistribution..."));
                SheetDistribution(ref workbook);
                Save(ref workbook);
            }

            if (param.output.evolution)
            {
                GICO.WriteLine(String.Format("  Excel: SheetEvolutionRadiusGyration..."));
                SheetEvolutionRadiusGyration(ref workbook);
                GICO.WriteLine(String.Format("  Excel: SheetEvolutionValueMediumOfEnergy..."));
                SheetEvolutionValueMediumOfEnergy(ref workbook);
                Save(ref workbook);
            }

            if (param.output.configuration)
            {
                GICO.WriteLine(String.Format("  Excel: SheetConfiguration..."));
                SheetConfiguration(ref workbook);
                Save(ref workbook);
            }

            if (output.histogram)
            {
                GICO.WriteLine(String.Format("  Excel: SheetsHistogram..."));
                SheetsHistogram(ref workbook);
                Save(ref workbook);
            }

            if (output.trajectory)
            {
                GICO.WriteLine(String.Format("  Excel: SheetTrajectory..."));
                SheetTrajectory(ref workbook);
                Save(ref workbook);
            }

            if (output.debug)
            {
                GICO.WriteLine(String.Format("  Excel: SheetDebug..."));
                SheetDebug(ref workbook);
                Save(ref workbook);
            }

            workbook = null;

        }


        private void SheetDistribution(ref XLWorkbook workbook)
        {
            // This is invariant
            NumberFormatInfo format = new NumberFormatInfo();
            // Set the 'splitter' for thousands
            format.NumberGroupSeparator = ".";
            // Set the decimal seperator
            format.NumberDecimalSeparator = ",";

            string sheetName = "Distribution";
            string fileName = "Distribution.dat";
            string mask = "";

            var worksheet01 = workbook.Worksheets.Add(sheetName);

            //DADOS
            string line;
            List<string> lines = new List<string>();
            using (ExtendedStreamReader file = new ExtendedStreamReader(dirWebExcel + @"\" + fileName, this.param.dataToProcess.Guid, false))
            {

                while ((line = file.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        break;

                    lines.Add(line);
                }
                file.Close();
                file.Dispose();
            }
            //FIM DADOS

            //HEADER
            worksheet01.Cell("A1").Value = sheetName;
            worksheet01.Cell("B1").Value = mask;
            worksheet01.Cell("C1").Value = mask;
            worksheet01.Cell("D1").Value = "idx";
            worksheet01.Cell("E1").Value = "mcStep";
            worksheet01.Cell("F1").Value = "rg2";
            worksheet01.Cell("G1").Value = "rg2(accumulated)";
            worksheet01.Cell("H1").Value = "<rg2>";
            worksheet01.Cell("I1").Value = "e";
            worksheet01.Cell("J1").Value = "e(accumulated)";
            worksheet01.Cell("K1").Value = "<e>";
            worksheet01.Cell("L1").Value = "U";
            worksheet01.Cell("M1").Value = "U(accumulated)";
            worksheet01.Cell("N1").Value = "<U>";

            //INFOS
            string comment = string.Empty;
            double value = 0;
            int i = 0;

            //DATA
            long idx, mcstep = 0;
            double rg, U, eE, eRG, eU, e, aE, aU, aRG;
            rg = U = eE = eRG = eU = e = aE = aU = aRG = 0;

            //PRIMEIRA PARTE COM HEADER
            for (i = 0; i < 8; i++)
            {

                string[] temp = lines[i].Split(':');
                comment = ConvertCustom.ToString(temp[0]);
                worksheet01.Cell("A" + (i + 2)).Value = comment;

                try
                {
                    value = ConvertCustom.ToDouble(temp[1]);
                    worksheet01.Cell("B" + (i + 2)).Value = value;
                }
                catch (FormatException)
                {
                    string strValue = Convert.ToString(temp[1]);
                    worksheet01.Cell("B" + (i + 2)).Value = strValue;
                }


                string[] tempResult = lines[i + 9].Split('\t');
                idx = ConvertCustom.ToInt32(tempResult[0]);
                mcstep = ConvertCustom.ToInt32(tempResult[1]);

                rg = double.Parse(tempResult[2], format);
                aRG = double.Parse(tempResult[3], format);
                eRG = double.Parse(tempResult[4], format);

                e = double.Parse(tempResult[5], format);
                aE = double.Parse(tempResult[6], format);
                eE = double.Parse(tempResult[7], format);

                U = double.Parse(tempResult[8], format);
                aU = double.Parse(tempResult[9], format);
                eU = double.Parse(tempResult[10], format);

                worksheet01.Cell("C" + (i + 2)).Value = mask;
                worksheet01.Cell("D" + (i + 2)).Value = idx;
                worksheet01.Cell("E" + (i + 2)).Value = mcstep;
                worksheet01.Cell("F" + (i + 2)).Value = rg;
                worksheet01.Cell("G" + (i + 2)).Value = aRG;
                worksheet01.Cell("H" + (i + 2)).Value = eRG;
                worksheet01.Cell("I" + (i + 2)).Value = e;
                worksheet01.Cell("J" + (i + 2)).Value = aE;
                worksheet01.Cell("K" + (i + 2)).Value = eE;
                worksheet01.Cell("L" + (i + 2)).Value = U;
                worksheet01.Cell("M" + (i + 2)).Value = aU;
                worksheet01.Cell("N" + (i + 2)).Value = eU;
            }

            //SEGUNDA PARTE
            for (int j = i; j < (lines.Count() - 9); j++)
            {
                worksheet01.Cell("A" + (j + 2)).Value = mask;
                worksheet01.Cell("B" + (j + 2)).Value = mask;
                worksheet01.Cell("C" + (j + 2)).Value = mask;

                string[] tempResult = lines[j + 9].Split('\t');
                idx = ConvertCustom.ToInt32(tempResult[0]);
                mcstep = ConvertCustom.ToInt32(tempResult[1]);

                rg = double.Parse(tempResult[2], format);
                aRG = double.Parse(tempResult[3], format);
                eRG = double.Parse(tempResult[4], format);

                e = double.Parse(tempResult[5], format);
                aE = double.Parse(tempResult[6], format);
                eE = double.Parse(tempResult[7], format);

                U = double.Parse(tempResult[8], format);
                aU = double.Parse(tempResult[9], format);
                eU = double.Parse(tempResult[10], format);


                worksheet01.Cell("D" + (j + 2)).Value = idx;
                worksheet01.Cell("E" + (j + 2)).Value = mcstep;
                worksheet01.Cell("F" + (j + 2)).Value = rg;
                worksheet01.Cell("G" + (j + 2)).Value = aRG;
                worksheet01.Cell("H" + (j + 2)).Value = eRG;
                worksheet01.Cell("I" + (j + 2)).Value = e;
                worksheet01.Cell("J" + (j + 2)).Value = aE;
                worksheet01.Cell("K" + (j + 2)).Value = eE;
                worksheet01.Cell("L" + (j + 2)).Value = U;
                worksheet01.Cell("M" + (j + 2)).Value = aU;
                worksheet01.Cell("N" + (j + 2)).Value = eU;
            }
            lines = null;
            //FIM DATASHEET

        }

        private void WriteSecondSheet(string line, int idx, ref IXLWorksheet worksheet01)
        {

            List<string> temp = line.Split('\t').ToList<string>();

            for (int j = 0; j < temp.Count; j++)
            {
                string refCell = Number2String(j + 1, true) + (idx + 1).ToString();
                worksheet01.Cell(refCell).Value = temp[j];
            }

        }

        private void SheetDebug(ref XLWorkbook workbook)
        {
            string sheetName = "debug";
            var worksheet01 = workbook.Worksheets.Add(sheetName);

            //DADOS
            string line;
            int numberofLines = ExtendedStreamReader.LinesOfFile(dirServerDebug + @"\" + debugFile);
            //List<string> lines = new List<string>();
            using (ExtendedStreamReader file = new ExtendedStreamReader(dirServerDebug + @"\" + debugFile, this.param.dataToProcess.Guid, this.param.dataToProcess.crypt))
            {
                int rec = 0;
                int idx = 0;

                while ((line = file.ReadLine()) != null)
                {
                    if (!(idx < 1048576))
                        break;

                    if (String.IsNullOrEmpty(line))
                        break;

                    WriteSecondSheet(line, idx, ref worksheet01);
                    idx++;

                    if (rec == 99999)
                    {
                        SaveDebug(ref workbook);
                        //http://stackoverflow.com/questions/22747360/append-to-excel-file-with-closedxml
                        //int NumberOfLastRow = worksheet01.LastRowUsed().RowNumber();
                        //IXLCell CellForNewData = worksheet01.Cell(NumberOfLastRow + 1, 1);
                        rec = 0;

                        PrintPercentCompleted(idx, numberofLines);
                    }
                    else
                    {
                        rec++;
                    }


                }
                file.Close();
                file.Dispose();
            }
            //FIM DADOS
        }

        private static int lastNumber = 0;
        public static void PrintPercentCompleted(int actual, int max)
        {
            long maxInterations = max;

            if (actual != 0)
            {
                double percent = actual;
                percent = ((percent * 100) / maxInterations);

                int value = Convert.ToInt32(percent.ToString("00.##").Substring(0, 2));

                //switch (value)
                //{
                //    case 10:
                //    case 20:
                //    case 30:
                //    case 40:
                //    case 50:
                //    case 60:
                //    case 70:
                //    case 80:
                //    case 90:
                //    case 100:
                if (lastNumber < value)
                {
                    lastNumber = value;
                    GICO.WriteLine(lastNumber + "%.. ");

                }
                //        break;
                //}
            }
            else
                GICO.WriteLine("0%.. ");
        }

        private void SheetsHistogram(ref XLWorkbook workbook)
        {
            string sheetName = string.Empty;

            //Recupera a quantidade de arquivos existentes
            string[] dir = Directory.GetFiles(dirServerHistogram);

            if (dir.Count() > 0)
            {
                foreach (string file in dir)
                {

                    int posBar = file.LastIndexOf(@"\", StringComparison.Ordinal);
                    int len = file.Count();

                    string fileName = file.Substring(posBar, len - posBar).Replace(@"\", "");

                    switch (fileName)
                    {
                        case "fileRealHistogramEndToEndDistance.dat":
                            sheetName = "EndToEnd distance";
                            break;
                        case "fileRealHistogramRadiuosOfGyration.dat":
                            sheetName = "Radiuos of gyration";
                            break;
                        default:
                            sheetName = file;
                            break;
                    }

                    var worksheet01 = workbook.Worksheets.Add(sheetName);

                    //DADOS
                    string line;
                    List<string> lines = new List<string>();
                    using (ExtendedStreamReader stream = new ExtendedStreamReader(dirServerHistogram + @"\" + fileName, this.param.dataToProcess.Guid, this.param.dataToProcess.crypt))
                    {

                        while ((line = stream.ReadLine()) != null)
                        {
                            if (String.IsNullOrEmpty(line))
                                break;

                            lines.Add(line);
                        }
                        stream.Close();

                        stream.Dispose();
                    }
                    //FIM DADOS

                    //DATASHEET
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] values = lines[i].Split('\t');

                        for (int j = 0; j < values.Count(); j++)
                        {
                            worksheet01.Cell(Convert.ToChar(65 + j).ToString() + (i + 1)).Value = values[j];
                        }
                    }

                    lines = null;
                    //FIM DATASHEET

                }
            }

        }


        private void SheetTrajectory(ref XLWorkbook workbook)
        {
            // This is invariant
            NumberFormatInfo format = new NumberFormatInfo();
            // Set the 'splitter' for thousands
            format.NumberGroupSeparator = ".";
            // Set the decimal seperator
            format.NumberDecimalSeparator = ",";

            string sheetName = "Trajectory";

            var worksheet01 = workbook.Worksheets.Add(sheetName);

            int numberLines = ExtendedStreamReader.LinesOfFile(dirServerDebug + @"\" + debugFile);

            //DADOS
            string line;
            List<Trajectory> trajectory = new List<Trajectory>();
            int _MCStep = 0;
            using (ExtendedStreamReader file = new ExtendedStreamReader(dirServerDebug + @"\" + debugFile, this.param.dataToProcess.Guid, this.param.dataToProcess.crypt))
            {

                double rg = 0; //Calculo acumulado do Raio de GIração

                int cont = 0; //Contador
                              //int lastMove = 0; //valor do último movimento
                int contAdd = 1;//contador adicional



                while ((line = file.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        break;

                    if (cont > 0)
                    {
                        string[] col = line.Split('\t');

                        int numberMov = ConvertCustom.ToInt32(col[0]);

                        if (numberMov < (delta * contAdd) + 1)
                        {
                            rg += double.Parse(col[3], format); //coluna no qual contêm valor do raio de giração
                            _MCStep = ConvertCustom.ToInt32(col[0]);
                        }
                        else
                        {
                            double value = ((double)1 / delta) * rg;
                            trajectory.Add(new Trajectory() { MCStep = _MCStep, rg = value });
                            contAdd++;
                            rg = 0;
                            rg += Convert.ToDouble(col[3]); //coluna no qual contêm valor do raio de giração
                        }

                    }

                    cont++;
                }
                file.Close();
                file.Dispose();
            }
            //FIM DADOS


            //DATASHEET
            worksheet01.Cell("A1").Value = sheetName;

            worksheet01.Cell("A2").Value = "lines";
            worksheet01.Cell("B2").Value = numberLines;

            worksheet01.Cell("A3").Value = "MCSteps";
            worksheet01.Cell("B3").Value = MCSteps;

            worksheet01.Cell("A4").Value = "delta";
            worksheet01.Cell("B4").Value = delta;

            worksheet01.Cell("A5").Value = "idx";
            worksheet01.Cell("B5").Value = "MCSteps";
            worksheet01.Cell("C5").Value = "SUM((1 / delta) * md)";

            for (int i = 0; i < trajectory.Count; i++)
            {
                double value = trajectory[i].rg;
                _MCStep = trajectory[i].MCStep;
                worksheet01.Cell("A" + (i + 6)).Value = i;
                worksheet01.Cell("B" + (i + 6)).Value = _MCStep;
                worksheet01.Cell("C" + (i + 6)).Value = value;
            }
        }

        private void SheetEvolutionRadiusGyration(ref XLWorkbook workbook) //SpreadsheetDocument ssDoc, Sheets sheets, WorkbookPart workbookPart)
        {
            string sheetName = "Evolution RG";
            string evolutionRGFile = "EvolutionRG.dat";

            var worksheet01 = workbook.Worksheets.Add(sheetName);

            //DADOS
            string line;
            List<string> lines = new List<string>();
            using (ExtendedStreamReader file = new ExtendedStreamReader(dirWebExcel + @"\" + evolutionRGFile, this.param.dataToProcess.Guid, false))
            {

                while ((line = file.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        break;

                    lines.Add(line);
                }
                file.Close();
                file.Dispose();
            }
            //FIM DADOS

            //HEADER
            worksheet01.Cell("A1").Value = sheetName;

            //INFOS
            string comment = string.Empty;
            double value = 0;
            int i = 0;

            for (i = 0; i < 5; i++)
            {
                string[] temp = lines[i].Split(':');
                comment = Convert.ToString(temp[0]);
                worksheet01.Cell("A" + (i + 2)).Value = comment;

                try
                {
                    value = Convert.ToDouble(temp[1]);
                    worksheet01.Cell("B" + (i + 2)).Value = value;
                }
                catch
                {
                    string valueStr = temp[1];
                    worksheet01.Cell("B" + (i + 2)).Value = valueStr;
                }
            }

            //DATA
            worksheet01.Cell("A7").Value = "idx";
            worksheet01.Cell("B7").Value = "MCStep";
            worksheet01.Cell("C7").Value = "RG";

            long mcstep = 0;
            double rg = 0;
            long idx = 0;

            i++; //para pular HEADER do arquivo que já foi escrito acima (LINHA 7 do EXCEL)

            for (i = i; i < lines.Count; i++)
            {
                string[] temp = lines[i].Split('\t');
                idx = Convert.ToInt32(temp[0]);
                mcstep = Convert.ToInt32(temp[1]);
                rg = Convert.ToDouble(temp[2]);

                worksheet01.Cell("A" + (i + 2)).Value = idx;
                worksheet01.Cell("B" + (i + 2)).Value = mcstep;
                worksheet01.Cell("C" + (i + 2)).Value = rg;
            }

            lines = null;
            //FIM DATASHEET
        }

        private void SheetEvolutionValueMediumOfEnergy(ref XLWorkbook workbook)
        {
            string sheetName = "Evolution Energy";
            string mediumValuesOfEnergy = "EvolutionEnergy.dat";

            var worksheet01 = workbook.Worksheets.Add(sheetName);

            //DADOS
            string line;
            List<string> lines = new List<string>();
            using (ExtendedStreamReader file = new ExtendedStreamReader(dirWebExcel + @"\" + mediumValuesOfEnergy, this.param.dataToProcess.Guid, false))
            {

                while ((line = file.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        break;

                    lines.Add(line);
                }
                file.Close();
                file.Dispose();
            }
            //FIM DADOS

            //HEADER
            worksheet01.Cell("A1").Value = sheetName;

            //INFOS
            string comment = string.Empty;
            double value = 0;
            int i = 0;

            for (i = 0; i < 5; i++)
            {
                string[] temp = lines[i].Split(':');
                comment = Convert.ToString(temp[0]);
                worksheet01.Cell("A" + (i + 2)).Value = comment;

                try
                {
                    value = Convert.ToDouble(temp[1]);
                    worksheet01.Cell("B" + (i + 2)).Value = value;
                }
                catch
                {
                    string valueStr = temp[1];
                    worksheet01.Cell("B" + (i + 2)).Value = valueStr;
                }
            }

            //DATA
            worksheet01.Cell("A7").Value = "idx";
            worksheet01.Cell("B7").Value = "MCStep";
            worksheet01.Cell("C7").Value = "E";

            long mcstep = 0;
            double e = 0;
            long idx = 0;

            i++; //para pular HEADER do arquivo que já foi escrito acima (LINHA 7 do EXCEL)
            for (i = i; i < lines.Count; i++)
            {
                string[] temp = lines[i].Split('\t');
                idx = Convert.ToInt32(temp[0]);
                mcstep = Convert.ToInt32(temp[1]);
                e = Convert.ToDouble(temp[2]);

                worksheet01.Cell("A" + (i + 2)).Value = idx;
                worksheet01.Cell("B" + (i + 2)).Value = mcstep;
                worksheet01.Cell("C" + (i + 2)).Value = e;

            }

            lines = null;
            //FIM DATASHEET
        }


        private void SheetConfiguration(ref XLWorkbook workbook)
        {
            string sheetName = "Configuration";
            string configuration = "Configuration.dat";

            var worksheet01 = workbook.Worksheets.Add(sheetName);


            //DADOS
            string line;
            List<string> lines = new List<string>();
            ExtendedStreamReader file = new ExtendedStreamReader(dirWebExcel + @"\" + configuration, this.param.dataToProcess.Guid, false);

            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrEmpty(line))
                    break;

                lines.Add(line);
            }
            file.Close();
            //FIM DADOS

            //HEADER
            worksheet01.Cell("A1").Value = sheetName;


            //INFOS
            string value = string.Empty;
            string refCell = string.Empty;

            for (int i = 0; i < lines.Count; i++)
            {
                string[] cols = lines[i].Split('\t');

                for (int j = 0; j < cols.Length; j++)
                {
                    value = Convert.ToString(cols[j]);
                    //refCell = ((char)(65 + j)).ToString();
                    refCell = ExcelColumnFromNumber(j + 1) + (i + 2);
                    worksheet01.Cell(refCell).Value = value;
                }
            }

            lines = null;
            //FIM DATASHEET

        }

        public static string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        private String Number2String(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }

    }
}



//private Cell CellText(string text, string cellReference)
//{
//    Cell cell = new Cell();
//    cell.DataType = CellValues.InlineString;
//    cell.InlineString = new InlineString { Text = new Text { Text = text } };
//    cell.CellReference = cellReference;
//    return cell;
//}

//private Cell CellNumber(string number, string cellReference)
//{
//    Cell cell = new Cell();
//    cell.DataType = CellValues.Number;
//    cell.CellValue = new CellValue(number);
//    cell.CellReference = cellReference;
//    return cell;
//}