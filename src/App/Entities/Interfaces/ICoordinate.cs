using System;
namespace GridProteinFolding.Interfaces
{
    public interface ICoordinate
    {
        ISeed Seed { get; set; }
        int seed_isem { get; set; }
        System.Data.Objects.DataClasses.EntityReference<ISeed> SeedReference { get; set; }
        int x { get; set; }
        int y { get; set; }
        int z { get; set; }
    }
}
