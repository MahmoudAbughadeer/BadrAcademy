using Shared.Utilities;
using System;
using Shared.Models;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace DAL
{
    public class clsLevelData
    {
        public static async Task<int?> AddNewAsync(string levelName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Levels (LevelName)
                    VALUES (@LevelName);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelName", levelName);

                    await conn.OpenAsync();

                    object result = await cmd.ExecuteScalarAsync();

                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }
        }

        public static async Task<bool> UpdateAsync(int levelId, string levelName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Levels
                    SET LevelName = @LevelName
                    WHERE LevelID = @LevelID;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@LevelName", levelName);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }
        }

        public static async Task<clsLevelDto> GetByLevelIdAsync(int? levelID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT LevelID, LevelName
                    FROM Levels
                    WHERE LevelID = @LevelID;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelID", levelID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsLevelDto()
                            {
                                LevelID = Convert.ToInt32(reader["LevelID"]),
                                LevelName = reader["LevelName"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }
        }

        public static async Task<clsLevelDto> GetByLevelNameAsync(string levelName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT LevelID, LevelName
                    FROM Levels
                    WHERE LevelName = @LevelName;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelName", levelName);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsLevelDto()
                            {
                                LevelID = Convert.ToInt32(reader["LevelID"]),
                                LevelName = reader["LevelName"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }
        }

        public static async Task<DataTable> GetAllAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT LevelID, LevelName
                    FROM Levels
                    ORDER BY LevelID;", conn))
                {
                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return dt;
        }

        public static async Task<bool> IsExistsAsync(string levelName)
        {
            bool levelExists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM Levels
                    WHERE LevelName = @LevelName
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelName", levelName);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    levelExists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return levelExists;
        }

        public static async Task<bool> DeleteAsync(int levelId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Levels
                    WHERE LevelID = @LevelID;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelID", levelId);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
