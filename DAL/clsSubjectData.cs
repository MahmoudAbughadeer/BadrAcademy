using Shared.Models;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace DAL
{
    public class clsSubjectData
    {
        public static async Task<int?> AddNewAsync(string subjectName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Subjects (SubjectName)
                    VALUES (@SubjectName);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectName", subjectName);

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

        public static async Task<bool> UpdateAsync(int subjectId, string subjectName)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Subjects
                    SET SubjectName = @SubjectName
                    WHERE SubjectID = @SubjectID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@SubjectName", subjectName);

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

        public static async Task<clsSubjectDto> GetBySubjectIdAsync(int? subjectID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT SubjectID, SubjectName
                    FROM Subjects
                    WHERE SubjectID = @SubjectID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsSubjectDto()
                            {
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                SubjectName = reader["SubjectName"].ToString(),
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
                    SELECT SubjectID, SubjectName
                    FROM Subjects
                    ORDER BY SubjectID", conn))
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
        public static async Task<DataTable> GetAllByDepartmentIdAsync(int? departmentID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT DISTINCT s.SubjectID, s.SubjectName
                    FROM Subjects s
                    JOIN SubjectOfferings so
                    ON s.SubjectID = so.SubjectID
                    WHERE @DepartmentID IS NULL OR so.DepartmentID = @DepartmentID
                    ORDER BY s.SubjectID", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", (object)departmentID ?? DBNull.Value);
                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("SubjectID", typeof(int));
                        dt.Columns.Add("SubjectName", typeof(string));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            );
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
        public static async Task<bool> DeleteAsync(int subjectId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Subjects
                    WHERE SubjectID = @SubjectID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);

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
