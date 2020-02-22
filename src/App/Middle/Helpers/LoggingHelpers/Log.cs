using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net.Mail;
using GridProteinFolding.Middle.Helpers.LoggingHelpers.User_Defined_Exceptions;
using System.Globalization;
using log4net;
using log4net.Config;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Middle.Helpers.LoggingHelpers
{
    public class Log
    {
        private static readonly ILog log4Net = LogManager.GetLogger(typeof(Log));

        public static void InformationLog(string message)
        {
            log4Net.Warn(message);
        }

        private void Logginglog4Net(System.Exception ex, Types.ErrorLevel typeErrorLevel)
        {
            switch (typeErrorLevel)
            {
                case Types.ErrorLevel.Critical:
                    log4Net.Fatal(ex);
                    break;
                case Types.ErrorLevel.Error:
                    log4Net.Error(ex);
                    break;
                case Types.ErrorLevel.None:
                    //log4Net.Info(ex);
                    break;
                case Types.ErrorLevel.Warning:
                    log4Net.Warn(ex);
                    break;
                default:
                    log4Net.Error(ex);
                    break;
            }

        }
        public virtual void Exception(System.Exception ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }

        public virtual void CommunicationException(CommunicationException ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }

        public virtual void ArgumentOutOfRangeException(ArgumentOutOfRangeException ex, Types.ErrorLevel typeErrorLevel, bool isLatticeComputed)
        {
            if (ex.StackTrace.Contains("ThrowArgumentOutOfRangeException") && isLatticeComputed)
            {
                //Index was out of range. Must be non-negative and less than the size of the collection.
                return;
            }
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }

        public virtual void TimeoutException(TimeoutException ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }

        public virtual void SmtpException(SmtpException ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }

        public virtual void EndpointNotFoundException(EndpointNotFoundException ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex, printResult);
            return;
        }


        public virtual void ErrorNeighborTopological(ErrorNeighborTopological ex, Types.ErrorLevel typeErrorLevel)
        {
            //PrintException(ex);
            return;
        }

        public virtual void ErroParseMoves(ErroParseMovesException ex, Types.ErrorLevel typeErrorLevel)
        {
            Logginglog4Net(ex, typeErrorLevel);
            //PrintException(ex);
            return;
        }

        public virtual void PrintException(System.Exception ex)
        {
            Logginglog4Net(ex, Types.ErrorLevel.Error);
            //PrintConsole(ex, printResult);
        }

        public virtual void PrintConsole(System.Exception ex, bool printResult)
        {
            if (printResult)
            {
                Console.WriteLine("-------------------------------------------------/");
                Console.WriteLine("Exception @ {0}", DateTime.Now.ToString());
                Console.WriteLine(string.Format(Resource.Data.ToString() + ": {0}", ex.Data == null ? string.Empty : ex.Data.ToString()));
                Console.WriteLine(string.Format(Resource.Message.ToString() + ": {0}", ex.Message == null ? string.Empty : ex.Message.ToString()));
                Console.WriteLine(string.Format(Resource.Source.ToString() + ": {0}", ex.Source == null ? string.Empty : ex.Source.ToString()));
                Console.WriteLine(string.Format(Resource.StackTrace.ToString() + ": {0}", ex.StackTrace == null ? string.Empty : ex.StackTrace.ToString()));
                Console.WriteLine(string.Format(Resource.TargetSite.ToString() + ": {0}", ex.TargetSite == null ? string.Empty : ex.TargetSite.ToString()));
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public virtual void ProxyException(string GetCurrentMethod, int tentative, bool printResult)
        {
            if (printResult)
            {
                Console.WriteLine("GetCurrentMethod: {0}, tentative: {1}", GetCurrentMethod, tentative);

            }
        }
    }
}
