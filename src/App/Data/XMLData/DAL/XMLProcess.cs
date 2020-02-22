using System;
using System.Data;
using System.Security;
using GridProteinFolding.Middle.Helpers.IOHelpers;

//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Data.XMLData.DAL
{
    public static class XMLProcess
    {
        //private XMLProcess() { }
        //public XMLProcess(string path)
        //{
        //    startupPath = path;

        //}
        static DataSet ds = new DataSet();
        static DataView dv = new DataView();
        public static string pathXML;
        private static string file = "Process.xml";
        /// <summary>
        /// Inserts a record into the Category table.
        /// </summary>
        /// 

        public static void VerifyIfExist(string pathXMLFile)
        {
            if (!File.Exists(pathXMLFile))
            {
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + file, pathXMLFile, true);
            }
        }

        public static void OpenAndLoad()
        {
            VerifyIfExist(pathXML + file);
            SelectAll();
        }

        /// <summary>
        /// Save the XML File
        /// </summary>
        public static void Save()
        {
            ds.WriteXml(pathXML + file, XmlWriteMode.WriteSchema);
        }

        /// <summary>
        /// Close connecting with XML File
        /// </summary>
        public static void Close()
        {
            ds.Dispose();
        }
        public static void Insert(Guid guid, int status, DateTime date)
        {
            DataRow dr = dv.Table.NewRow();
            dr[0] = guid;
            dr[1] = status;
            dr[2] = date;
            dv.Table.Rows.Add(dr);
            Save();
        }

        /// <summary>
        /// Updates a record in the Category table.
        /// </summary>
        public static void Update(Guid guid, int status, DateTime date)
        {
            DataRow dr = Select(guid.ToString());
            dr[1] = status;
            dr[2] = date;
            Save();
        }

        /// <summary>
        /// Deletes a record from the Category table by a composite primary key.
        /// </summary>
        public static void Delete(Guid guid)
        {
            dv.RowFilter = "guid='" + guid + "'";
            dv.Sort = "guid";
            dv.Delete(0);
            dv.RowFilter = "";
            Save();
        }


        /// <summary>
        /// Selects a single record from the Category table.
        /// </summary>
        public static DataRow Select(string guid)
        {
            dv.RowFilter = "guid='" + guid + "'";
            dv.Sort = "guid";
            DataRow dr = null;
            if (dv.Count > 0)
            {
                dr = dv[0].Row;
            }
            dv.RowFilter = "";
            return dr;
        }

        public static DataRow Select()
        {
            dv.RowFilter = "";
            dv.Sort = "guid";
            DataRow dr = null;
            if (dv.Count > 0)
            {

                dr = dv[0].Row;

            }
            dv.RowFilter = "";
            return dr;
        }

        public static DataRow Select(int[] status, string rowFilter)
        {
            foreach (int value in status)
            {
                dv.RowFilter = rowFilter + "=" + value.ToString();
                dv.Sort = "status";
                DataRow dr = null;

                if (dv.Count > 0)
                {

                    dr = dv[0].Row;

                }
                dv.RowFilter = "";

                if (dr != null)
                    return dr;
            }
            return null;
        }

        /// <summary>
        /// Selects all records from the Category table.
        /// </summary>
        public static DataView SelectAll()
        {
            ds.Clear();
            ds.ReadXml(pathXML + file, XmlReadMode.ReadSchema);
            dv = ds.Tables[0].DefaultView;
            return dv;
        }
    }
}
