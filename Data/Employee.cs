using System;
using System.Collections.Generic;

#nullable disable

namespace Payroll.Data
{
    public partial class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? Dob { get; set; }
        public string DeptCode { get; set; }
        public DateTime? HiredDate { get; set; }
        public double? Salary { get; set; }
    }
}
