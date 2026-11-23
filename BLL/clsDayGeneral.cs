using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class clsDayGeneral
    {
        public static async Task<DataTable> GetAllAsync(
            int settingID,
            int? levelID = null,
            int? departmentID = null,
            int? subjectID = null)
        {
            try
            {
                return await clsDayGeneralData.GetAllAsync(settingID, levelID, departmentID, subjectID);
            }
            catch
            {
                throw new Exception("حدث خطأ أثناء محاولة استرداد البيانات");
            }
        }

        public static async Task<DataTable> GetAllRepeatedAsync(
            Dictionary<int, int> repeatByDepartment,
            int settingID,
            int? levelID = null,
            int? departmentID = null,
            int? subjectID = null)
        {
            try
            {
                return await clsDayGeneralData.GetAllRepeatedAsync(
                    repeatByDepartment,
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
