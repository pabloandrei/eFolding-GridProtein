using System;
using System.Linq;
using System.Collections;
using System.Data;
using GridProteinFolding.Data.XMLData.DAL;
using GridProteinFolding.Data.XMLData.Entity;

namespace GridProteinFolding.Data.XMLData.Bussines
{
    /// <summary>
    /// Summary description for Category.
    /// </summary>

    public class Actions
    {
        public static Process GetProcess(Guid guid)
        {
            DataRow iDr = null;
            iDr = XMLProcess.Select(guid.ToString());
            Process proc = null;
            if (iDr != null)
            {
                proc = new Process();

                proc.Guid = iDr[0] != DBNull.Value ? new Guid(iDr[0].ToString()) : new Guid();
                proc.Status = iDr[1] != DBNull.Value ? Convert.ToInt32(iDr[1]) : 0;
                proc.Date = iDr[2] != DBNull.Value ? Convert.ToDateTime(iDr[2]) : DateTime.MinValue;
            }
            return proc;
        }

        public static Process GetProcess(int[] status, string rowFilter)
        {
            DataRow iDr = null;
            iDr = XMLProcess.Select(status, rowFilter);
            Process proc = null;
            if (iDr != null)
            {
                proc = new Process();

                proc.Guid = iDr[0] != DBNull.Value ? new Guid(iDr[0].ToString()) : new Guid();
                proc.Status = iDr[1] != DBNull.Value ? Convert.ToInt32(iDr[1]) : 0;
                proc.Date = iDr[2] != DBNull.Value ? Convert.ToDateTime(iDr[2]) : DateTime.MinValue;
            }
            return proc;
        }

        public static Process GetOneProcess()
        {
            DataRow iDr = null;
            iDr = XMLProcess.Select();
            Process proc = null;
            if (iDr != null)
            {
                proc = new Process();
                proc.Guid = iDr[0] != DBNull.Value ? new Guid(iDr[0].ToString()) : new Guid();
                proc.Status = iDr[1] != DBNull.Value ? Convert.ToInt32(iDr[1]) : 0;
                proc.Date = iDr[2] != DBNull.Value ? Convert.ToDateTime(iDr[2]) : DateTime.MinValue;
            }
            return proc;
        }

        public static IList GetProcessList()
        {

            return XMLProcess.SelectAll();

        }

        public static void UpdateProcess(Process proc)
        {
            XMLProcess.Update(proc.Guid, proc.Status, proc.Date);
        }

        public static void InsertProcess(Process proc)
        {
            XMLProcess.Insert(proc.Guid, proc.Status, proc.Date);
        }

        public static void DeleteProcess(Guid guid)
        {
            XMLProcess.Delete(guid);
        }

        public static void Open(string pathXML)
        {
            XMLProcess.pathXML = pathXML;
            XMLProcess.OpenAndLoad();
        }

        public static void Close()
        {
            XMLProcess.Close();
        }
    }

}