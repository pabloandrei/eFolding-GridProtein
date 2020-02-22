using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GridProteinFolding.Middle.Helpers.DebugHelpers;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.UI.ProteinFoldingService.ServiceReference;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;

namespace GridProteinFolding.UI.ProteinFoldingService.App
{
    public class ProxyServiceClient
    {
        //número de tentativas para consumo de serviços WCF
        private static int numberTry = 5;
        private static System.Net.NetworkInformation.PhysicalAddress macAddr = Net.GetMacAddress();
        private static RequestorInfo requestorInfo = new RequestorInfo()
        {
            macAddr = macAddr.ToString(),
            machineName = Environment.MachineName,
            clientVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
            password = "anonymous",
            user = "anonymous"
        };

        public ProxyServiceClient()
        {

        }
        public static ServiceClient objDistributed;

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
                objDistributed = new ServiceClient();
                objDistributed.Open();
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex, Types.ErrorLevel.Error);
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
        #endregion

        public static bool Echo()
        {
            try
            {
                OpenChannel();
                bool retEcho = objDistributed.Echo().Equals("echo");
                CloseChannel();
                return retEcho;
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex);
                //throw;
            }
            return false;
        }
        public static byte SetStatus(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            //CheckStatusChannel();
            byte retProxy = byte.MinValue;
            //Muda Status no Servidor 
            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    retProxy = objDistributed.SetOneProcess(newParam, state, requestorInfo);
                    CloseChannel();
                    break;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                    //throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    //throw;
                }
            }

            Sql.entityProcess.guid = newParam.dataToProcess.Guid;
            Sql.entityProcess.status = Convert.ToByte(retProxy);
            Sql.entityProcess.date = DateTime.Now;
            //Muda Status no Cliente
            GridProteinFolding.Data.XMLData.Bussines.Actions.UpdateProcess(Sql.entityProcess);

            return retProxy;
        }

        public static byte GetStatus(Guid guid)
        {
            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    byte ret = objDistributed.GetStatus(guid);
                    CloseChannel();
                    return ret;
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

            return (int)BasicEnums.State.Error;
        }

        public static ServiceParamWcf GetOneProcess()
        {
            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    ServiceParamWcf ret = objDistributed.GetOneProcess(requestorInfo);
                    CloseChannel();
                    return ret;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                    throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    throw;
                }
            }

            return null;
        }



        public static ServiceParamWcf GetOneProcessGui(Guid gui)
        {


            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    ServiceParamWcf ret = objDistributed.GetOneProcessGui(gui, requestorInfo);
                    CloseChannel();
                    return ret;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().CommunicationException(ex);
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    //CheckStatusChannel();
                    throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    throw;
                }
            }

            return null;
        }


        public static bool Autentication(ref string message)
        {
            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    bool ret = objDistributed.Autentication(requestorInfo, ref message);
                    CloseChannel();
                    return ret;
                }
                catch (CommunicationException ex)
                {
                    new CustomLog().ProxyException(System.Reflection.MethodBase.GetCurrentMethod().Name, tryCon, true);
                    new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                    throw;
                }
                catch (Exception ex)
                {
                    new CustomLog().Exception(ex);
                    throw;
                }
            }

            return false;
        }

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