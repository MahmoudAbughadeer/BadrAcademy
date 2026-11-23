using Microsoft.Data.Sqlite;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsAnswerEnvelopesData
    {
        public static async Task<DataTable> GetAllAsyncForFinal(int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            DataTable dt = new DataTable();

            try
            {
               using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
               using (SqliteCommand cmd = new SqliteCommand(@"
                        SELECT
                            ExamID,
                            SubjectName,
                            Level,
                            Department,
                            ExamDayName,
                            ExamDate,
                            TimeFrom,
                            TimeTo,
                            Period,
                            CommitteeNo,
                            HallName,
                            StudentsCount,
                            EnvelopeSeq,
                            TotalCommittees
                        FROM vw_AnswerEnvelopes
                        WHERE SettingID = @SettingID
                          AND (@LevelID IS NULL OR LevelID = @LevelID)
                          AND (@DepartmentID IS NULL OR DepartmentID = @DepartmentID)
                          AND (@SubjectID IS NULL OR SubjectID = @SubjectID)
                        ORDER BY ExamDate, TimeFrom, Level, Department, EnvelopeSeq;", conn))
                { 
                    cmd.Parameters.AddWithValue("@SettingID", settingID);
                    cmd.Parameters.AddWithValue("@LevelID", (object)levelID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartmentID", (object)departmentID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubjectID", (object)subjectID ?? DBNull.Value);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("ExamID", typeof(int));
                        dt.Columns.Add("SubjectName", typeof(string));
                        dt.Columns.Add("Level", typeof(string));
                        dt.Columns.Add("Department", typeof(string));
                        dt.Columns.Add("ExamDayName", typeof(string));
                        dt.Columns.Add("ExamDate", typeof(string));
                        dt.Columns.Add("TimeFrom", typeof(string));
                        dt.Columns.Add("TimeTo", typeof(string));
                        dt.Columns.Add("Period", typeof(string));
                        dt.Columns.Add("CommitteeNo", typeof(string));
                        dt.Columns.Add("HallName", typeof(string));
                        dt.Columns.Add("StudentsCount", typeof(int));
                        dt.Columns.Add("EnvelopeSeq", typeof(int));
                        dt.Columns.Add("TotalCommittees", typeof(int));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),  // ExamID
                                reader.GetString(1), // SubjectName
                                reader.GetString(2), // Level
                                reader.GetString(3), // Department
                                reader.GetString(4), // ExamDayName
                                reader.GetString(5), // ExamDate
                                clsFormat.TimeNoLeadingZero(reader.GetString(6)),
                                clsFormat.TimeNoLeadingZero(reader.GetString(7)),
                                reader.GetString(8),
                                reader.GetString(9),
                                reader.GetString(10),
                                reader.GetInt32(11),
                                reader.GetInt32(12),
                                reader.GetInt32(13)
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
        public static async Task<DataTable> GetAllAsyncForMidterm(int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                        SELECT
                            ExamID,
                            SubjectName,
                            Level,
                            Department,
                            ExamDayName,
                            ExamDate,
                            TimeFrom,
                            TimeTo,
                            Period,
                            CommitteeNo,
                            HallName,
                            StudentsCount,
                            EnvelopeSeq,
                            TotalCommittees
                        FROM vw_AnswerEnvelopesForMid
                        WHERE SettingID = @SettingID
                          AND (@LevelID IS NULL OR LevelID = @LevelID)
                          AND (@DepartmentID IS NULL OR DepartmentID = @DepartmentID)
                          AND (@SubjectID IS NULL OR SubjectID = @SubjectID)
                        ORDER BY ExamDate, TimeFrom, Level, Department, EnvelopeSeq;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);
                    cmd.Parameters.AddWithValue("@LevelID", (object)levelID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartmentID", (object)departmentID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubjectID", (object)subjectID ?? DBNull.Value);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("ExamID", typeof(int));
                        dt.Columns.Add("SubjectName", typeof(string));
                        dt.Columns.Add("Level", typeof(string));
                        dt.Columns.Add("Department", typeof(string));
                        dt.Columns.Add("ExamDayName", typeof(string));
                        dt.Columns.Add("ExamDate", typeof(string));
                        dt.Columns.Add("TimeFrom", typeof(string));
                        dt.Columns.Add("TimeTo", typeof(string));
                        dt.Columns.Add("Period", typeof(string));
                        dt.Columns.Add("CommitteeNo", typeof(string));
                        dt.Columns.Add("HallName", typeof(string));
                        dt.Columns.Add("StudentsCount", typeof(int));
                        dt.Columns.Add("EnvelopeSeq", typeof(int));
                        dt.Columns.Add("TotalCommittees", typeof(int));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),  // ExamID
                                reader.GetString(1), // SubjectName
                                reader.GetString(2), // Level
                                reader.GetString(3), // Department
                                reader.GetString(4), // ExamDayName
                                reader.GetString(5), // ExamDate
                                clsFormat.TimeNoLeadingZero(reader.GetString(6)),
                                clsFormat.TimeNoLeadingZero(reader.GetString(7)),
                                reader.GetString(8),
                                reader.GetString(9),
                                reader.GetString(10),
                                reader.GetInt32(11),
                                reader.GetInt32(12),
                                reader.GetInt32(13)
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
    }
}
