//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GridProteinFolding.Entities.Internal
{
    using System;
    using System.Collections.Generic;
    
    public partial class Configuration
    {
        public long id { get; set; }
        public System.Guid process_guid { get; set; }
        public int mcStep { get; set; }
        public int order { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    
        public virtual Process Process { get; set; }
    }
}
