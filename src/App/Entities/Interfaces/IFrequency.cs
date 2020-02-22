using System;

namespace GridProteinFolding.Interfaces
{
    public interface IFrequency
    {
        double final { get; set; }
        IHistogram Histogram { get; set; }
        int histogram_id { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IHistogram> HistogramReference { get; set; }
        double initial { get; set; }
        int occurency { get; set; }
    }
}
