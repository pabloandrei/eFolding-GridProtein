//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Text;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Description;
//using GridProteinFolding.Entities;
//using GridProteinFolding.Entities.Internal;
//using System.Configuration;
//using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
//using GridProteinFolding.Middle.Helpers.IOHelpers;

//namespace GridProteinFolding.WCF.ServiceDistributed
//{
//    public class GeneratingResults : GridProteinFolding.WCF.ServiceDistributed.IGeneratingResults
//    {
//        public void Do(Param paramWCF)
//        {
//            Services.GeneratingResultsService.Applications objApplications = new Services.GeneratingResultsService.Applications(paramWCF);
//            objApplications.Do();

//            return;
//        }
//    }
//}
