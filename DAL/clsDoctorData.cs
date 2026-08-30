using Microsoft.Data.Sqlite;
using Shared.Models;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsDoctorData
    {
        public static async Task<int?> AddNewAsync(string doctorName, int departmentId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Doctors (DoctorName, DepartmentID)
                    VALUES (@DoctorName, @DepartmentID);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);

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

        public static async Task<bool> UpdateAsync(int doctorId, string doctorName, int departmentId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Doctors
                    SET DoctorName = @DoctorName,
                        DepartmentID = @DepartmentID
                    WHERE DoctorID = @DoctorID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                    cmd.Parameters.AddWithValue("@DoctorName", doctorName);
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

        public static async Task<clsDoctorDto> GetByDoctorIdAsync(int? doctorID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT DoctorID, DoctorName, DepartmentID
                    FROM Doctors
                    WHERE DoctorID = @DoctorID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsDoctorDto()
                            {
                                DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                DoctorName = reader["DoctorName"].ToString(),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"])
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
                    SELECT DoctorID, DoctorName, dm.DepartmentName
                    FROM Doctors d
                    JOIN Departments dm ON d.DepartmentID = dm.DepartmentID
                    ORDER BY DoctorID;", conn))
                {
                    await conn.OpenAsync();

                    //using (SqliteDataReader reader = await cmd.ExecuteReaderAsync())
                    //    dt.Load(reader);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("DoctorID", typeof(long));
                        dt.Columns.Add("DoctorName", typeof(string));
                        dt.Columns.Add("DepartmentName", typeof(string));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return dt;
        }

        public static async Task<bool> DeleteAsync(int doctorId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Doctors
                    WHERE DoctorID = @DoctorID;", conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorID", doctorId);

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
