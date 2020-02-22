using System;

namespace GridProteinFolding.Interfaces
{
    public interface IProcess
    {
        IDataToProcess DataToProcess { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IDataToProcess> DataToProcessReference { get; set; }
        DateTime date { get; set; }
        Guid guid { get; set; }
        IMiscellaneou Miscellaneou { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IMiscellaneou> MiscellaneouReference { get; set; }
        System.Data.Objects.DataClasses.EntityCollection<ISeed> Seeds { get; set; }
        IStatus Status { get; set; }
        byte status_id { get; set; }
        System.Data.Objects.DataClasses.EntityReference<IStatus> StatusReference { get; set; }
        Guid userId { get; set; }
    }
}
