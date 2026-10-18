using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class clsExamScheduleDto
    {
        public int ExamID { get; set; }
        public int OfferingID { get; set; }
        public string ExamDayName { get; set; }
        public DateTime ExamDate { get; set; }
        public float TimeFrom { get; set; }
        public float TimeTo { get; set; }
        public string Period { get; set; }
        public int SettingID { get; set; }
    }
}
