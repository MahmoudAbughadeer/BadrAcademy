using Shared.Utilities;
using System;
using Shared.Models;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace DAL
{
    public class clsDepartmentData
    {
        public static async Task<int?> AddNewAsync(string departmentName, string description)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Departments (DepartmentName, Description)
                    VALUES (@DepartmentName, @Description);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);

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

        public static async Task<bool> UpdateAsync(int departmentId, string departmentName, string description)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Departments
                    SET DepartmentName = @DepartmentName,
                        Description = @Description
                    WHERE DepartmentID = @DepartmentID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);

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

        public static async Task<clsDepartmentDto> GetByDepartmentIdAsync(int? departmentID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT DepartmentID, DepartmentName, Description
                    FROM Departments
                    WHERE DepartmentID = @DepartmentID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsDepartmentDto()
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString(),
                                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString()
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

        public static async Task<clsDepartmentDto> GetByDepartmentNameAsync(string departmentName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT DepartmentID, DepartmentName, Description
                    FROM Departments
                    WHERE DepartmentName = @DepartmentName;", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsDepartmentDto()
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString(),
                                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString()
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
                    SELECT DepartmentID, DepartmentName, Description
                    FROM Departments
                    ORDER BY DepartmentID;", conn))
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

        public static async Task<bool> IsExistsAsync(string departmentName)
        {
            bool departmentExists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM Departments
                    WHERE DepartmentName = @DepartmentName
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    departmentExists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return departmentExists;
        }

        public static async Task<bool> DeleteAsync(int departmentId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Departments
                    WHERE DepartmentID = @DepartmentID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);

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
