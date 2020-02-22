using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GridProteinFolding.Middle.Helpers.LoggingHelpers.User_Defined_Exceptions
{
    public class ErrorNeighborTopological : System.Exception
    {
        protected ErrorNeighborTopological(SerializationInfo info,
        StreamingContext context) : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }

        public ErrorNeighborTopological()
        {
        }

        public ErrorNeighborTopological(string message)
            : base(message)
        {
        }

        public ErrorNeighborTopological(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
