using DAL;
using Shared.Models;
using System;
using BLL.Core;
using System.Data;

using System.Threading.Tasks;

namespace BLL
{
    public class clsSetting
    {
        //Enums
        enMode _mode;


        //Properties
        public int? SettingID { get; private set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public string SemesterType { get; set; }
        public string DefaultAnswerForms { get; set; }


        //Ctor
        public clsSetting(string academicYear, string semester, string semesterType, string defaultAnswerForms)
        {
            this._mode = enMode.Add;
            this.AcademicYear = academicYear;
            this.Semester = semester;
            this.SemesterType = semesterType;
            this.DefaultAnswerForms = defaultAnswerForms;
        }

        private clsSetting(int? settingID, string academicYear, string semester, string semesterType, string defaultAnswerForms)
        {
            this._mode = enMode.Update;
            this.SettingID = settingID;
            this.AcademicYear = academicYear;
            this.Semester = semester;
            this.SemesterType = semesterType;
            this.DefaultAnswerForms = defaultAnswerForms;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? settingID = await clsSettingData.AddNewAsync(this.AcademicYear, this.Semester, this.SemesterType, this.DefaultAnswerForms);
                this.SettingID = settingID;
                return settingID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة إعدادات جديدة");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.SettingID.HasValue)
                    return false;

                return await clsSettingData.UpdateAsync(this.SettingID.Value, this.AcademicYear, this.Semester, this.SemesterType, this.DefaultAnswerForms);
            }
            catch
            {
                throw new Exception("تعذر تحديث الإعدادات");
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

        public static async Task<clsSetting> FindAsync(int? settingID)
        {
            try
            {
                clsSettingDto setting = await clsSettingData.GetBySettingIdAsync(settingID);

                if (setting == null)
                    return null;

                return new clsSetting(setting.SettingID, setting.AcademicYear, setting.Semester,
                    setting.SemesterType, setting.DefaultAnswerForms);
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
                return await clsSettingData.GetAllAsync(pageNumber, rowsPerPage);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(int settingId)
        {
            try
            {
                return await clsSettingData.IsExistsAsync(settingId);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int settingId)
        {
            try
            {
                return await clsSettingData.DeleteAsync(settingId);
            }
            catch
            {
                throw new Exception("تعذر حذف الإعدادات، تأكد أنها غير مستخدمة في جدول الامتحانات أو التوزيع");
            }
        }


    }
}
