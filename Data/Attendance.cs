using System;
using System.Collections.Generic;

#nullable disable

namespace Payroll.Data
{
    public partial class Attendance
    {
        public string AttendId { get; set; }
        public int? EmpId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public int? TotalWorkingHour { get; set; }
        public int? LateMinute { get; set; }
    }
}
