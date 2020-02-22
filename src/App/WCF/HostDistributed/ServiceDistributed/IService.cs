using System;
using System.ServiceModel;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using System.Collections.Generic;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Entities.Internal;

/*

using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel.Description;
using GridProteinFolding.Entities;
using GridProteinFolding.Entities.Internal;
using System.Collections.ObjectModel;
*/

namespace GridProteinFolding.WCF.ServiceDistributed
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        string Echo();

        [OperationContract]
        bool Autentication(Services.DistributedService.RequestorInfo requestorInfo, ref string message);

        [OperationContract]
        Service.ParamWcf GetOneProcess(Services.DistributedService.RequestorInfo requestorInf);

        [OperationContract]
        Service.ParamWcf GetOneProcessGui(Guid guid, Services.DistributedService.RequestorInfo requestorInfo);

        [OperationContract]
        Service.ParamWcf GetOneProcessGuiForETL(Guid guid, Services.DistributedService.RequestorInfo requestorInfo);
        [OperationContract]
        byte SetOneProcess(Param param, BasicEnums.State state, Services.DistributedService.RequestorInfo requestorInfo);

        [OperationContract]
        byte SetOneProcessByGuid(Guid guid, BasicEnums.State state, Services.DistributedService.RequestorInfo requestorInfo);

        [OperationContract]
        byte GetStatus(Guid guid);

        [OperationContract]
        List<Guid> GetGuidsToApplications();

        [OperationContract]
        bool SetBlob(Guid guid, byte[] data);
    }
}
