using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Data.SQLite
{
    public class CustomLog : GridProteinFolding.Middle.Helpers.LoggingHelpers.Log
    {
        //public void PrintFormException(Exception ex)
        //{
        //    ConsoleColor oldColour = Console.ForegroundColor;
        //    GICO.ForegroundColor(ConsoleColor.Red);
        //    GICO.WriteLine(ex);
        //    GICO.ForegroundColor(oldColour);
        //    //new FormProtein().DisplayMessage(ex.Message.ToString());

        //    PrintConsole(ex);
        //}

        public void Exception(System.Exception ex)
        {
            base.Exception(ex, Types.ErrorLevel.None);
            //PrintFormException(ex);
        }



    }
}
