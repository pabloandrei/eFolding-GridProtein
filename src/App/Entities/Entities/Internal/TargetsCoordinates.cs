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
    
    public partial class TargetsCoordinates
    {
        public int id { get; set; }
        public int targets_id { get; set; }
        public int value { get; set; }
    
        public virtual Targets Targets { get; set; }
    }
}
