using System;

namespace GridProteinFolding.Interfaces
{
    public interface INeighbor
    {
        ISeed Seed { get; set; }
        int seed_isem { get; set; }
        System.Data.Objects.DataClasses.EntityReference<ISeed> SeedReference { get; set; }
        float xMinus { get; set; }
        float xPlus { get; set; }
        float yMinus { get; set; }
        float yPlus { get; set; }
        float zMinus { get; set; }
        float zPlus { get; set; }
    }
}
