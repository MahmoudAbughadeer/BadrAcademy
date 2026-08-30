using DAL;
using Shared.Models;
using System;
using System.Data;
using BLL.Core;
using System.Threading.Tasks;

namespace BLL
{
    public class clsLevel
    {
        //Enums
        enMode _mode;


        //Properties
        public int? LevelID { get; private set; }
        public string LevelName { get; set; }


        //Ctor
        public clsLevel(string levelName)
        {
            this._mode = enMode.Add;
            this.LevelName = levelName;
        }

        private clsLevel(int? levelID, string levelName)
        {
            this._mode = enMode.Update;
            this.LevelID = levelID;
            this.LevelName = levelName;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? levelID = await clsLevelData.AddNewAsync(this.LevelName);
                this.LevelID = levelID;
                return levelID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة فرقة جديدة");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.LevelID.HasValue)
                    return false;

                return await clsLevelData.UpdateAsync(this.LevelID.Value, this.LevelName);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات الفرقة");
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

        public static async Task<clsLevel> FindAsync(int? levelID)
        {
            try
            {
                clsLevelDto level = await clsLevelData.GetByLevelIdAsync(levelID);

                if (level == null)
                    return null;

                return new clsLevel(level.LevelID, level.LevelName);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<clsLevel> FindByNameAsync(string levelName)
        {
            try
            {
                clsLevelDto level = await clsLevelData.GetByLevelNameAsync(levelName);

                if (level == null)
                    return null;

                return new clsLevel(level.LevelID, level.LevelName);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<DataTable> GetAllAsync()
        {
            try
            {
                return await clsLevelData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(string levelName)
        {
            try
            {
                return await clsLevelData.IsExistsAsync(levelName);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int levelId)
        {
            try
            {
                return await clsLevelData.DeleteAsync(levelId);
            }
            catch
            {
                throw new Exception("تعذر حذف الفرقة، تأكد أنها غير مستخدمة في جدول المقررات أو التوزيع");
            }
        }


    }
}
