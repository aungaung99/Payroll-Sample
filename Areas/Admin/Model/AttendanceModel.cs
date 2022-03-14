using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Model
{
    public class AttendanceModel
    {
        public string AttendId { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string ClockIn { get; set; }
        public string ClockOut { get; set; }
        public double? TotalWorkingHour { get; set; }
        public int? LateMinute { get; set; }
        public double? TotalActualWHour { get; set; }
    }
}
