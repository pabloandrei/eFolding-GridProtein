﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;

    public partial class GridProteinFoldingEntities : DbContext
    {
        public GridProteinFoldingEntities()
            : base("name=GridProteinFoldingEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<ClientCurrentVersion> ClientCurrentVersion { get; set; }
        public DbSet<ConfigApp> ConfigApp { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<DataToProcess> DataToProcess { get; set; }
        public DbSet<DataToResult> DataToResult { get; set; }
        public DbSet<Model> Model { get; set; }
        public DbSet<ModelType> ModelType { get; set; }
        public DbSet<Output> Output { get; set; }
        public DbSet<Process> Process { get; set; }
        public DbSet<ProcessLog> ProcessLog { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Targets> Targets { get; set; }
        public DbSet<TargetsCoordinates> TargetsCoordinates { get; set; }
        public DbSet<Blob> Blob { get; set; }

        public virtual int BunkFiles(string baseDir, string process_guid)
        {
            var baseDirParameter = baseDir != null ?
                new ObjectParameter("baseDir", baseDir) :
                new ObjectParameter("baseDir", typeof(string));

            var process_guidParameter = process_guid != null ?
                new ObjectParameter("process_guid", process_guid) :
                new ObjectParameter("process_guid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BunkFiles", baseDirParameter, process_guidParameter);
        }

        public virtual int BunkFiles02(string baseDir, string process_guid)
        {
            var baseDirParameter = baseDir != null ?
                new ObjectParameter("baseDir", baseDir) :
                new ObjectParameter("baseDir", typeof(string));

            var process_guidParameter = process_guid != null ?
                new ObjectParameter("process_guid", process_guid) :
                new ObjectParameter("process_guid", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BunkFiles02", baseDirParameter, process_guidParameter);
        }

        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));

            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }

        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));

            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }

        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }


        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));

            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));

            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }

        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
