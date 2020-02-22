using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
//using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
//using Microsoft.Practices.Unity;
using System.Configuration;
using System.Diagnostics;
using System.Security;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.LoggingHelpers
{
    public static class LoggingHelper
    {
        public static bool logarErro = Convert.ToBoolean(ConfigurationManager.AppSettings["LogarErro"]);

        //private static void ConfigureEnterpriseLibraryContainer()
        //{
        //var builder = new ConfigurationSourceBuilder();
        //builder.ConfigureInstrumentation().EnableLogging();
        //builder.ConfigureLogging().WithOptions
        //       .LogToCategoryNamed("General")
        //         .WithOptions
        //         .SetAsDefaultCategory()
        //         .SendTo
        //         .EventLog("Log Event")
        //         .FormatWith(new FormatterBuilder()
        //         .TextFormatterNamed("Textformatter"))
        //         .LogToCategoryNamed("Application")                     
        //         .WithOptions.ToSourceLevels(System.Diagnostics.SourceLevels.Error);                  


        //var configSource = new DictionaryConfigurationSource();
        //builder.UpdateConfigurationWithReplace(configSource);
        //EnterpriseLibraryContainer.Current =
        //EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
        //}

        public static void WriteLogEventLog(System.Exception ex)
        {
            //try
            //{
            //    Logger.Write(ex.Message+ex.StackTrace);
            //}
            //catch (Exception ex1)
            //{
            //    throw new Exception(ex1.Message);
            //}

            if (ex != null)
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "GridProteinFolding";
                sLog = "Application";
                sEvent = ex.Message + ex.StackTrace;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent,
                    EventLogEntryType.Error);
            }

        }

        static LoggingHelper()
        {
            //ConfigureEnterpriseLibraryContainer();
        }
    }
}