using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class clsDistributionTemplateDto
    {
        public int TemplateID { get; set; }
        public int LevelID { get; set; }
        public int DepartmentID { get; set; }
        public int CommitteeNo { get; set; }
        public int StudentsCount { get; set; }
        public int SettingID { get; set; }
        public int HallID { get; set; }
    }
}
