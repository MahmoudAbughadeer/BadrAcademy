using Shared.Models;
using Shared.Utilities;
using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
namespace DAL
{
    public class clsExamScheduleData
    {
        public static async Task<int?> AddNewAsync(int offeringId, string examDayName, DateTime examDate,
            float timeFrom, float timeTo, string period, int settingId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO ExamSchedule (OfferingID, ExamDayName, ExamDate, TimeFrom, TimeTo, Period, SettingID)
                    VALUES (@OfferingID, @ExamDayName, @ExamDate, @TimeFrom, @TimeTo, @Period, @SettingID);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                    cmd.Parameters.AddWithValue("@ExamDayName", examDayName);
                    cmd.Parameters.AddWithValue("@ExamDate", examDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@TimeFrom", timeFrom);
                    cmd.Parameters.AddWithValue("@TimeTo", timeTo);
                    cmd.Parameters.AddWithValue("@Period", period);
                    cmd.Parameters.AddWithValue("@SettingID", settingId);

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

        public static async Task<bool> UpdateAsync(int examId, int offeringId, string examDayName, DateTime examDate,
            float timeFrom, float timeTo, string period, int settingId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE ExamSchedule
                    SET OfferingID = @OfferingID,
                        ExamDayName = @ExamDayName,
                        ExamDate = @ExamDate,
                        TimeFrom = @TimeFrom,
                        TimeTo = @TimeTo,
                        Period = @Period,
                        SettingID = @SettingID
                    WHERE ExamID = @ExamID;", conn))
                {
                    cmd.Parameters.AddWithValue("@ExamID", examId);
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                    cmd.Parameters.AddWithValue("@ExamDayName", examDayName);
                    cmd.Parameters.AddWithValue("@ExamDate", examDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@TimeFrom", timeFrom);
                    cmd.Parameters.AddWithValue("@TimeTo", timeTo);
                    cmd.Parameters.AddWithValue("@Period", period);
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

        public static async Task<clsExamScheduleDto> GetByExamIdAsync(int? examID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT ExamID, OfferingID, ExamDayName, ExamDate, TimeFrom, TimeTo, Period, SettingID
                    FROM ExamSchedule
                    WHERE ExamID = @ExamID;", conn))
                {
                    cmd.Parameters.AddWithValue("@ExamID", examID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsExamScheduleDto()
                            {
                                ExamID = Convert.ToInt32(reader["ExamID"]),
                                OfferingID = Convert.ToInt32(reader["OfferingID"]),
                                ExamDayName = reader["ExamDayName"].ToString(),
                                ExamDate = DateTime.ParseExact(reader["ExamDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                TimeFrom = Convert.ToSingle(reader["TimeFrom"]),
                                TimeTo = Convert.ToSingle(reader["TimeTo"]),
                                Period = reader["Period"].ToString(),
                                SettingID = Convert.ToInt32(reader["SettingID"]),
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

        public static async Task<clsExamScheduleDto> GetByOfferingIdAsync(int? offeringID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT ExamID, OfferingID, ExamDayName, ExamDate, TimeFrom, TimeTo, Period, SettingID
                    FROM ExamSchedule
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsExamScheduleDto()
                            {
                                ExamID = Convert.ToInt32(reader["ExamID"]),
                                OfferingID = Convert.ToInt32(reader["OfferingID"]),
                                ExamDayName = reader["ExamDayName"].ToString(),
                                ExamDate = DateTime.ParseExact(reader["ExamDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                TimeFrom = Convert.ToSingle(reader["TimeFrom"]),
                                TimeTo = Convert.ToSingle(reader["TimeTo"]),
                                Period = reader["Period"].ToString(),
                                SettingID = Convert.ToInt32(reader["SettingID"]),
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
                    SELECT ExamID, OfferingID, ExamDayName, ExamDate, TimeFrom, TimeTo, Period, SettingID
                    FROM ExamSchedule
                    ORDER BY ExamID", conn))
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

        public static async Task<DataTable> GetExamScheduleDetailsAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                   SELECT
                        es.ExamID,
                    	es.SettingID,
                        es.OfferingID,
                    	l.LevelName,
                    	d.DepartmentName,
                    	s.SubjectName,
                    	doc.DoctorName,
                    	es.ExamDate,
                    	es.ExamDayName,
                    	es.TimeFrom,
                    	es.TimeTo,
                    	es.Period
                   FROM ExamSchedule es
                   JOIN SubjectOfferings so ON es.OfferingID = so.OfferingID
                   JOIN Levels l 			 ON so.LevelID = l.LevelID
                   JOIN Departments d 	     ON so.DepartmentID = d.DepartmentID
                   JOIN Subjects s          ON so.SubjectID = s.SubjectID
                   JOIN Doctors doc	     ON so.DoctorID = doc.DoctorID
                   ORDER BY es.ExamDate;", conn))
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

        public static async Task<bool> DeleteAsync(int examId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM ExamSchedule
                    WHERE ExamID = @ExamID;", conn))
                {
                    cmd.Parameters.AddWithValue("@ExamID", examId);

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

        public static async Task<bool> DeleteByOfferingIDAsync(int offeringID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM ExamSchedule
                    WHERE OfferingID = @OfferingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@OfferingID", offeringID);

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
