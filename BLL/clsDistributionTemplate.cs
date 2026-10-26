using BLL.Core;
using DAL;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsDistributionTemplate
    {
        //Enums
        enMode _mode;


        //Properties
        public int? TemplateID { get; private set; }
        public int LevelID { get; set; }
        public int DepartmentID { get; set; }
        public int CommitteeNo { get; set; }
        public int StudentsCount { get; set; }
        public int SettingID { get; set; }
        public int HallID { get; set; }



        //Ctor
        public clsDistributionTemplate(int levelID, int departmentID, int committeeNo,
            int studentsCount, int settingID, int hallID)
        {
            this._mode = enMode.Add;
            this.LevelID = levelID;
            this.DepartmentID = departmentID;
            this.CommitteeNo = committeeNo;
            this.StudentsCount = studentsCount;
            this.SettingID = settingID;
            this.HallID = hallID;
        }

        private clsDistributionTemplate(int? templateID, int levelID, int departmentID, int committeeNo,
            int studentsCount, int settingID, int hallID)
        {
            this._mode = enMode.Update;
            this.TemplateID = templateID;
            this.LevelID = levelID;
            this.DepartmentID = departmentID;
            this.CommitteeNo = committeeNo;
            this.StudentsCount = studentsCount;
            this.SettingID = settingID;
            this.HallID = hallID;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? templateID = await clsDistributionTemplateData.AddNewAsync(this.LevelID, this.DepartmentID,
                    this.CommitteeNo, this.StudentsCount, this.SettingID, this.HallID);
                this.TemplateID = templateID;
                return templateID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة قالب توزيع جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.TemplateID.HasValue)
                    return false;

                return await clsDistributionTemplateData.UpdateAsync(this.TemplateID.Value, this.LevelID,
                    this.DepartmentID, this.CommitteeNo, this.StudentsCount, this.SettingID, this.HallID);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات قالب التوزيع");
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

        public static async Task<clsDistributionTemplate> FindAsync(int? templateID)
        {
            try
            {
                clsDistributionTemplateDto template = await clsDistributionTemplateData.GetByTemplateIdAsync(templateID);

                if (template == null)
                    return null;

                return new clsDistributionTemplate(template.TemplateID, template.LevelID, template.DepartmentID,
                    template.CommitteeNo, template.StudentsCount, template.SettingID, template.HallID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<clsDistributionTemplate> FindByUniqueKeyAsync(int settingID, int levelID,
            int departmentID, int committeeNo)
        {
            try
            {
                clsDistributionTemplateDto template = await clsDistributionTemplateData.GetByUniqueKeyAsync(
                    settingID, levelID, departmentID, committeeNo);

                if (template == null)
                    return null;

                return new clsDistributionTemplate(template.TemplateID, template.LevelID, template.DepartmentID,
                    template.CommitteeNo, template.StudentsCount, template.SettingID, template.HallID);
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
                return await clsDistributionTemplateData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }
        public static async Task<DataTable> GetDistributionTemplateDetailsAsync()
        {
            try
            {
                return await clsDistributionTemplateData.GetDistributionTemplateDetailsAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }
        public static async Task<DataTable> GetBySettingIdAsync(int settingID)
        {
            try
            {
                return await clsDistributionTemplateData.GetBySettingIdAsync(settingID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int templateId)
        {
            try
            {
                return await clsDistributionTemplateData.DeleteAsync(templateId);
            }
            catch
            {
                throw new Exception("تعذر حذف قالب التوزيع");
            }
        }

        public static async Task<bool> DeleteBySettingIDAsync(int settingID)
        {
            try
            {
                return await clsDistributionTemplateData.DeleteBySettingIDAsync(settingID);
            }
            catch
            {
                throw new Exception("تعذر حذف قوالب التوزيع");
            }
        }

        public static clsDistributionTemplate CreateForUpdate(int templateID, int levelID, int departmentID,
            int committeeNo, int studentsCount, int settingID, int hallID)
        {
            return new clsDistributionTemplate(templateID, levelID, departmentID, committeeNo,
                studentsCount, settingID, hallID);
        }
    }
}
