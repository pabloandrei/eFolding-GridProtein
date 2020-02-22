using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using System.Net.Mail;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using ProteinFoldingService;

namespace GridProteinFolding.UI.ProteinFoldingService.App
{
    public class CustomLog : GridProteinFolding.Middle.Helpers.LoggingHelpers.Log
    {
        public void PrintFormException(Exception ex)
        {
            ConsoleColor oldColour = Console.ForegroundColor;
            GICO.ForegroundColor(ConsoleColor.Red);
            GICO.WriteLine(ex);
            GICO.ForegroundColor(oldColour);
            new FormProtein().DisplayMessage(ex.Message.ToString());

            PrintConsole(ex, true);
        }

        public void Exception(System.Exception ex)
        {
            base.Exception(ex, Types.ErrorLevel.Error);
            PrintFormException(ex);
        }

        public void CommunicationException(CommunicationException ex)
        {
            base.CommunicationException(ex, Types.ErrorLevel.Error);
            PrintFormException(ex);
        }

        public void ArgumentOutOfRangeException(ArgumentOutOfRangeException ex)
        {
            base.ArgumentOutOfRangeException(ex, Types.ErrorLevel.Error, false);
            PrintFormException(ex);
        }

        public void TimeoutException(TimeoutException ex)
        {
            base.TimeoutException(ex, Types.ErrorLevel.Error);
            PrintFormException(ex);
        }
        public void SmtpException(SmtpException ex)
        {
            base.SmtpException(ex, Types.ErrorLevel.Error);
            PrintFormException(ex);
        }

        public void EndpointNotFoundException(EndpointNotFoundException ex)
        {
            base.EndpointNotFoundException(ex, Types.ErrorLevel.Error);
            PrintFormException(ex);
        }

        public override void ProxyException(string GetCurrentMethod, int tentative, bool printResult)
        {
            base.ProxyException(GetCurrentMethod, tentative, printResult);
        }
    }
}
