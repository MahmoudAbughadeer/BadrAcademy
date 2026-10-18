using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class clsOfferingDetailsDto
    {
        public int? OfferingID { get; set; }
        public string SubjectName { get; set; }
        public string LevelName { get; set; }
        public string DepartmentName { get; set; }
        public string DoctorName { get; set; }
        public string Semester { get; set; }
        public bool HasExam {  get; set; }
    }
}
