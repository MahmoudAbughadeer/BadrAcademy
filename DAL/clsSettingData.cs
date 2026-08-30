using Shared.Models;
using Shared.Utilities;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DAL
{
    public class clsSettingData
    {
        public static async Task<int?> AddNewAsync(string academicYear, string semester, string semesterType, string defaultAnswerForms)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Settings (AcademicYear, Semester, SemesterType, DefaultAnswerForms)
                    VALUES (@AcademicYear, @Semester, @SemesterType, @DefaultAnswerForms);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);
                    cmd.Parameters.AddWithValue("@Semester", semester);
                    cmd.Parameters.AddWithValue("@SemesterType", semesterType);
                    cmd.Parameters.AddWithValue("@DefaultAnswerForms", defaultAnswerForms);

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

        public static async Task<bool> UpdateAsync(int settingId, string academicYear, string semester, string semesterType, string defaultAnswerForms)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Settings
                    SET AcademicYear = @AcademicYear,
                        Semester = @Semester,
                        SemesterType = @SemesterType,
                        DefaultAnswerForms = @DefaultAnswerForms
                    WHERE SettingID = @SettingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);
                    cmd.Parameters.AddWithValue("@Semester", semester);
                    cmd.Parameters.AddWithValue("@SemesterType", semesterType);
                    cmd.Parameters.AddWithValue("@DefaultAnswerForms", defaultAnswerForms);

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

        public static async Task<clsSettingDto> GetBySettingIdAsync(int? settingID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT SettingID, AcademicYear, Semester, SemesterType, DefaultAnswerForms
                    FROM Settings
                    WHERE SettingID = @SettingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsSettingDto()
                            {
                                SettingID = Convert.ToInt32(reader["SettingID"]),
                                AcademicYear = reader["AcademicYear"].ToString(),
                                Semester = reader["Semester"].ToString(),
                                SemesterType = reader["SemesterType"].ToString(),
                                DefaultAnswerForms = reader["DefaultAnswerForms"].ToString()
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
                    SELECT SettingID, AcademicYear, Semester, SemesterType, DefaultAnswerForms
                    FROM Settings
                    ORDER BY SettingID
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

        public static async Task<bool> IsExistsAsync(int settingId)
        {
            bool settingExists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM Settings
                    WHERE SettingID = @SettingID
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingId);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    settingExists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return settingExists;
        }

        public static async Task<bool> DeleteAsync(int settingId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Settings
                    WHERE SettingID = @SettingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingId);

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
