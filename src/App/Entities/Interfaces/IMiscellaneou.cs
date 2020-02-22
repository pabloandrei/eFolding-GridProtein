using System;

namespace GridProteinFolding.Interfaces
{
    public interface IMiscellaneou
    {
        bool executeExcel { get; set; }
        bool executeFake { get; set; }
        bool executeOrigin { get; set; }
        IProcess Process { get; set; }
        Guid process_guid { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IProcess> ProcessReference { get; set; }
    }
}
