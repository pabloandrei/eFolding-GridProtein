using System;

namespace GridProteinFolding.Interfaces
{
    public interface IHistogram
    {
        IFrequency Frequency { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IFrequency> FrequencyReference { get; set; }
        int id { get; set; }
        int lenght { get; set; }
        double max { get; set; }
        double medium { get; set; }
        double min { get; set; }
        double n { get; set; }
        ISeed Seed { get; set; }
        int seed_isem { get; set; }
        System.Data.Objects.DataClasses.EntityReference<ISeed> SeedReference { get; set; }
        double sum { get; set; }
        float type { get; set; }
    }
}
