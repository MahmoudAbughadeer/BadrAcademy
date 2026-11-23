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
    public static class clsQuestionEnvelopesData
    {
        public static async Task<DataTable> GetAllAsync(int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                                SELECT ExamID, SubjectName, Level, Department, ExamDayName, ExamDate,
                                       TimeFrom, TimeTo, Period, CommitteeFrom, CommitteeTo, HallName,
                                       TotalStudents, EnvelopeOrder, TotalEnvelopes, DefaultAnswerForms
                                FROM vw_QuestionEnvelopes
                                WHERE SettingID = @SettingID
                                  AND (@LevelID IS NULL OR LevelID = @LevelID)
                                  AND (@DepartmentID IS NULL OR DepartmentID = @DepartmentID)
                                  AND (@SubjectID IS NULL OR SubjectID = @SubjectID)
                                ORDER BY ExamDate, TimeFrom, Level, Department, EnvelopeOrder;", conn))
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
                        dt.Columns.Add("CommitteeFrom", typeof(int));
                        dt.Columns.Add("CommitteeTo", typeof(int));
                        dt.Columns.Add("HallName", typeof(string));
                        dt.Columns.Add("TotalStudents", typeof(int));
                        dt.Columns.Add("EnvelopeOrder", typeof(int));
                        dt.Columns.Add("TotalEnvelopes", typeof(int));
                        dt.Columns.Add("DefaultAnswerForms", typeof(string));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5),
                                clsFormat.TimeNoLeadingZero(reader.GetString(6)),
                                clsFormat.TimeNoLeadingZero(reader.GetString(7)),
                                reader.GetString(8),
                                reader.GetInt32(9),
                                reader.GetInt32(10),
                                reader.GetString(11),
                                reader.GetInt32(12),
                                reader.GetInt32(13),
                                reader.GetInt32(14),
                                reader.GetString(15)
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
