using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Model
{
    public class EmployeeModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? Dob { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public DateTime? HiredDate { get; set; }
        public double? Salary { get; set; }
    }
}
