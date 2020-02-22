using System;

namespace GridProteinFolding.Interfaces
{
    public interface ISeed
    {
        ICoordinate Coordinate { get; set; }
        System.Data.Objects.DataClasses.EntityReference<ICoordinate> CoordinateReference { get; set; }
        System.Data.Objects.DataClasses.EntityCollection<IHistogram> Histograms { get; set; }
        int isem { get; set; }
        INeighbor Neighbor { get; set; }
        System.Data.Objects.DataClasses.EntityReference<INeighbor> NeighborReference { get; set; }
        IProcess Process { get; set; }
        Guid process_guid { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IProcess> ProcessReference { get; set; }
    }
}
