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
    public class clsDoctor
    {
        //Enums
        enMode _mode;


        //Properties
        public int? DoctorID { get; private set; }
        public string DoctorName { get; set; }
        public int DepartmentID { get; set; }


        //Ctor
        public clsDoctor(string doctorName, int departmentID)
        {
            this._mode = enMode.Add;
            this.DoctorName = doctorName;
            this.DepartmentID = departmentID;
        }

        private clsDoctor(int? doctorID, string doctorName, int departmentID)
        {
            this._mode = enMode.Update;
            this.DoctorID = doctorID;
            this.DoctorName = doctorName;
            this.DepartmentID = departmentID;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? doctorID = await clsDoctorData.AddNewAsync(this.DoctorName, this.DepartmentID);
                this.DoctorID = doctorID;
                return doctorID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة دكتور جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.DoctorID.HasValue)
                    return false;

                return await clsDoctorData.UpdateAsync(this.DoctorID.Value, this.DoctorName, this.DepartmentID);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات الدكتور");
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

        public static async Task<clsDoctor> FindAsync(int? doctorID)
        {
            try
            {
                clsDoctorDto doctor = await clsDoctorData.GetByDoctorIdAsync(doctorID);

                if (doctor == null)
                    return null;

                return new clsDoctor(doctor.DoctorID, doctor.DoctorName, doctor.DepartmentID);
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
                return await clsDoctorData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int doctorId)
        {
            try
            {
                return await clsDoctorData.DeleteAsync(doctorId);
            }
            catch
            {
                throw new Exception("تعذر حذف الدكتور، تأكد أنه غير مستخدم في جدول المقررات");
            }
        }
    }
}
