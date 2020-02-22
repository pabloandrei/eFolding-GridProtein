using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
using GridProteinFolding.Middle.Helpers.TypesHelpers;
using GridProteinFolding.Entities.Internal;
//using EntityFramework.BulkInsert.Extensions;
using System.Transactions;

namespace GridProteinFolding.Middle.Integration
{

    class ColsForDepedentModel
    {
        public int mcSteps;
        public double rg;
        public double e;
        public double U;
    }

    class ColsForRG
    {
        public int mcSteps = 0;
        public double rg = 0;
    }

    class ColsForEnergy
    {
        public int mcSteps = 0;
        public double e = 0;

    }

    public class ConfigIntegrator
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private Param param;
        private long MCSteps;
        private int totalSitio;
        private long maxInterations;
        private int valueDivResult;
        private int valueDiscard;

        private string debugFile = "Debug" + Directory.FileExtension;

        ExtendedDirectoryInfo dirBaseServer;
        ExtendedDirectoryInfo dirBaseWeb;

        int movimentsOccurred;
        int delta;
        long mcSteps;

        public ConfigIntegrator(Param param, bool filesMoved)
        {

            this.param = param;
            this.totalSitio = param.dataToProcess.totalSitio;
            this.MCSteps = param.dataToProcess.maxInterations * totalSitio * 5;
            this.valueDivResult = param.dataToResults.valueDivResult;
            this.valueDiscard = param.dataToResults.valueDiscard;
            this.maxInterations = param.dataToProcess.maxInterations;

            this.dirBaseServer = new ExtendedDirectoryInfo(DirBaseService.GetDirBaseService().dirBaseServer);
            this.dirBaseWeb = new ExtendedDirectoryInfo(DirBaseService.GetDirBaseService().dirBaseWeb);

            if (!filesMoved)
                this.movimentsOccurred = ExtendedStreamReader.LinesOfFile(dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Debug\Debug.dat");
            else
                this.movimentsOccurred = ExtendedStreamReader.LinesOfFile(dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Debug.dat");

            this.delta = (movimentsOccurred - valueDiscard) / valueDivResult;

            this.mcSteps = param.dataToProcess.maxInterations * 5 * param.dataToProcess.totalSitio;
        }

        public void MoveFile()
        {
            try
            {
                string dirSouce = dirBaseWeb.FullName() + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirDebug;
                string dirDestine = dirBaseWeb.FullName() + param.dataToProcess.Guid.ToString() + @"\" + Resource.DirResult;


                GridProteinFolding.Middle.Helpers.IOHelpers.Directory.CreateDirIfNotExist(dirDestine);

                foreach (string tempFile in Directory.GetFilesOfDir(dirSouce))
                {
                    GridProteinFolding.Middle.Helpers.IOHelpers.File.Copy(tempFile, dirDestine + @"\\" + Path.GetFileName(tempFile), true);
                }

                GridProteinFolding.Middle.Helpers.IOHelpers.Directory.DeleteFileAndDirIfExists(dirSouce, null);
                GridProteinFolding.Middle.Helpers.IOHelpers.Directory.Delete(dirSouce);

                int posBar = dirSouce.LastIndexOf(@"\", StringComparison.Ordinal);
                GridProteinFolding.Middle.Helpers.IOHelpers.Directory.DeleteFileAndDirIfExists(dirSouce.Substring(0, dirSouce.Length - (dirSouce.Length - posBar)), null);
            }
            catch (Exception ex)
            {
                GICO.WriteLine(ex);
                throw;
            }
        }

        public void SimulationResults()
        {
            string originFile = dirBaseServer.FullName() + this.param.dataToProcess.Guid + @"\Result\Trajectory\SimulationResults.dat";
            string destFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\SimulationResults.dat";
            File.Copy(originFile, destFile, true);
        }


        public void Distribution()
        {
            try
            {
                string readerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Debug.dat";
                string writerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Distribution.dat";

                Guid guid = param.dataToProcess.Guid;
                int isem = param.dataToProcess.isem;
                int sitios = param.dataToProcess.totalSitio;

                //Data
                string line;
                List<ColsForDepedentModel> md = new List<ColsForDepedentModel>();

                using (ExtendedStreamReader souceFile = new ExtendedStreamReader(readerFile,
                    this.param.dataToProcess.Guid, false))
                {

                    ExtendedStreamWriter destinationFile = new ExtendedStreamWriter(writerFile, false,
                       this.param.dataToProcess.Guid, false);

                    string[] cols = null;
                    int i = 0;

                    while ((line = souceFile.ReadLine()) != null)
                    {

                        if (String.IsNullOrEmpty(line))
                            break;

                        //pulas cabeçalho &  faz o discarte o número de discartes
                        if (i == 0)
                        {
                            for (int k = 0; k < this.valueDiscard; k++)
                            {
                                line = souceFile.ReadLine();
                            }

                        }
                        else
                        {

                            cols = line.Split('\t').ToArray();
                            Int32 _mcSteps = Convert.ToInt32(cols[0]);
                            double _rg = Convert.ToDouble(cols[3]);
                            double _e = Convert.ToDouble(cols[2]);
                            double _U = Convert.ToDouble(cols[8]);

                            md.Add(new ColsForDepedentModel() { mcSteps = _mcSteps, rg = _rg, e = _e, U = _U });

                            //pula o Delta
                            for (int k = 0; k < this.delta; k++)
                            {
                                line = souceFile.ReadLine();
                            }

                        }

                        i++;
                    }

                    souceFile.Close();
                    //souceFile = null;



                    //Write result file
                    destinationFile.WriteLine("simulation id: {0}", guid.ToString());
                    destinationFile.WriteLine("isem: {0}", isem);
                    destinationFile.WriteLine("sitios: {0}", sitios);
                    destinationFile.WriteLine("model: {0}", TranslateModel(param.dataToProcess.modelType));
                    destinationFile.WriteLine("mcStep: {0}", this.mcSteps);
                    destinationFile.WriteLine("temperature: {0}", param.dataToProcess.temperature);
                    destinationFile.WriteLine("valueDiscard: {0}", this.valueDiscard);
                    destinationFile.WriteLine("delta: {0}", this.delta);

                    destinationFile.WriteLine("idx\t mcSteps\t rg2\t rg2(accumulated)\t <rg2>\t e\t e(accumulated)\t <e>\t U\t U(accumulated)\t <U>");

                    double aRG2, eRG2, previousRG2, actualRG2;
                    aRG2 = eRG2 = previousRG2 = actualRG2 = 0;
                    double aE, eE, previousE, actualE;
                    aE = eE = previousE = actualE = 0;
                    double aU, eU, previousU, actualU;
                    aU = eU = previousU = actualU = 0;

                    Int64 idx = 0;

                    for (int z = 0; z < md.Count(); z++)
                    {
                        idx = (z + 1);

                        //Acumulado
                        aRG2 += md[z].rg;
                        aE += md[z].e;
                        aU += md[z].U;

                        //Evolução do RAIO DE GIRAÇÃO
                        actualRG2 = md[z].rg;
                        eRG2 = aRG2 / idx;
                        previousRG2 = eRG2;

                        //Evolução do E
                        actualE = md[z].e;
                        eE = aE / idx;
                        previousE = eE;

                        //Evolução do U
                        actualU = md[z].U;
                        eU = aU / idx;
                        previousE = eU;


                        destinationFile.WriteLine(z + "\t" + md[z].mcSteps + "\t" + actualRG2 + "\t" + aRG2 + "\t" + eRG2 + "\t" + actualE + "\t" + aE + "\t" + eE + "\t" + actualU + "\t" + aU + "\t" + eU);

                    }

                    destinationFile.Flush();
                    destinationFile.Close();
                    souceFile.Dispose();
                }
            }
            catch (Exception ex)
            {
                GICO.WriteLine(ex);
                throw;
            }
        }

        private static string TranslateModel(byte modelType)
        {
            string model = string.Empty;

            //0-Randon; 1-Negative; 2-HP; 3-estero-quimico,4-GO
            switch (Convert.ToInt32(modelType))
            {
                case 0:
                    model = "Random";
                    break;
                case 1:
                    model = "Hydrophobic";
                    break;
                case 2:
                    model = "Polar";
                    break;
                case 3:
                    model = "HP";
                    break;
                default:
                    model = "N/A";
                    break;
            }

            return model;
        }

        private GridProteinFoldingEntities AddToContext(GridProteinFoldingEntities context,
            Entities.Internal.Configuration entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<Entities.Internal.Configuration>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new GridProteinFoldingEntities();
                    //context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }

        public void LoadConfigurationOutPutForOrigin()
        {
            GICO.WriteLine(String.Format("  Output: LoadConfigurationOutPutToOrigin: Delete..."));
            ////Delete IF EXIST
            //using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            //{
            //    ctx.Database.ExecuteSqlCommand("DELETE FROM OUTPUT WHERE guid= {0}", param.dataToProcess.Guid);
            //}

            string readerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Configuration.dat";
            using (ExtendedStreamReader souceFile = new ExtendedStreamReader(readerFile,
                this.param.dataToProcess.Guid, false))
            {

                string line;
                int i = 0;


                GICO.WriteLine(String.Format("  Output: LoadConfigurationOutPutToOrigin: Insert..."));
                while ((line = souceFile.ReadLine()) != null)
                {

                    if (String.IsNullOrEmpty(line))
                        break;

                    //pulas cabeçalho e o número de discartes
                    if (i == 0)
                    {
                        line = souceFile.ReadLine();
                    }

                    string[] data = line.Split('\t').ToArray();
                    Int32 mcStep = 0;
                    int order = 0;
                    int count = 0;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        GridProteinFoldingEntities ctx = null;

                        try
                        {
                            ctx = new GridProteinFoldingEntities();
                            Entities.Internal.Configuration configuration = null;

                            for (int j = 0; j < data.Length; j++)
                            {
                                if (j == 0) { mcStep = Convert.ToInt32(data[j]); j++; }

                                configuration = new Entities.Internal.Configuration();

                                configuration.process_guid = this.param.dataToProcess.Guid;
                                configuration.mcStep = mcStep;
                                configuration.order = order;
                                configuration.x = Convert.ToInt32(data[j]); j++;
                                configuration.y = Convert.ToInt32(data[j]); j++;
                                configuration.z = Convert.ToInt32(data[j]);

                                order++;

                                ++count;
                                ctx = AddToContext(ctx, configuration, count, 100, true);
                                //ctx.Configuration.Add(configuration);
                            }

                            if (configuration != null)
                            {
                                ctx.SaveChanges();
                                configuration = null;
                            }
                        }
                        finally
                        {
                            if (ctx != null)
                                ctx.Dispose();
                        }
                        scope.Complete();
                    }
                    i++;
                }


                souceFile.Close();
                //souceFile = null;
                souceFile.Dispose();
            }
        }


        public void ConfigurationOutPut()
        {
            string[] files = Directory.GetFilesOfDir(dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result", "Trajectory*.dat");
            Array.Sort<string>(files);
            Array.Reverse(files);
            string readerLastFile = files[0];
            Array.Sort<string>(files);

            //Data
            string line;
            List<string> configurations = new List<string>();

            //FILEs - lê arquivo por arquivo efetuando o SALTO de MCTSP efetuado
            string readerFirstFile = string.Empty;

            ExtendedStreamReader souceFirstFile;
            int i = 0;
            for (int numberOfFile = 0; numberOfFile < files.Length; numberOfFile++)
            {
                readerFirstFile = files[numberOfFile];
                using (souceFirstFile = new ExtendedStreamReader(readerFirstFile,
                this.param.dataToProcess.Guid, false))
                {
                    bool readHeader = false;

                    while ((line = souceFirstFile.ReadLine()) != null)
                    {
                        if (String.IsNullOrEmpty(line))
                            break;

                        //cabeçalho do primeiro arquivo
                        if (!readHeader)
                        {
                            configurations.Add(line);
                            readHeader = true;
                            line = souceFirstFile.ReadLine();
                        }

                        if (i == 0)
                        {
                            //for (int k = 0; k < this.valueDiscard; k++)
                            //{
                            //    line = souceFirstFile.ReadLine();
                            //}
                            configurations.Add(line);
                        }
                        else
                        {
                            //if (i == param.output.configurationJumpStep)
                            //{
                            //    i = 0;
                            configurations.Add(line);
                            //}
                        }
                        i++;


                    }
                    souceFirstFile.Close();

                    souceFirstFile.Dispose();
                }
            }

            //LAST FILE
            string writerConfigurationFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Configuration.dat";
            ExtendedStreamWriter destinationFile = new ExtendedStreamWriter(writerConfigurationFile, false,
               this.param.dataToProcess.Guid, false);

            line = string.Empty;
            string last = File.ReadLastLineOfFile(readerLastFile);
            configurations.Add(last);



            souceFirstFile = null;

            //Write result file
            foreach (string data in configurations)
            {
                destinationFile.WriteLine(data);
            }

            destinationFile.Flush();
            destinationFile.Close();

            //Após consolidar arquivo de CONFIGURATION.DAT, os arquivos de TRAJETORY* devem ser descartados
            foreach (string file in files)
            {
                File.Delete(file);
            }

        }

        public void EvolutionRadiusGyration()
        {

            string readerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Debug.dat";
            string writerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\EvolutionRG.dat";

            //Data
            string line;
            List<ColsForRG> rg = new List<ColsForRG>();

            using (ExtendedStreamReader souceFile = new ExtendedStreamReader(readerFile,
                this.param.dataToProcess.Guid, false))
            {

                ExtendedStreamWriter destinationFile = new ExtendedStreamWriter(writerFile, false,
                   this.param.dataToProcess.Guid, false);

                string[] cols = null;
                int i = 0;

                while ((line = souceFile.ReadLine()) != null)
                {

                    if (String.IsNullOrEmpty(line))
                        break;

                    //pulas cabeçalho e o número de discartes
                    if (i == 0)
                    {
                        for (int k = 0; k < this.valueDiscard; k++)
                        {
                            line = souceFile.ReadLine();
                        }

                    }
                    else
                    {

                        cols = line.Split('\t').ToArray();
                        Int32 _mcSteps = Convert.ToInt32(cols[0]);
                        double _rg = Convert.ToDouble(cols[3]);

                        rg.Add(new ColsForRG() { mcSteps = _mcSteps, rg = _rg });

                        //pula o Delta
                        for (int k = 0; k < this.delta; k++)
                        {
                            line = souceFile.ReadLine();
                        }

                    }

                    i++;
                }

                souceFile.Close();
                //souceFile = null;

                //Write result file
                destinationFile.WriteLine("mcStep: {0}", this.mcSteps);
                destinationFile.WriteLine("valueDiscard: {0}", this.valueDiscard);
                destinationFile.WriteLine("delta: {0}", this.delta);
                destinationFile.WriteLine("temperature: {0}", param.dataToProcess.temperature);
                destinationFile.WriteLine("model: {0}", TranslateModel(param.dataToProcess.modelType));

                destinationFile.WriteLine("idx" + "\t" + "_mcSteps" + "\t" + "rg2");
                for (int z = 0; z < rg.Count(); z++)
                {

                    destinationFile.WriteLine(z + "\t" + rg[z].mcSteps + "\t" + rg[z].rg);

                }

                destinationFile.Flush();
                destinationFile.Close();
                souceFile.Dispose();
            }

        }


        public void EvolutionValueMediumOfEnergy()
        {
            string readerFile = dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\Debug.dat";
            string writerFile = @dirBaseWeb.FullName() + this.param.dataToProcess.Guid + @"\Result\EvolutionEnergy.dat";

            //Data
            string line;
            List<ColsForEnergy> energy = new List<ColsForEnergy>();

            using (ExtendedStreamReader souceFile = new ExtendedStreamReader(readerFile,
                this.param.dataToProcess.Guid, false))
            {

                ExtendedStreamWriter destinationFile = new ExtendedStreamWriter(writerFile, false,
                   this.param.dataToProcess.Guid, false);

                string[] cols = null;
                int i = 0;

                while ((line = souceFile.ReadLine()) != null)
                {

                    if (String.IsNullOrEmpty(line))
                        break;

                    //pulas cabeçalho e o número de discartes
                    if (i < 1)
                    {
                        for (int k = 0; k < this.valueDiscard; k++)
                        {
                            line = souceFile.ReadLine();
                        }

                    }
                    else
                    {
                        cols = line.Split('\t').ToArray();

                        Int32 _mcSteps = Convert.ToInt32(cols[0]);
                        double _newE = Convert.ToDouble(cols[2]);

                        energy.Add(new ColsForEnergy() { mcSteps = _mcSteps, e = _newE });

                        //pula o Delta
                        for (int k = 0; k < this.delta; k++)
                        {
                            line = souceFile.ReadLine();
                        }

                    }

                    i++;
                }

                souceFile.Close();
                //souceFile = null;

                //Write result file
                destinationFile.WriteLine("mcStep: {0}", this.mcSteps);
                destinationFile.WriteLine("valueDiscard: {0}", this.valueDiscard);
                destinationFile.WriteLine("delta: {0}", this.delta);
                destinationFile.WriteLine("temperature: {0}", param.dataToProcess.temperature);
                destinationFile.WriteLine("model: {0}", TranslateModel(param.dataToProcess.modelType));

                destinationFile.WriteLine("idx" + "\t" + "_mcSteps" + "\t" + "energy");
                for (int z = 0; z < energy.Count(); z++)
                {

                    destinationFile.WriteLine(z + "\t" + energy[z].mcSteps + "\t" + energy[z].e);

                }

                destinationFile.Flush();
                destinationFile.Close();
                souceFile.Dispose();
            }
        }

    }
}
