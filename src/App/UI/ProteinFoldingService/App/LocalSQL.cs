using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Data;

namespace GridProteinFolding.UI.ProteinFoldingService.App
{
    public class Sql
    {
        public static Data.XMLData.Entity.Process entityProcess;

        public static void Process()
        {
            entityProcess = new Data.XMLData.Entity.Process();
        }

        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            entityProcess = null;

            System.GC.SuppressFinalize(this);
        }

        public static void Open()
        {
            //Data.XMLData.Bussines.Actions.Open(AppDomain.CurrentDomain.BaseDirectory);
            Data.XMLData.Bussines.Actions.Open(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\efolding.fcfrp.usp.br\Client\");
        }

        public static void RecoverGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            Sql.entityProcess = SelectGuid(newParam);

            if (Sql.entityProcess == null)
            {
                InsertGuid(newParam, state);
                Sql.entityProcess = SelectGuid(newParam);
            }
        }

        public static void InsertGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            Data.XMLData.Bussines.Actions.InsertProcess(new Data.XMLData.Entity.Process() { Guid = newParam.dataToProcess.Guid, Status = Convert.ToByte(state), Date = DateTime.Now });
        }

        public static GridProteinFolding.Data.XMLData.Entity.Process SelectGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam)
        {
            Data.XMLData.Entity.Process process = GridProteinFolding.Data.XMLData.Bussines.Actions.GetProcess(newParam.dataToProcess.Guid);
            return process;
        }

        public static Data.XMLData.Entity.Process SelectPend()
        {
            //APLICAR O FILTRO
            int[] filter = new int[4];
            filter[0] = 3;
            filter[1] = 4;
            filter[2] = 5;
            filter[3] = 6;

            Data.XMLData.Entity.Process process = GridProteinFolding.Data.XMLData.Bussines.Actions.GetProcess(filter, "status");
            return process;
        }

        public static Data.XMLData.Entity.Process SetStatusProcessando(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            try
            {
                //Muda Status de Aguardando para EM PROCESSAMENTO no Cliente
                RecoverGuid(newParam, state);

                return SelectGuid(newParam);
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex, Types.ErrorLevel.Critical);
                throw;
            }
        }
    }
}
