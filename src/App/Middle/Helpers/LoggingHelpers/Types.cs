using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.LoggingHelpers
{
    /// <summary>
    /// Nível de erro ou exceptions
    /// </summary>
    public class Types
    {
        public enum ErrorLevel { None = 0, Warning = 1, Error = 2, Critical = 3 };
    }
}
