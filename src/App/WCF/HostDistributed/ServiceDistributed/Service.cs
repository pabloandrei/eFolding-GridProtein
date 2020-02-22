using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GridProteinFolding.Entities.Internal;
using System.Configuration;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;

/*using System.ServiceModel;GetOneProcessGuiForETL
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using GridProteinFolding.Entities;
using System.Collections.ObjectModel;
*/

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.WCF.ServiceDistributed
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service : IService
    {

        [DataContract]
        //[assembly: CLSCompliant(true)]
        public class ParamWcf
        {
            [DataMember]
            public Param param;

        }

        #region Process

        public string Echo()
        {
            return Services.DistributedService.Service.Echo();
        }

        public bool Autentication(Services.DistributedService.RequestorInfo requestorInfo, ref string message)
        {
            return Services.DistributedService.Service.Autentication(requestorInfo, ref message);
        }


        public ParamWcf GetOneProcessGui(Guid guid, Services.DistributedService.RequestorInfo requestorInfo)
        {
            Process process = Services.DistributedService.Service.GetOneProcessGui(guid, requestorInfo);
            return MakeParams(process);
        }

        public ParamWcf GetOneProcessGuiForETL(Guid guid, Services.DistributedService.RequestorInfo requestorInf)
        {
            Process process = Services.DistributedService.Service.GetOneProcessGuiForETL(guid, requestorInf);
            return MakeParams(process);
        }

        public ParamWcf GetOneProcess(Services.DistributedService.RequestorInfo requestorInf)
        {
            Process process = Services.DistributedService.Service.GetOneProcess(requestorInf);
            return MakeParams(process);
        }

        public List<Guid> GetGuidsToApplications()
        {
            return Services.DistributedService.Service.GetGuidsToApplications();
        }
        #endregion

        private ParamWcf MakeParams(Process process)
        {
            if (process != null)
            {
                ParamWcf paramWCF = new ParamWcf();
                paramWCF.param = new Param();
                paramWCF.param.dataToProcess = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.DataToProcess();
                paramWCF.param.dataToResults = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.DataToResults();
                paramWCF.param.files = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Files();
                paramWCF.param.internalProcess = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.InternalProcess();
                paramWCF.param.model = new List<Middle.Helpers.ConfigurationHelpers.Model>();
                paramWCF.param.configApp = new Middle.Helpers.ConfigurationHelpers.ConfigApp();
                paramWCF.param.output = new Middle.Helpers.ConfigurationHelpers.Output();

                #region configAp
                paramWCF.param.configApp.EqualOne = process.ConfigApp.randomEqualOne;
                paramWCF.param.configApp.MagicNumber = Convert.ToInt32(process.ConfigApp.randomMagicNumber);
                #endregion

                #region dataToProcess
                paramWCF.param.dataToProcess.Guid = process.guid;
                paramWCF.param.dataToProcess.isem = process.DataToProcess.isem;
                paramWCF.param.dataToProcess.maxInterations = process.DataToProcess.maxInterations;
                paramWCF.param.dataToProcess.valueOfDelta = process.DataToProcess.valueOfDelta;
                paramWCF.param.dataToProcess.totalSitio = process.DataToProcess.totalSitio;
                paramWCF.param.dataToProcess.loadDatFile = process.DataToProcess.loadDatFile == null ? false : (bool)process.DataToProcess.loadDatFile;
                paramWCF.param.dataToProcess.file = process.DataToProcess.file;
                paramWCF.param.dataToProcess.maxMotionPeerIsem = process.DataToProcess.maxMotionPeerIsem;
                paramWCF.param.dataToProcess.modelType = process.DataToProcess.modelType;
                paramWCF.param.dataToProcess.beta = process.DataToProcess.beta;

                paramWCF.param.dataToProcess.temperature = process.DataToProcess.temperature;
                paramWCF.param.dataToProcess.crypt = process.crypt;
                paramWCF.param.dataToProcess.recPathEvery = process.DataToProcess.recPathEvery;
                paramWCF.param.dataToProcess.splitFileEvery = process.DataToProcess.splitFileEvery;

                String modelRet = string.Empty;
                foreach (Entities.Internal.Model model in process.Model)
                {
                    modelRet += model.monomero.ToString() + "," + model.value.ToString() + ";";
                }

                if (modelRet != string.Empty)
                {
                    modelRet = modelRet.Substring(0, (modelRet.Count() - 1)); //REMOVER ultimo ; .
                }

                paramWCF.param.dataToProcess.model = modelRet;
                #endregion


                paramWCF.param.files.Debug = "Debug" + Directory.FileExtension;
                paramWCF.param.files.count = 1;
                //if (process.DataToProcess.Targets.Count>0)
                //{
                //    //paramWCF.param.dataToProcess.targets = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Targets();
                //    //paramWCF.param.dataToProcess.targets.targetsCoordinates = new List<GridProteinFolding.Middle.Helpers.ConfigurationHelpers.TargetsCoordinates>();

                //    //paramWCF.param.dataToProcess.targets.id = process.DataToProcess.Targets.id;
                //    //paramWCF.param.dataToProcess.targets.description = process.DataToProcess.Targets.description;

                //    //foreach (GridProteinFolding.Entities.Internal.TargetsCoordinates targetCoord in process.DataToProcess.Targets.TargetsCoordinates)
                //    //{
                //    //    paramWCF.param.dataToProcess.targets.targetsCoordinates.Add(new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.TargetsCoordinates() { id = targetCoord.id, targetsId = targetCoord.targets_id, value = targetCoord.value });
                //    //}
                //}

                #region DataToResults
                paramWCF.param.dataToResults.valueDiscard = process.DataToResult.valueDiscard;
                paramWCF.param.dataToResults.valueDivResult = process.DataToResult.valueDivResult;
                #endregion

                #region Model
                foreach (Entities.Internal.Model item in process.Model)
                {
                    paramWCF.param.model.Add(new Middle.Helpers.ConfigurationHelpers.Model() { monomero = item.monomero, value = item.value });
                }
                #endregion

                #region Output
                paramWCF.param.output.configuration = process.Output.configuration;
                //paramWCF.param.output.configurationJumpStep = process.Output.configurationJumpStep;
                paramWCF.param.output.distribution = process.Output.distribution;
                paramWCF.param.output.evolution = process.Output.evolution;
                paramWCF.param.output.debug = process.Output.debug;
                paramWCF.param.output.histogram = process.Output.histogram;
                paramWCF.param.output.trajectory = process.Output.trajectory;
                #endregion

                return paramWCF;
            }
            else
            {
                return null;
            }
        }

        public byte GetStatus(Guid guid)
        {
            return Services.DistributedService.Service.GetStatus(guid);
        }


        public byte SetOneProcessByGuid(Guid guid, BasicEnums.State state, Services.DistributedService.RequestorInfo requestorInf)
        {
            return Services.DistributedService.Service.SetOneProcess(guid, state, requestorInf);
        }


        public byte SetOneProcess(Param param, BasicEnums.State state, Services.DistributedService.RequestorInfo requestorInf)
        {
            return Services.DistributedService.Service.SetOneProcess(param, state, requestorInf);
        }

        public static void CheckWorkFolders()
        {
            Directory.CreateDirIfNotExist(ConfigurationManager.AppSettings["uploadFolder"].ToString());
        }

        public bool SetBlob(Guid guid, byte[] data)
        {
            return Services.DistributedService.Service.SetBlob(guid, data);
        }
    }
}
