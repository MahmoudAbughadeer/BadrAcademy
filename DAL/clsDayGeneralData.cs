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
    public static class clsDayGeneralData
    {
        public static async Task<DataTable> GetAllAsync(
            int settingID,
            int? levelID = null,
            int? departmentID = null,
            int? subjectID = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                        SELECT ExamID, SubjectName, DoctorName, Doctor2Name, Level, Department,
                               ExamDayName, ExamDate, TimeFrom, TimeTo, Period
                        FROM vw_DayGeneral
                        WHERE SettingID = @SettingID
                          AND (@LevelID IS NULL OR LevelID = @LevelID)
                          AND (@DepartmentID IS NULL OR DepartmentID = @DepartmentID)
                          AND (@SubjectID IS NULL OR SubjectID = @SubjectID)
                        ORDER BY ExamDate, TimeFrom, Level, Department, DoctorName;", conn))
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
                        dt.Columns.Add("DoctorName", typeof(string));
                        dt.Columns.Add("Doctor2Name", typeof(string));
                        dt.Columns.Add("Level", typeof(string));
                        dt.Columns.Add("Department", typeof(string));
                        dt.Columns.Add("ExamDayName", typeof(string));
                        dt.Columns.Add("ExamDate", typeof(string));
                        dt.Columns.Add("TimeFrom", typeof(string));
                        dt.Columns.Add("TimeTo", typeof(string));
                        dt.Columns.Add("Period", typeof(string));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5),
                                reader.GetString(6),
                                reader.GetString(7),
                                clsFormat.TimeNoLeadingZero(reader.GetString(8)),
                                clsFormat.TimeNoLeadingZero(reader.GetString(9)),
                                reader.GetString(10)
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



        public static async Task<DataTable> GetAllRepeatedAsync(Dictionary<int, int> repeatByDepartment,
        int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            DataTable dt = new DataTable();
            var repeats = repeatByDepartment ?? new Dictionary<int, int>();

            try
            {
                // (NULL, 1) keeps the VALUES list valid when no department is given; it never matches a row
                var values = new StringBuilder("(NULL, 1)");
                for (int i = 0; i < repeats.Count; i++)
                    values.Append(", (@Dept" + i + ", @Count" + i + ")");

                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand($@"
                        WITH RECURSIVE Numbers(n) AS (
                            SELECT 1 UNION ALL SELECT n + 1 FROM Numbers WHERE n < 50
                        ),
                        Repeats(DepartmentID, RepeatCount) AS (VALUES {values} )
                        SELECT g.ExamID, g.SubjectName, g.DoctorName, g.Doctor2Name, g.Level, g.Department,
                               g.ExamDayName, g.ExamDate, g.TimeFrom, g.TimeTo, g.Period, num.n AS CopyNo
                        FROM vw_DayGeneral g
                        LEFT JOIN Repeats r ON r.DepartmentID = g.DepartmentID
                        JOIN Numbers num    ON num.n <= COALESCE(r.RepeatCount, 1)
                        WHERE g.SettingID = @SettingID
                          AND (@LevelID IS NULL OR g.LevelID = @LevelID)
                          AND (@DepartmentID IS NULL OR g.DepartmentID = @DepartmentID)
                          AND (@SubjectID IS NULL OR g.SubjectID = @SubjectID)
                        ORDER BY g.ExamDate, g.TimeFrom, g.Level, g.Department, g.ExamID, g.DoctorName, num.n;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);
                    cmd.Parameters.AddWithValue("@LevelID", (object)levelID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartmentID", (object)departmentID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubjectID", (object)subjectID ?? DBNull.Value);

                    int i = 0;
                    foreach (var item in repeats)
                    {
                        cmd.Parameters.AddWithValue("@Dept" + i, item.Key);
                        cmd.Parameters.AddWithValue("@Count" + i, item.Value);
                        i++;
                    }

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("ExamID", typeof(int));
                        dt.Columns.Add("SubjectName", typeof(string));
                        dt.Columns.Add("DoctorName", typeof(string));
                        dt.Columns.Add("Doctor2Name", typeof(string));
                        dt.Columns.Add("Level", typeof(string));
                        dt.Columns.Add("Department", typeof(string));
                        dt.Columns.Add("ExamDayName", typeof(string));
                        dt.Columns.Add("ExamDate", typeof(string));
                        dt.Columns.Add("TimeFrom", typeof(string));
                        dt.Columns.Add("TimeTo", typeof(string));
                        dt.Columns.Add("Period", typeof(string));
                        dt.Columns.Add("CopyNo", typeof(int));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5),
                                reader.GetString(6),
                                reader.GetString(7),
                                clsFormat.TimeNoLeadingZero(reader.GetString(8)),
                                clsFormat.TimeNoLeadingZero(reader.GetString(9)),
                                reader.GetString(10),
                                reader.GetInt32(11)
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
