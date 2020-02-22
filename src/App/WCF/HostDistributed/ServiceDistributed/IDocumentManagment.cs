using System;
using System.ServiceModel;


namespace GridProteinFolding.WCF.ServiceDistributed
{
    [ServiceContract]
    interface IDocumentManagment
    {
        [OperationContract]
        bool DocumentEcho(string fileName);
        //[OperationContract]
        //byte[] DownloadDocument(string fileName);
        [OperationContract]
        void UploadDocument(string fileName, byte[] data, bool append);
        [OperationContract]
        void ExtractDocument(string fileName);
    }
}
