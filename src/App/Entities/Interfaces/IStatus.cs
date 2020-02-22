using System;

namespace GridProteinFolding.Interfaces
{
    public interface IStatus
    {
        string description { get; set; }
        byte id { get; set; }
        System.Data.Objects.DataClasses.EntityCollection<IProcess> Processes { get; set; }
    }
}
