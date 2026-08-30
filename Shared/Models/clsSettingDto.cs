using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class clsSettingDto
    {
        public int SettingID { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public string SemesterType { get; set; }
        public string DefaultAnswerForms { get; set; }
    }
}
