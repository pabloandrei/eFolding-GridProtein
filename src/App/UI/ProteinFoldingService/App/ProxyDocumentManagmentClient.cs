using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using GridProteinFolding.UI.ProteinFoldingService.DocumentReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace GridProteinFolding.UI.ProteinFoldingService.App
{
    class ProxyDocumentManagmentClient
    {
        //número de tentativas para consumo de serviços WCF
        private static int numberTry = 5;
        public ProxyDocumentManagmentClient() { }

        public static DocumentManagmentClient objDistributed;

        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            objDistributed = null;

            System.GC.SuppressFinalize(this);
        }

        #region PRIVATE
        private static void OpenChannel()
        {
            try
            {
                objDistributed = new DocumentManagmentClient();
                objDistributed.Open();
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex);
                new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, 0, true);
                throw;
            }

        }

        private static void CloseChannel()
        {
            try
            {
                if (objDistributed != null)
                {
                    if (objDistributed.State != CommunicationState.Faulted)
                    {
                        objDistributed.Close();
                    }
                    else
                    {
                        objDistributed.Abort();
                    }
                }
            }
            catch (CommunicationException ex)
            {
                new CustomLog().CommunicationException(ex);
                new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, 0, true);
                // Communication exceptions are normal when
                // closing the connection.
                objDistributed.Abort();
            }
            catch (TimeoutException ex)
            {
                new CustomLog().TimeoutException(ex, Types.ErrorLevel.Error);
                new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, 0, true);
                // Timeout exceptions are normal when closing
                // the connection.
                objDistributed.Abort();
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex, Types.ErrorLevel.Error);
                // Any other exception and you should 
                // abort the connection and rethrow to 
                // allow the exception to bubble upwards.
                objDistributed.Abort();
                throw;
            }
            finally
            {
                // This is just to stop you from trying to 
                // close it again (with the null check at the start).
                // This may not be necessary depending on
                // your architecture.
                objDistributed = null;
            }

        }

        //private static void CheckStatusChannel()
        //{

        //    if (objDistributed.State == CommunicationState.Faulted)
        //    {
        //        new CustomLog().CommunicationException(new CommunicationException("CommunicationState.Faulted"));
        //        OpenChannel();
        //    }
        //    else if (objDistributed.State != CommunicationState.Opened)
        //    {
        //        OpenChannel();
        //    }
        //}
        #endregion

        public static void DocumentEcho(FileInfo destFileName)
        {
            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    objDistributed.DocumentEcho(destFileName.Name.ToString());
                    CloseChannel();
                    return;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().CommunicationException(ex);
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    //CheckStatusChannel();
                    //throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    //throw;
                }
            }
        }
        public static void UploadDocument(FileInfo destFileName, byte[] bytes, bool append)
        {
            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    objDistributed.UploadDocument(destFileName.Name.ToString(), bytes, append);
                    CloseChannel();
                    return;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().CommunicationException(ex);
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    //CheckStatusChannel();
                    //throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    //throw;
                }
            }
        }

        public static void ExtractDocument(FileInfo destFileName)
        {
            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    objDistributed.ExtractDocument(destFileName.Name.ToString());
                    CloseChannel();
                    return;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().CommunicationException(ex);
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    //CheckStatusChannel();
                    //throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    //throw;
                }
            }

        }
    }
}
