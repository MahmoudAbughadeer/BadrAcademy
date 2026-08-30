using DAL;
using Shared.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using BLL.Core;

namespace BLL
{
    public class clsSubjectOffering
    {
        //Enums
        enMode _mode;


        //Properties
        public int? OfferingID { get; private set; }
        public int SubjectID { get; set; }
        public int LevelID { get; set; }
        public int DepartmentID { get; set; }
        public string Semester { get; set; }


        //Ctor
        public clsSubjectOffering(int subjectID, int levelID, int departmentID, string semester)
        {
            this._mode = enMode.Add;
            this.SubjectID = subjectID;
            this.LevelID = levelID;
            this.DepartmentID = departmentID;
            this.Semester = semester;
        }

        private clsSubjectOffering(int? offeringID, int subjectID, int levelID, int departmentID, string semester)
        {
            this._mode = enMode.Update;
            this.OfferingID = offeringID;
            this.SubjectID = subjectID;
            this.LevelID = levelID;
            this.DepartmentID = departmentID;
            this.Semester = semester;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? offeringID = await clsSubjectOfferingData.AddNewAsync(this.SubjectID, this.LevelID, this.DepartmentID, this.Semester);
                this.OfferingID = offeringID;
                return offeringID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة عرض مقرر جديد");
            }
        }

        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.OfferingID.HasValue)
                    return false;

                return await clsSubjectOfferingData.UpdateAsync(this.OfferingID.Value, this.SubjectID, this.LevelID, this.DepartmentID, this.Semester);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات عرض المقرر");
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

        public static async Task<clsSubjectOffering> FindAsync(int? offeringID)
        {
            try
            {
                clsSubjectOfferingDto offering = await clsSubjectOfferingData.GetByOfferingIdAsync(offeringID);

                if (offering == null)
                    return null;

                return new clsSubjectOffering(offering.OfferingID, offering.SubjectID, offering.LevelID,
                    offering.DepartmentID, offering.Semester);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        // Raw IDs
        public static async Task<DataTable> GetAllAsync(int pageNumber, int rowsPerPage)
        {
            try
            {
                return await clsSubjectOfferingData.GetAllAsync(pageNumber, rowsPerPage);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        // Resolved names — for UI grids/display
        public static async Task<DataTable> GetAllWithNamesAsync(int pageNumber, int rowsPerPage)
        {
            try
            {
                return await clsSubjectOfferingData.GetAllWithNamesAsync(pageNumber, rowsPerPage);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        
        
        // Filter by Level + Department + Semester — used when building exam schedule
        public static async Task<DataTable> GetByLevelDepartmentSemesterAsync(int levelId, int departmentId, string semester)
        {
            try
            {
                return await clsSubjectOfferingData.GetByLevelDepartmentSemesterAsync(levelId, departmentId, semester);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(int subjectId, int levelId, int departmentId, string semester)
        {
            try
            {
                return await clsSubjectOfferingData.IsExistsAsync(subjectId, levelId, departmentId, semester);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int offeringId)
        {
            try
            {
                return await clsSubjectOfferingData.DeleteAsync(offeringId);
            }
            catch
            {
                throw new Exception("تعذر حذف عرض المقرر، تأكد أنه غير مستخدم في جدول الامتحانات أو تعيينات الدكاترة");
            }
        }
    }
}
