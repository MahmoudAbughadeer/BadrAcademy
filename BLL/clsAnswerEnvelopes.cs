using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class clsAnswerEnvelopes
    {
        public static async Task<DataTable> GetAllAsyncForFinal(int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            try
            {
                return await clsAnswerEnvelopesData.GetAllAsyncForFinal(
                    settingID,
                    levelID,
                    departmentID,
                    subjectID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<DataTable> GetAllAsyncForMidterm(int settingID, int? levelID = null, int? departmentID = null, int? subjectID = null)
        {
            try
            {
                return await clsAnswerEnvelopesData.GetAllAsyncForMidterm(
                    settingID,
                    levelID,
                    departmentID,
                    subjectID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }
    }
}
