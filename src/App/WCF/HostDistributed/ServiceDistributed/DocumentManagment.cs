using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using GridProteinFolding;


namespace GridProteinFolding.WCF.ServiceDistributed
{
    public class DocumentManagment : ServiceDistributed.IDocumentManagment
    {
        public bool DocumentEcho(string fileName)
        {

            Services.DocumentManagementService.DocumentLibraryService objDocument = new Services.DocumentManagementService.DocumentLibraryService();
            return objDocument.DocumentEcho(fileName);

        }

        public void UploadDocument(string fileName, byte[] data, bool append)
        {
            Services.DocumentManagementService.DocumentLibraryService objDocument = new Services.DocumentManagementService.DocumentLibraryService();
            objDocument.UploadDocument(fileName, data, append);
            objDocument = null;
        }

        public void ExtractDocument(string fileName)
        {
            Services.DocumentManagementService.DocumentLibraryService objDocument = new Services.DocumentManagementService.DocumentLibraryService();
            objDocument.ExtractDocument(fileName);
            objDocument = null;
        }


        //public byte[] DownloadDocument(string fPath)
        //fileName
        //    DocumentManagementService.DocumentLibraryService objDocument = new DocumentManagementService.DocumentLibraryService();
        //    return objDocument.DownloadDocument(fPath);

        //}
    }
}
