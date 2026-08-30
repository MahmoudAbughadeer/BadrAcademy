using Shared.Models;
using Shared.Utilities;
using System;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class clsHallData
    {
        public static async Task<int?> AddNewAsync(string hallName, string hallType, int defaultCapacity, bool isActive)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Halls (HallName, HallType, DefaultCapacity, IsActive)
                    VALUES (@HallName, @HallType, @DefaultCapacity, @IsActive);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@HallName", hallName);
                    cmd.Parameters.AddWithValue("@HallType", hallType);
                    cmd.Parameters.AddWithValue("@DefaultCapacity", defaultCapacity);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

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

        public static async Task<bool> UpdateAsync(int hallId, string hallName, string hallType, int defaultCapacity, bool isActive)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Halls
                    SET HallName = @HallName,
                        HallType = @HallType,
                        DefaultCapacity = @DefaultCapacity,
                        IsActive = @IsActive
                    WHERE HallID = @HallID;", conn))
                {
                    cmd.Parameters.AddWithValue("@HallID", hallId);
                    cmd.Parameters.AddWithValue("@HallName", hallName);
                    cmd.Parameters.AddWithValue("@HallType", hallType);
                    cmd.Parameters.AddWithValue("@DefaultCapacity", defaultCapacity);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

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

        public static async Task<clsHallDto> GetByHallIdAsync(int? hallID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT HallID, HallName, HallType, DefaultCapacity, IsActive
                    FROM Halls
                    WHERE HallID = @HallID;", conn))
                {
                    cmd.Parameters.AddWithValue("@HallID", hallID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsHallDto()
                            {
                                HallID = Convert.ToInt32(reader["HallID"]),
                                HallName = reader["HallName"].ToString(),
                                HallType = reader["HallType"].ToString(),
                                DefaultCapacity = Convert.ToInt32(reader["DefaultCapacity"]),
                                IsActive = Convert.ToInt64(reader["IsActive"]) == 1
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

        public static async Task<clsHallDto> GetByHallNameAsync(string hallName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT HallID, HallName, HallType, DefaultCapacity, IsActive
                    FROM Halls
                    WHERE HallName = @HallName;", conn))
                {
                    cmd.Parameters.AddWithValue("@HallName", hallName);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsHallDto()
                            {
                                HallID = Convert.ToInt32(reader["HallID"]),
                                HallName = reader["HallName"].ToString(),
                                HallType = reader["HallType"].ToString(),
                                DefaultCapacity = Convert.ToInt32(reader["DefaultCapacity"]),
                                IsActive = Convert.ToInt64(reader["IsActive"]) == 1
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

        public static async Task<DataTable> GetAllAsync(int pageNumber, int rowsPerPage)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT HallID, HallName, HallType, DefaultCapacity, IsActive
                    FROM Halls
                    ORDER BY HallID
                    LIMIT @RowsPerPage OFFSET @Offset;", conn))
                {
                    int offset = (pageNumber - 1) * rowsPerPage;
                    cmd.Parameters.AddWithValue("@RowsPerPage", rowsPerPage);
                    cmd.Parameters.AddWithValue("@Offset", offset);

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

        public static async Task<DataTable> GetAllActiveAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT HallID, HallName, HallType, DefaultCapacity, IsActive
                    FROM Halls
                    WHERE IsActive = 1
                    ORDER BY HallName;", conn))
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

        public static async Task<bool> IsExistsAsync(string hallName)
        {
            bool hallExists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM Halls
                    WHERE HallName = @HallName
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@HallName", hallName);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    hallExists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return hallExists;
        }

        public static async Task<bool> DeleteAsync(int hallId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Halls
                    WHERE HallID = @HallID;", conn))
                {
                    cmd.Parameters.AddWithValue("@HallID", hallId);

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
            
