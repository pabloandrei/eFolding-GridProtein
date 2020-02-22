using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Integration;
using System.Configuration;
using System.Diagnostics;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using SIO = System.IO;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Services.GeneratingResultsService
{
    public class Applications : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private Param param;

        public Applications(Param param)
        {
            this.param = param;
        }

        public void Do()
        {
            GICO.WriteLine(String.Format("  Decrypt..."));
            Decrypt();

            ConfigIntegrator configIntegrator = new ConfigIntegrator(param, false);
            GICO.WriteLine(String.Format("  MoveFile..."));
            MoveFile(ref configIntegrator);

            GICO.WriteLine(String.Format("  SimulationResults..."));
            SimulationResults(ref configIntegrator);

            if (param.output.distribution)
            {
                GICO.WriteLine(String.Format("  Output: Distribution..."));
                Distribution(ref configIntegrator);
            }

            if (param.output.evolution)
            {
                GICO.WriteLine(String.Format("  Output: EvolutionRadiusGyration..."));
                EvolutionRadiusGyration(ref configIntegrator);
                GICO.WriteLine(String.Format("  Output: EvolutionValueMediumOfEnergy..."));
                EvolutionValueMediumOfEnergy(ref configIntegrator);
            }

            if (param.output.configuration)
            {
                GICO.WriteLine(String.Format("  Output: Configuration..."));
                ConfigurationOutPut(ref configIntegrator);
            }


            //Excel
            GICO.WriteLine(String.Format("  Excel..."));
            Excel(param.output);

            if (param.output.configuration)
            {
                GICO.WriteLine(String.Format("  Output: LoadConfigurationOutPutToOrigin..."));
                //LoadConfigurationOutPutToOrigin(ref configIntegrator);
                //ORIGIN
                GICO.WriteLine(String.Format("  Origin..."));
                //Origin();
            }

            configIntegrator = null;
            return;
        }

        private void ConfigurationOutPut(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.ConfigurationOutPut();
        }

        private void SimulationResults(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.SimulationResults();
        }

        private void LoadConfigurationOutPutToOrigin(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.LoadConfigurationOutPutForOrigin();
        }

        private void MoveFile(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.MoveFile();
        }


        private void Distribution(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.Distribution();
        }


        private void EvolutionRadiusGyration(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.EvolutionRadiusGyration();
        }


        private void EvolutionValueMediumOfEnergy(ref ConfigIntegrator configIntegrator)
        {
            configIntegrator.EvolutionValueMediumOfEnergy();
        }

        private void Origin()
        {
            OriginIntegrator objOriginIntegrator = new OriginIntegrator(param);
            objOriginIntegrator.Execute();
            objOriginIntegrator = null;
        }

        private void Excel(Middle.Helpers.ConfigurationHelpers.Output output)
        {
            ExcelIntegrator objExcel = new ExcelIntegrator(param);
            objExcel.Execute(output);
            objExcel = null;
        }

        private void Decrypt()
        {
            DecryptIntegrator objDecrypt = new DecryptIntegrator(param);
            objDecrypt.Decrypt();
            objDecrypt = null;
        }

        private void Web()
        {
            return;
        }

    }
}
