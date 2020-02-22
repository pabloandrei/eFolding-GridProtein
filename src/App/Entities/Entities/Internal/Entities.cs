using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Entities.Internal
{
    public partial class Process
    {
        public string StatusDescription { get { return Status.description; } }
    }

    public partial class ProcessLog
    {
        public string StatusDescription { get { return status_id.ToString(); } }
    }
}
