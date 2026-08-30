using DAL;
using Shared.Models;
using System;
using BLL.Core;
using System.Threading.Tasks;
using System.Data;

namespace BLL
{
    public class clsHall
    {
        //Enums
        enMode _mode;


        //Properties
        public int? HallID { get; private set; }
        public string HallName { get; set; }
        public string HallType { get; set; }
        public int DefaultCapacity { get; set; }
        public bool IsActive { get; set; }


        //Ctor
        public clsHall(string hallName, string hallType, int defaultCapacity, bool isActive = true)
        {
            this._mode = enMode.Add;
            this.HallName = hallName;
            this.HallType = hallType;
            this.DefaultCapacity = defaultCapacity;
            this.IsActive = isActive;
        }

        private clsHall(int? hallID, string hallName, string hallType, int defaultCapacity, bool isActive)
        {
            this._mode = enMode.Update;
            this.HallID = hallID;
            this.HallName = hallName;
            this.HallType = hallType;
            this.DefaultCapacity = defaultCapacity;
            this.IsActive = isActive;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? hallID = await clsHallData.AddNewAsync(this.HallName, this.HallType, this.DefaultCapacity, this.IsActive);
                this.HallID = hallID;
                return hallID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة قاعة جديدة");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.HallID.HasValue)
                    return false;

                return await clsHallData.UpdateAsync(this.HallID.Value, this.HallName, this.HallType, this.DefaultCapacity, this.IsActive);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات القاعة");
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

        public static async Task<clsHall> FindAsync(int? hallID)
        {
            try
            {
                clsHallDto hall = await clsHallData.GetByHallIdAsync(hallID);

                if (hall == null)
                    return null;

                return new clsHall(hall.HallID, hall.HallName, hall.HallType, hall.DefaultCapacity, hall.IsActive);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }

        }

        public static async Task<clsHall> FindByNameAsync(string hallName)
        {
            try
            {
                clsHallDto hall = await clsHallData.GetByHallNameAsync(hallName);

                if (hall == null)
                    return null;

                return new clsHall(hall.HallID, hall.HallName, hall.HallType, hall.DefaultCapacity, hall.IsActive);
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
                return await clsHallData.GetAllAsync(pageNumber, rowsPerPage);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<DataTable> GetAllActiveAsync()
        {
            try
            {
                return await clsHallData.GetAllActiveAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> IsExistsAsync(string hallName)
        {
            try
            {
                return await clsHallData.IsExistsAsync(hallName);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<bool> DeleteAsync(int hallId)
        {
            try
            {
                return await clsHallData.DeleteAsync(hallId);
            }
            catch
            {
                throw new Exception("تعذر حذف القاعة، تأكد أنها غير مستخدمة في جدول التوزيع");
            }
        }
    }
}
