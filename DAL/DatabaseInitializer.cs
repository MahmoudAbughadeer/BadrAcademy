
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace DAL
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (SqliteConnection con = new SqliteConnection(clsDataAccessSettings.ConnectionString))
            {
                con.Open();

                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "ControlDB_SQLite.sql");

                string script = File.ReadAllText(scriptPath);

                using (SqliteCommand cmd = new SqliteCommand(script, con))
                    cmd.ExecuteNonQuery();

            }
        }
    }
}
