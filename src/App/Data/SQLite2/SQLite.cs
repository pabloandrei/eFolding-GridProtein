using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Globalization;

using EntityProcess = GridProteinFolding.Data.SQLite.Entities.Process;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Data.SQLite
{
    public static class SQLite
    {
        static SQLiteConnection m_dbConnection;
        public const string fileName = "GridProteinFolding.Data.SQLite";

        /// <summary>
        /// Creates an empty database file
        /// </summary>
        private static void CreateNewDatabase(string baseDirectory)
        {
            string file = baseDirectory + @"\" + fileName;

            if (!File.Exists(file))
                SQLiteConnection.CreateFile(file);
        }

        /// <summary>
        /// Creates a connection with our database file.
        /// </summary>
        public static void ConnectToDatabase(string baseDirectory)
        {
            try
            {
                CreateNewDatabase(baseDirectory);

                m_dbConnection = new SQLiteConnection("Data Source=" + baseDirectory + @"\" + fileName + ";Version=3;");
                m_dbConnection.Open();

                //GICO.WriteLine("Connected to Database...");

                IsTableExists();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Close a connection with our database file.
        /// </summary>
        public static void CloseDatabase()
        {
            m_dbConnection.Clone();
        }

        private static void IsTableExists()
        {
            string query = "SELECT name FROM sqlite_master WHERE name='Process'";

            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);

            if (command.ExecuteScalar() == null)
            {
                CreateTable();

            }

        }

        /// <summary>
        /// Creates a table
        /// </summary>
        private static bool CreateTable()
        {
            try
            {
                string sql = "CREATE TABLE Process (guid VARCHAR(255) PRIMARY KEY, status INT, date VARCHAR(19) )";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                return true;
            }
            catch (SQLiteException ex)
            {
                new CustomLog().Exception(ex);
                GICO.WriteLine("TABLE Process exist!");
                return false;
            }
        }

        /// <summary>
        /// Drop a table
        /// </summary>
        private static bool DropTable()
        {
            try
            {
                string sql = "DROP TABLE Process";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                return true;
            }
            catch (SQLiteException ex)
            {
                new CustomLog().Exception(ex);
                GICO.WriteLine("TABLE Process not exist!");
                return false;
            }
        }

        /// <summary>
        /// Inserts some values
        /// </summary>
        public static void ExecuteInsert(EntityProcess entityProcess)
        {
            Guid guid = entityProcess.guid;
            int status = entityProcess.status;
            DateTime date = entityProcess.date;

            string sql = "INSERT INTO Process (guid, status, date) VALUES ('" + guid.ToString() + "', " + status + ", '" + date.ToString("yyyy-MM-dd hh:mm:ss") + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Inserts some values
        /// </summary>
        public static void ExecuteUpdate(EntityProcess entityProcess)
        {
            Guid guid = entityProcess.guid;
            int status = entityProcess.status;
            DateTime? date = entityProcess.date;

            string sql = "UPDATE Process SET status=" + status + " WHERE guid='" + guid.ToString() + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Writes of console (for test)
        /// </summary>
        public static List<EntityProcess> ExecuteReader(string query)
        {
            List<EntityProcess> entityProcess = new List<EntityProcess>();

            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Guid guid = Guid.Parse(reader["guid"].ToString());
                int status = int.Parse(reader["status"].ToString());
                DateTime date;
                try
                {
                    date = DateTime.Parse(reader["date"].ToString());
                }
                catch
                {
                    date = DateTime.Now;
                }


                entityProcess.Add(new EntityProcess() { date = date, guid = guid, status = status });

            }

            return entityProcess;
        }
    }
}
