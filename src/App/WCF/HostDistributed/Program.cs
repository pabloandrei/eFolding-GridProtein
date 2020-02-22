using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Description;
using GridProteinFolding.WCF.ServiceDistributed;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using System.Globalization;
using GridProteinFolding.Middle.Helpers.TypesHelpers;

namespace GridProteinFolding.WCF.HostDistributed
{
    class Program
    {
        static void Main(string[] args)
        {

            //Check Folders
            GridProteinFolding.WCF.ServiceDistributed.Service.CheckWorkFolders();

            //Run Hosts
            ServiceHost hostService = new ServiceHost(typeof(Service));
            ServiceHost hostDocumentManagment = new ServiceHost(typeof(DocumentManagment));
            //ServiceHost hostGeneratingResults = new ServiceHost(typeof(GeneratingResults));


            try
            {
                #region hostService
                hostService.Open();
                ServiceDescription serviceDesciption = hostService.Description;

                foreach (ServiceEndpoint endpoint in serviceDesciption.Endpoints)
                {
                    ConsoleColor oldColour = Console.ForegroundColor;
                    GICO.ForegroundColor(ConsoleColor.Red);
                    GICO.WriteLine(ExtendedString.Format("Endpoint - address:  {0}", endpoint.Address));
                    GICO.WriteLine(ExtendedString.Format("         - binding name:\t\t{0}", endpoint.Binding.Name));
                    GICO.WriteLine(ExtendedString.Format("         - contract name:\t\t{0}", endpoint.Contract.Name));
                    GICO.ForegroundColor(oldColour);
                }
                #endregion

                #region hostDocumentManagment
                hostDocumentManagment.Open();
                serviceDesciption = hostDocumentManagment.Description;

                foreach (ServiceEndpoint endpoint in serviceDesciption.Endpoints)
                {
                    ConsoleColor oldColour = Console.ForegroundColor;
                    GICO.ForegroundColor(ConsoleColor.Red);
                    GICO.WriteLine(ExtendedString.Format("Endpoint - address:  {0}", endpoint.Address));
                    GICO.WriteLine(ExtendedString.Format("         - binding name:\t\t{0}", endpoint.Binding.Name));
                    GICO.WriteLine(ExtendedString.Format("         - contract name:\t\t{0}", endpoint.Contract.Name));
                    GICO.WriteLine();
                    GICO.ForegroundColor(oldColour);
                }
                #endregion


                //#region hostGeneratingResults
                //hostGeneratingResults.Open();
                //serviceDesciption = hostGeneratingResults.Description;

                //foreach (ServiceEndpoint endpoint in serviceDesciption.Endpoints)
                //{
                //    ConsoleColor oldColour = Console.ForegroundColor;
                //    GICO.ForegroundColor(ConsoleColor.Red);
                //    GICO.WriteLine(String.Format("Endpoint - address:  {0}", endpoint.Address));
                //    GICO.WriteLine(String.Format("         - binding name:\t\t{0}", endpoint.Binding.Name));
                //    GICO.WriteLine(String.Format("         - contract name:\t\t{0}", endpoint.Contract.Name));
                //    GICO.WriteLine();
                //    GICO.ForegroundColor(oldColour);
                //}
                //#endregion
                GICO.WriteLine("Service Distributed: Service is up and running!");

                GICO.WriteLine();
                ConsoleKeyInfo key;

                Console.WriteLine("Press the Escape (Esc) key to quit: \n");
                do
                {
                    key = Console.ReadKey();
                } while (key.Key != ConsoleKey.Escape);


            }
            catch (Exception ex)
            {
                GICO.WriteLine(ex.Message.ToString());
                GICO.WriteLine(ex.InnerException.ToString());

                Console.ReadKey();
            }
            finally
            {

                if (hostService.State != CommunicationState.Closed && hostService.State != CommunicationState.Faulted)
                    hostService.Close();

                hostService = null;


                if (hostDocumentManagment.State != CommunicationState.Closed && hostDocumentManagment.State != CommunicationState.Faulted)
                    hostDocumentManagment.Close();

                hostDocumentManagment = null;

            }
        }


    }
}
