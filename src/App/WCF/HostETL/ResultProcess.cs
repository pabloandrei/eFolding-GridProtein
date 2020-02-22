using System;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using System.ServiceModel;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.ETL.HostETL.ServiceReferenceETL;
using GridProteinFolding.Services.GeneratingResultsService;

namespace GridProteinFolding.ETL.HostETL
{
    class ResultProcess
    {
        private static int timeForConsulting = 3;

        public void Process()
        {
            GICO.WriteLine(String.Format("ETL Service: Service is up and running!"));

            while (true)
            {
                System.Threading.Thread.Sleep(1000 * timeForConsulting);

                AfterSleep();
            }
        }

        private bool AfterSleep()
        {
            bool retRun = false;

            try
            {
                Guid[] guids = Proxy.GetGuidsToApplications();

                if (guids != null)
                {
                    foreach (Guid guid in guids)
                    {
                       ServiceParamWcf paramWCF = Proxy.GetOneProcessGuiForETL(guid);

                        if (paramWCF != null)
                        {
                            ConsoleColor oldColour = Console.ForegroundColor;
                            GICO.ForegroundColor(ConsoleColor.Yellow);

                            GICO.WriteLine(string.Empty);
                            GICO.WriteLine(String.Format("Guid running: {0}", guid.ToString()));

                            GICO.ForegroundColor(oldColour);

                            retRun = Run(paramWCF);

                            Proxy.SetOneProcessByGuid(paramWCF.param.dataToProcess.Guid, BasicEnums.State.ResultsProcessed);

                            GICO.WriteLine(String.Format("Finished Guid: {0}", guid.ToString()));
                            GICO.WriteLine(string.Empty);
                        }
                    }
                }
                return retRun;
            }
            catch (EndpointNotFoundException)
            {
                //new CustomLog().EndpointNotFoundException(ex);
            }

            return false;
        }

        public bool Run(ServiceParamWcf paramWCF)
        {
            Param param = new Param();
            param.dataToProcess = new DataToProcess() { Guid = paramWCF.param.dataToProcess.Guid };
            param.dataToProcess.maxInterations = paramWCF.param.dataToProcess.maxInterations;
            param.dataToProcess.totalSitio = paramWCF.param.dataToProcess.totalSitio;
            param.dataToProcess.Guid = paramWCF.param.dataToProcess.Guid;
            param.dataToProcess.temperature = paramWCF.param.dataToProcess.temperature;
            param.dataToProcess.modelType = paramWCF.param.dataToProcess.modelType;
            param.dataToProcess.crypt = paramWCF.param.dataToProcess.crypt;
            param.dataToProcess.isem = paramWCF.param.dataToProcess.isem;

            param.files = new Files() { Debug = paramWCF.param.files.Debug };

            param.dataToResults = new DataToResults();
            param.dataToResults.valueDiscard = paramWCF.param.dataToResults.valueDiscard;
            param.dataToResults.valueDivResult = paramWCF.param.dataToResults.valueDivResult;

            param.output = new Output();
            param.output.configuration = paramWCF.param.output.configuration;
            //param.output.configurationJumpStep = paramWCF.param.output.configurationJumpStep;
            param.output.evolution = paramWCF.param.output.evolution;
            param.output.distribution = paramWCF.param.output.distribution;
            param.output.debug = paramWCF.param.output.debug;
            param.output.histogram = paramWCF.param.output.histogram;
            param.output.trajectory = paramWCF.param.output.trajectory; 

            Applications objGeneratingResultsReference = new Applications(param);
            objGeneratingResultsReference.Do();
            objGeneratingResultsReference = null;

            param = null;
            paramWCF = null;


            return true;
        }
    }
}
