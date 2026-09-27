using DAL;
using Shared.Models;
using System;
using System.Data;
using BLL.Core;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSubject
    {
        //Enums
        enMode _mode;


        //Properties
        public int? SubjectID { get; private set; }
        public string SubjectName { get; set; }



        //Ctor
        public clsSubject(string subjectName)
        {
            this._mode = enMode.Add;
            this.SubjectName = subjectName;
        }

        private clsSubject(int? subjectID, string subjectName)
        {
            this._mode = enMode.Update;
            this.SubjectID = subjectID;
            this.SubjectName = subjectName;
        }


        //Private Method
        private async Task<bool> AddNewAsync()
        {
            try
            {
                int? subjectID = await clsSubjectData.AddNewAsync(this.SubjectName);
                this.SubjectID = subjectID;
                return subjectID != null;
            }
            catch (Exception ex)
            {
                throw new Exception("تعذر إضافة مقرر جديد");
            }
        }
        private async Task<bool> UpdateAsync()
        {
            try
            {
                if (!this.SubjectID.HasValue)
                    return false;

                return await clsSubjectData.UpdateAsync(this.SubjectID.Value, this.SubjectName);
            }
            catch
            {
                throw new Exception("تعذر تحديث بيانات المقرر");
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

        public static async Task<clsSubject> FindAsync(int? subjectID)
        {
            try
            {
                clsSubjectDto subject = await clsSubjectData.GetBySubjectIdAsync(subjectID);

                if (subject == null)
                    return null;

                return new clsSubject(subject.SubjectID, subject.SubjectName);
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
                return await clsSubjectData.GetAllAsync();
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }


        public static async Task<bool> DeleteAsync(int subjectId)
        {
            try
            {
                return await clsSubjectData.DeleteAsync(subjectId);
            }
            catch
            {
                throw new Exception("تعذر حذف المقرر، تأكد أنه غير مستخدم في جدول عروض المقررات");
            }
        }

        public static clsSubject CreateForUpdate(int subjectID, string subjectName)
        {
            return new clsSubject(subjectID, subjectName);
        }

    }
}
