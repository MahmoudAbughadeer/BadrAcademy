using DAL;
using System;
using System.Data;
using System.Threading.Tasks;
using Shared.Models;
using BLL.Utilities;
using BLL.Core;

namespace BLL
{
    public class clsUser
    {
        //Enums
        enMode _mode;
       

        //Properties
        public int? UserID { get; private set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { private get; set; }
        public bool IsActive { get; set; }


        //Ctor
        public clsUser(string fullName, string username, string password, bool isActive)
        {
            this._mode = enMode.Add;
            this.FullName = fullName;
            this.Username = username;
            this.Password = password;
            this.IsActive = isActive;
        }

        private clsUser(int? userID, string fullName, string username, bool isActive)
        {
            this._mode = enMode.Update;
            this.UserID = userID;
            this.FullName = fullName;
            this.Username = username;
            this.IsActive = isActive;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                string hashedPassword = clsHasher.Hash(this.Password);

                int? userID = await clsUserData.AddNewAsync(this.FullName, this.Username, hashedPassword, this.IsActive);
                this.UserID = userID;
                return userID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة مستخدم جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.UserID.HasValue)
                    return false;

                string hashedPassword = !string.IsNullOrEmpty(this.Password)
                    ? clsHasher.Hash(this.Password)
                    : await clsUserData.GetHashPasswordAsync(this.UserID.Value);

                return await clsUserData.UpdateAsync(this.UserID.Value, this.FullName, this.Username, hashedPassword, this.IsActive);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات المستخدم");
            }
        }



        //Public Method
        public async Task<bool> SaveAsync()
        {
            switch (_mode)
            {
                case enMode.Add:
                    if (await AddNewAsync())
                    {
                        _mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return await UpdateAsync();
            }

            return false;
        }

        public static async Task<clsUser> FindAsync(int? userID)
        {
            try
            {
                clsUserDto user = await clsUserData.GetByUserIdAsync(userID);

                if (user == null)
                    return null;

                return new clsUser(user.UserID, user.FullName, user.Username, user.IsActive);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<clsUser> FindByCredentialsAsync(string username, string password)
        {
            try
            {
                clsUserDto user = await clsUserData.GetByCredentialsAsync(username, clsHasher.Hash(password));

                if (user == null)
                    return null;

                return new clsUser(user.UserID, user.FullName, user.Username, user.IsActive);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<DataTable> GetAllAsync(int pageNumber, int rowsPerPage)
        {
            try
            {
                return await clsUserData.GetAllAsync(pageNumber, rowsPerPage);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(string username)
        {
            try
            {
                return await clsUserData.IsExistsAsync(username);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int userId)
        {
            try
            {
                return await clsUserData.DeleteAsync(userId);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة حذف بيانات المستخدم");
            }
        }

        public static async Task<string> GetHashPasswordAsync(int userID)
        {
            try
            {
                return await clsUserData.GetHashPasswordAsync(userID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            string newHashed = clsHasher.Hash(newPassword);

            bool success = await clsUserData.ChangePasswordAsync(userId, newHashed);

            return success;
        }
    }
}
