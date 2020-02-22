using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.LoggingHelpers.Exception
{
    /// <summary>
    /// Severity level of Exception
    /// </summary>
    internal enum SeverityLevel
    {
        Fatal,
        Critical,
        Information
    }
    /// <summary>
    /// Log level of Exception
    /// </summary>
    internal enum LogLevel
    {
        Debug,
        Event
    }
}
