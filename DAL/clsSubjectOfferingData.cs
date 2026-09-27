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
        public static async Task<int?> AddNewAsync(int subjectId, int levelId, int departmentId, int doctorId, string semester)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO SubjectOfferings (SubjectID, LevelID, DepartmentID, DoctorID, Semester)
                    VALUES (@SubjectID, @LevelID, @DepartmentID, @DoctorID, @Semester);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@DoctorID", doctorId);
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

        public static async Task<bool> UpdateAsync(int offeringId, int subjectId, int levelId, int departmentId, int doctorID, string semester)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE SubjectOfferings
                    SET SubjectID    = @SubjectID,
                        LevelID      = @LevelID,
                        DepartmentID = @DepartmentID,
                        DoctorID     = @DoctorID,
                        Semester     = @Semester
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);
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
                    SELECT OfferingID, SubjectID, LevelID, DepartmentID, DoctorID, Semester
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
                                DoctorID = Convert.ToInt32(reader["DoctorID"]),
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

        public static async Task<DataTable> GetAllWithNamesAsync()
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
                        d.DoctorName,
                        so.Semester
                    FROM SubjectOfferings so
                    JOIN Subjects s     ON s.SubjectID       = so.SubjectID
                    JOIN Levels lv      ON lv.LevelID        = so.LevelID
                    JOIN Departments dp ON dp.DepartmentID   = so.DepartmentID
                    JOIN Doctors d      ON d.DoctorID        = so.DepartmentID
                    ORDER BY so.OfferingID", conn))
                {
                    await conn.OpenAsync();

                    using (SqliteDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("OfferingID", typeof(long));
                        dt.Columns.Add("SubjectName", typeof(string));
                        dt.Columns.Add("LevelName", typeof(string));
                        dt.Columns.Add("DepartmentName", typeof(string));
                        dt.Columns.Add("DoctorName", typeof(string));
                        dt.Columns.Add("Semester", typeof(string));

                        while(await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt64(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5)
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
