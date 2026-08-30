using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading.Tasks;
using Shared.Utilities;
using Shared.Models;

namespace DAL
{
    public class clsUserData
    {
        public static async Task<int?> AddNewAsync(string fullName, string username, string passwordHash, bool isActive)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    INSERT INTO Users (FullName, Username, PasswordHash, IsActive)
                    VALUES (@FullName, @Username, @PasswordHash, @IsActive);
                    SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

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

        public static async Task<bool> UpdateAsync(int userId, string fullName, string username, string passwordHash, bool isActive)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Users
                    SET FullName = @FullName,
                        Username = @Username,
                        PasswordHash = @PasswordHash,
                        IsActive = @IsActive
                    WHERE UserID = @UserID;", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

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

        public static async Task<clsUserDto> GetByUserIdAsync(int? userID)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT UserID, FullName, Username, PasswordHash, IsActive
                    FROM Users
                    WHERE UserID = @UserID;", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsUserDto()
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                FullName = reader["FullName"].ToString(),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
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

        public static async Task<clsUserDto> GetByUsernameAsync(string username)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT UserID, FullName, Username, PasswordHash, IsActive
                    FROM Users
                    WHERE Username = @Username;", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsUserDto()
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                FullName = reader["FullName"].ToString(),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
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

        public static async Task<clsUserDto> GetByCredentialsAsync(string username, string passwordHash)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT UserID, FullName, Username, PasswordHash, IsActive
                    FROM Users
                    WHERE Username = @Username AND PasswordHash = @passwordHash", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                    await conn.OpenAsync();

                    using (SqliteDataReader reader = (SqliteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new clsUserDto()
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                FullName = reader["FullName"].ToString(),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
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
                    SELECT UserID, FullName, Username, PasswordHash, IsActive
                    FROM Users
                    ORDER BY UserID
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

        public static async Task<bool> IsExistsAsync(string username)
        {
            bool userExists;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT 1
                    FROM Users
                    WHERE Username = @Username
                    LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    userExists = result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return userExists;
        }

        public static async Task<bool> DeleteAsync(int userId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    DELETE FROM Users
                    WHERE UserID = @UserID;", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

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

        public static async Task<string> GetHashPasswordAsync(int userId)
        {
            string hashPassword = null;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    SELECT PasswordHash
                    FROM Users
                    WHERE UserID = @UserID;", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        hashPassword = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError(ex.ToString());
                throw;
            }

            return hashPassword;
        }

        public static async Task<bool> ChangePasswordAsync(int userId, string newHashedPassword)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(clsDataAccessSettings.ConnectionString))
                using (SqliteCommand cmd = new SqliteCommand(@"
                    UPDATE Users
                    SET PasswordHash = @NewHashedPassword
                    WHERE UserID = @UserID;", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@NewHashedPassword", newHashedPassword);

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
