using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class clsSubjectOfferingDto
    {
        public int OfferingID { get; set; }
        public int SubjectID { get; set; }
        public int LevelID { get; set; }
        public int DepartmentID { get; set; }
        public string Semester { get; set; }
    }
}
