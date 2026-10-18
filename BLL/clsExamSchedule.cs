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
    public class clsExamSchedule
    {
        //Enums
        enMode _mode;


        //Properties
        public int? ExamID { get; private set; }
        public int OfferingID { get; set; }
        public string ExamDayName { get; set; }
        public DateTime ExamDate { get; set; }
        public float TimeFrom { get; set; }
        public float TimeTo { get; set; }
        public string Period { get; set; }
        public int SettingID { get; set; }



        //Ctor
        public clsExamSchedule(int offeringID, string examDayName, DateTime examDate,
            float timeFrom, float timeTo, string period, int settingID)
        {
            this._mode = enMode.Add;
            this.OfferingID = offeringID;
            this.ExamDayName = examDayName;
            this.ExamDate = examDate;
            this.TimeFrom = timeFrom;
            this.TimeTo = timeTo;
            this.Period = period;
            this.SettingID = settingID;
        }

        private clsExamSchedule(int? examID, int offeringID, string examDayName, DateTime examDate,
            float timeFrom, float timeTo, string period, int settingID)
        {
            this._mode = enMode.Update;
            this.ExamID = examID;
            this.OfferingID = offeringID;
            this.ExamDayName = examDayName;
            this.ExamDate = examDate;
            this.TimeFrom = timeFrom;
            this.TimeTo = timeTo;
            this.Period = period;
            this.SettingID = settingID;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? examID = await clsExamScheduleData.AddNewAsync(this.OfferingID, this.ExamDayName,
                    this.ExamDate, this.TimeFrom, this.TimeTo, this.Period, this.SettingID);
                this.ExamID = examID;
                return examID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة موعد امتحان جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.ExamID.HasValue)
                    return false;

                return await clsExamScheduleData.UpdateAsync(this.ExamID.Value, this.OfferingID, this.ExamDayName,
                    this.ExamDate, this.TimeFrom, this.TimeTo, this.Period, this.SettingID);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات موعد الامتحان");
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

        public static async Task<clsExamSchedule> FindAsync(int? examID)
        {
            try
            {
                clsExamScheduleDto exam = await clsExamScheduleData.GetByExamIdAsync(examID);

                if (exam == null)
                    return null;

                return new clsExamSchedule(exam.ExamID, exam.OfferingID, exam.ExamDayName, exam.ExamDate,
                    exam.TimeFrom, exam.TimeTo, exam.Period, exam.SettingID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<clsExamSchedule> FindByOfferingIdAsync(int? offeringID)
        {
            try
            {
                clsExamScheduleDto exam = await clsExamScheduleData.GetByOfferingIdAsync(offeringID);

                if (exam == null)
                    return null;

                return new clsExamSchedule(exam.ExamID, exam.OfferingID, exam.ExamDayName, exam.ExamDate,
                    exam.TimeFrom, exam.TimeTo, exam.Period, exam.SettingID);
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
                return await clsExamScheduleData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<DataTable> GetExamScheduleDetailsAsync()
        {
            try
            {
                return await clsExamScheduleData.GetExamScheduleDetailsAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }
        
        public static async Task<bool> DeleteAsync(int examId)
        {
            try
            {
                return await clsExamScheduleData.DeleteAsync(examId);
            }
            catch
            {
                throw new Exception("تعذر حذف موعد الامتحان");
            }
        }

        public static async Task<bool> DeleteByOfferingIDAsync(int offeringID)
        {
            try
            {
                return await clsExamScheduleData.DeleteByOfferingIDAsync(offeringID);
            }
            catch
            {
                throw new Exception("تعذر حذف موعد الامتحان");
            }
        }

        public static clsExamSchedule CreateForUpdate(int examID, int offeringID, string examDayName, DateTime examDate,
            float timeFrom, float timeTo, string period, int settingID)
        {
            return new clsExamSchedule(examID, offeringID, examDayName, examDate, timeFrom, timeTo, period, settingID);
        }

    }
}
