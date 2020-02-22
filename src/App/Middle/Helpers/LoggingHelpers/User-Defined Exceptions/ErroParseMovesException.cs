using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GridProteinFolding.Middle.Helpers.LoggingHelpers.User_Defined_Exceptions
{

    public class ErroParseMovesException : System.Exception
    {
        protected ErroParseMovesException(SerializationInfo info,
        StreamingContext context) : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }

        public ErroParseMovesException()
        {
        }

        public ErroParseMovesException(string message)
            : base(message)
        {
        }

        public ErroParseMovesException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
