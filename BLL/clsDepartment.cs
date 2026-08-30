using DAL;
using Shared.Models;
using System;
using System.Data;
using BLL.Core;
using System.Threading.Tasks;

namespace BLL
{
    public class clsDepartment
    {
        //Enums
        enMode _mode;


        //Properties
        public int? DepartmentID { get; private set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }


        //Ctor
        public clsDepartment(string departmentName, string description = null)
        {
            this._mode = enMode.Add;
            this.DepartmentName = departmentName;
            this.Description = description;
        }

        private clsDepartment(int? departmentID, string departmentName, string description)
        {
            this._mode = enMode.Update;
            this.DepartmentID = departmentID;
            this.DepartmentName = departmentName;
            this.Description = description;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? departmentID = await clsDepartmentData.AddNewAsync(this.DepartmentName, this.Description);
                this.DepartmentID = departmentID;
                return departmentID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة قسم جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.DepartmentID.HasValue)
                    return false;

                return await clsDepartmentData.UpdateAsync(this.DepartmentID.Value, this.DepartmentName, this.Description);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات القسم");
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

        public static async Task<clsDepartment> FindAsync(int? departmentID)
        {
            try
            {
                clsDepartmentDto department = await clsDepartmentData.GetByDepartmentIdAsync(departmentID);

                if (department == null)
                    return null;

                return new clsDepartment(department.DepartmentID, department.DepartmentName, department.Description);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<clsDepartment> FindByNameAsync(string departmentName)
        {
            try
            {
                clsDepartmentDto department = await clsDepartmentData.GetByDepartmentNameAsync(departmentName);

                if (department == null)
                    return null;

                return new clsDepartment(department.DepartmentID, department.DepartmentName, department.Description);
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
                return await clsDepartmentData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(string departmentName)
        {
            try
            {
                return await clsDepartmentData.IsExistsAsync(departmentName);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int departmentId)
        {
            try
            {
                return await clsDepartmentData.DeleteAsync(departmentId);
            }
            catch
            {
                throw new Exception("تعذر حذف القسم، تأكد أنه غير مستخدم في جدول المقررات أو التوزيع");
            }
        }
    }
}
