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
    public class clsDistributionTemplateData
    {
        public static async Task<int?> AddNewAsync(int levelId, int departmentId, int committeeNo,
            int studentsCount, int settingId, int hallId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO DistributionTemplate (LevelID, DepartmentID, CommitteeNo, StudentsCount, SettingID, HallID)
                    VALUES (@LevelID, @DepartmentID, @CommitteeNo, @StudentsCount, @SettingID, @HallID);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@CommitteeNo", committeeNo);
                    cmd.Parameters.AddWithValue("@StudentsCount", studentsCount);
                    cmd.Parameters.AddWithValue("@SettingID", settingId);
                    cmd.Parameters.AddWithValue("@HallID", hallId);

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

        public static async Task<bool> UpdateAsync(int templateId, int levelId, int departmentId, int committeeNo,
            int studentsCount, int settingId, int hallId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE DistributionTemplate
                    SET LevelID = @LevelID,
                        DepartmentID = @DepartmentID,
                        CommitteeNo = @CommitteeNo,
                        StudentsCount = @StudentsCount,
                        SettingID = @SettingID,
                        HallID = @HallID
                    WHERE TemplateID = @TemplateID;", conn))
                {
                    cmd.Parameters.AddWithValue("@TemplateID", templateId);
                    cmd.Parameters.AddWithValue("@LevelID", levelId);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@CommitteeNo", committeeNo);
                    cmd.Parameters.AddWithValue("@StudentsCount", studentsCount);
                    cmd.Parameters.AddWithValue("@SettingID", settingId);
                    cmd.Parameters.AddWithValue("@HallID", hallId);

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

        public static async Task<clsDistributionTemplateDto> GetByTemplateIdAsync(int? templateID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT TemplateID, LevelID, DepartmentID, CommitteeNo, StudentsCount, SettingID, HallID
                    FROM DistributionTemplate
                    WHERE TemplateID = @TemplateID;", conn))
                {
                    cmd.Parameters.AddWithValue("@TemplateID", templateID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsDistributionTemplateDto()
                            {
                                TemplateID = Convert.ToInt32(reader["TemplateID"]),
                                LevelID = Convert.ToInt32(reader["LevelID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                CommitteeNo = Convert.ToInt32(reader["CommitteeNo"]),
                                StudentsCount = Convert.ToInt32(reader["StudentsCount"]),
                                SettingID = Convert.ToInt32(reader["SettingID"]),
                                HallID = Convert.ToInt32(reader["HallID"]),
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

        public static async Task<clsDistributionTemplateDto> GetByUniqueKeyAsync(int settingID, int levelID,
            int departmentID, int committeeNo)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT TemplateID, LevelID, DepartmentID, CommitteeNo, StudentsCount, SettingID, HallID
                    FROM DistributionTemplate
                    WHERE SettingID = @SettingID
                      AND LevelID = @LevelID
                      AND DepartmentID = @DepartmentID
                      AND CommitteeNo = @CommitteeNo;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);
                    cmd.Parameters.AddWithValue("@LevelID", levelID);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.Parameters.AddWithValue("@CommitteeNo", committeeNo);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsDistributionTemplateDto()
                            {
                                TemplateID = Convert.ToInt32(reader["TemplateID"]),
                                LevelID = Convert.ToInt32(reader["LevelID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                CommitteeNo = Convert.ToInt32(reader["CommitteeNo"]),
                                StudentsCount = Convert.ToInt32(reader["StudentsCount"]),
                                SettingID = Convert.ToInt32(reader["SettingID"]),
                                HallID = Convert.ToInt32(reader["HallID"]),
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
                    SELECT TemplateID, LevelID, DepartmentID, CommitteeNo, StudentsCount, SettingID, HallID
                    FROM DistributionTemplate
                    ORDER BY TemplateID", conn))
                {
                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("TemplateID", typeof(int));
                        dt.Columns.Add("LevelID", typeof(int));
                        dt.Columns.Add("DepartmentID", typeof(int));
                        dt.Columns.Add("CommitteeNo", typeof(int));
                        dt.Columns.Add("StudentsCount", typeof(int));
                        dt.Columns.Add("SettingID", typeof(int));
                        dt.Columns.Add("HallID", typeof(int));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                reader.GetInt32(6)
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
        public static async Task<DataTable> GetDistributionTemplateDetailsAsync()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
            SELECT
                dt.TemplateID,
                dt.SettingID,
                l.LevelName,
                d.DepartmentName,
                dt.CommitteeNo,
                dt.StudentsCount,
                h.HallName
            FROM DistributionTemplate dt
            JOIN Levels l       ON dt.LevelID = l.LevelID
            JOIN Departments d  ON dt.DepartmentID = d.DepartmentID
            JOIN Halls h        ON dt.HallID = h.HallID
            ORDER BY dt.CommitteeNo;", conn))
                {
                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("TemplateID", typeof(int));
                        dt.Columns.Add("SettingID", typeof(int));
                        dt.Columns.Add("LevelName", typeof(string));
                        dt.Columns.Add("DepartmentName", typeof(string));
                        dt.Columns.Add("CommitteeNo", typeof(int));
                        dt.Columns.Add("StudentsCount", typeof(int));
                        dt.Columns.Add("HallName", typeof(string));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                reader.GetString(6)
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
        public static async Task<DataTable> GetBySettingIdAsync(int settingID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT TemplateID, LevelID, DepartmentID, CommitteeNo, StudentsCount, SettingID, HallID
                    FROM DistributionTemplate
                    WHERE SettingID = @SettingID
                    ORDER BY LevelID, DepartmentID, CommitteeNo;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        dt.Columns.Add("TemplateID", typeof(int));
                        dt.Columns.Add("LevelID", typeof(int));
                        dt.Columns.Add("DepartmentID", typeof(int));
                        dt.Columns.Add("CommitteeNo", typeof(int));
                        dt.Columns.Add("StudentsCount", typeof(int));
                        dt.Columns.Add("SettingID", typeof(int));
                        dt.Columns.Add("HallID", typeof(int));

                        while (await reader.ReadAsync())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                reader.GetInt32(6)
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

        public static async Task<bool> DeleteAsync(int templateId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM DistributionTemplate
                    WHERE TemplateID = @TemplateID;", conn))
                {
                    cmd.Parameters.AddWithValue("@TemplateID", templateId);

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

        public static async Task<bool> DeleteBySettingIDAsync(int settingID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM DistributionTemplate
                    WHERE SettingID = @SettingID;", conn))
                {
                    cmd.Parameters.AddWithValue("@SettingID", settingID);

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
