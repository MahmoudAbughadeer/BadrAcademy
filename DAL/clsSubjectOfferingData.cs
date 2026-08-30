using Shared.Models;
using Shared.Utilities;
using System;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class clsSubjectOfferingData
    {
        public static async Task<int?> AddNewAsync(int subjectId, int levelId, int departmentId, string semester)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO SubjectOfferings (SubjectID, LevelID, DepartmentID, Semester)
                    VALUES (@SubjectID, @LevelID, @DepartmentID, @Semester);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@Semester", semester);

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

        public static async Task<bool> UpdateAsync(int offeringId, int subjectId, int levelId, int departmentId, string semester)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE SubjectOfferings
                    SET SubjectID    = @SubjectID,
                        LevelID      = @LevelID,
                        DepartmentID = @DepartmentID,
                        Semester     = @Semester
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@Semester", semester);

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

        public static async Task<clsSubjectOfferingDto> GetByOfferingIdAsync(int? offeringID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT OfferingID, SubjectID, LevelID, DepartmentID, Semester
                    FROM SubjectOfferings
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsSubjectOfferingDto()
                            {
                                OfferingID = Convert.ToInt32(reader["OfferingID"]),
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                LevelID = Convert.ToInt32(reader["LevelID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                Semester = reader["Semester"].ToString()
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

        // Raw IDs — for internal/BL use
        public static async Task<DataTable> GetAllAsync(int pageNumber, int rowsPerPage)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT OfferingID, SubjectID, LevelID, DepartmentID, Semester
                    FROM SubjectOfferings
                    ORDER BY OfferingID
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

        // Resolved names — for UI display (grids, dropdowns, etc.)
        public static async Task<DataTable> GetAllWithNamesAsync(int pageNumber, int rowsPerPage)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT
                        so.OfferingID,
                        s.SubjectName,
                        lv.LevelName,
                        dp.DepartmentName,
                        so.Semester
                    FROM SubjectOfferings so
                    JOIN Subjects s    ON s.SubjectID       = so.SubjectID
                    JOIN Levels lv     ON lv.LevelID        = so.LevelID
                    JOIN Departments dp ON dp.DepartmentID  = so.DepartmentID
                    ORDER BY so.OfferingID
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

        // Filter by Level + Department + Semester — main use case for building exam schedule UI
        public static async Task<DataTable> GetByLevelDepartmentSemesterAsync(int levelId, int departmentId, string semester)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT
                        so.OfferingID,
                        s.SubjectName,
                        lv.LevelName,
                        dp.DepartmentName,
                        so.Semester
                    FROM SubjectOfferings so
                    JOIN Subjects s     ON s.SubjectID      = so.SubjectID
                    JOIN Levels lv      ON lv.LevelID       = so.LevelID
                    JOIN Departments dp ON dp.DepartmentID  = so.DepartmentID
                    WHERE so.LevelID      = @LevelID
                      AND so.DepartmentID = @DepartmentID
                      AND so.Semester     = @Semester
                    ORDER BY s.SubjectName;", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@Semester", semester);

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

        public static async Task<bool> IsExistsAsync(int subjectId, int levelId, int departmentId, string semester)
        {
            bool exists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM SubjectOfferings
                    WHERE SubjectID    = @SubjectID
                      AND LevelID      = @LevelID
                      AND DepartmentID = @DepartmentID
                      AND Semester     = @Semester
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@Semester", semester);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    exists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return exists;
        }

        public static async Task<bool> DeleteAsync(int offeringId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM SubjectOfferings
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);

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
