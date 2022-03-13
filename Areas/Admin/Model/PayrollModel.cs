using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Model
{
    public class PayrollModel
    {
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
        public double? TotalWorkingHour { get; set; }
        public double? Salary { get; set; }
        public double? TotalAmount { get; set; }
    }
}
