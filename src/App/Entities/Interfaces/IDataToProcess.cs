using System;

namespace GridProteinFolding.Interfaces
{
    public interface IDataToProcess
    {
        bool calcDelta { get; set; }
        long isem { get; set; }
        bool loadDatFile { get; set; }
        int maxInterations { get; set; }
        double numberOfDelta { get; set; }
        IProcess Process { get; set; }
        Guid process_guid { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IProcess> ProcessReference { get; set; }
        int totalSitio { get; set; }
    }
}
