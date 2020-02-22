using System;
using System.ServiceModel;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using GridProteinFolding.ETL.HostETL.ServiceReferenceETL;

namespace GridProteinFolding.ETL.HostETL
{
    public class Proxy
    {
        //número de tentativas para consumo de serviços WCF
        private static int numberTry = 5;

        public Proxy()
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


        private static void OpenChannel()
        {
            objDistributed = new ServiceClient();
            objDistributed.Open();
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
            catch (CommunicationException)
            {
                // new CustomLog().CommunicationException(ex);
                // Communication exceptions are normal when
                // closing the connection.
                objDistributed.Abort();
            }
            catch (TimeoutException)
            {
                //new CustomLog().TimeoutException(ex, Types.ErrorLevel.Error, false);
                // Timeout exceptions are normal when closing
                // the connection.
                objDistributed.Abort();
            }
            catch (Exception)
            {
                //new CustomLog().Exception(ex, Types.ErrorLevel.Error, false);
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

        //        private static void CheckStatusChannel()
        //        {
        //#if DEBUGPROXY
        //            Console.WriteLine(HelperDebug.GetCurrentMethod());
        //#endif
        //            if (objDistributed == null)
        //            {
        //                objDistributed = new ServiceClient();
        //            }

        //            if (objDistributed.State == CommunicationState.Faulted)
        //            {
        //                //new CustomLog().CommunicationException(new CommunicationException("CommunicationState.Faulted"));
        //                OpenChannel();
        //            }
        //            else if (objDistributed.State != CommunicationState.Opened)
        //            {
        //                OpenChannel();
        //            }
        //        }


        public static ServiceParamWcf GetOneProcessGuiForETL(Guid guid)
        {
            System.Net.NetworkInformation.PhysicalAddress macAddr = Net.GetMacAddress();

            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    ServiceParamWcf ret = objDistributed.GetOneProcessGuiForETL(guid, new RequestorInfo() { macAddr = macAddr.ToString(), machineName = Environment.MachineName });
                    CloseChannel();
                    return ret;
                }
                catch (CommunicationException)
                {
                    throw;
                    //new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                }
                catch (Exception)
                {
                    //new CustomLog().Exception(ex);
                    throw;
                }
            }

            return null;
        }

        public static Guid[] GetGuidsToApplications()
        {
            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    Guid[] ret = objDistributed.GetGuidsToApplications();
                    CloseChannel();
                    return ret;
                }
                catch (CommunicationException)
                {
                    //new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                    throw;
                }
                catch (Exception)
                {
                    //new CustomLog().Exception(ex);
                    throw;
                }
            }

            return null;
        }

        public static void SetOneProcessByGuid(Guid guid, BasicEnums.State state)
        {
            System.Net.NetworkInformation.PhysicalAddress macAddr = Net.GetMacAddress();

            //CheckStatusChannel();

            for (int tryCon = 0; tryCon < numberTry; tryCon++)
            {
                try
                {
                    OpenChannel();
                    objDistributed.SetOneProcessByGuid(guid, state, new RequestorInfo() { macAddr = macAddr.ToString(), machineName = Environment.MachineName });
                    CloseChannel();
                    return;
                }
                catch (CommunicationException)
                {
                    //new CustomLog().CommunicationException(ex);
                    //CheckStatusChannel();
                    throw;
                }
                catch (Exception)
                {
                    //new CustomLog().Exception(ex);
                    throw;
                }
            }

            return;
        }

    }
}
