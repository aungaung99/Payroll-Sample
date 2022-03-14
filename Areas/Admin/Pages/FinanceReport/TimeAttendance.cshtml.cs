using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Payroll.Data;

namespace Payroll.Areas.Admin.Pages.FinanceReport
{
    [Authorize]
    public class TimeAttendanceModel : PageModel
    {
        PayrollContext _context;
        public TimeAttendanceModel(PayrollContext context)
        {
            _context = context;
        }

        public class TimeAttendance
        {
            public string AttendId { get; set; }
            public int? EmpId { get; set; }
            public string EmpName { get; set; }
            public DateTime Date { get; set; }
            public string InoutTime { get; set; }
            public double? TotalWorkingHours { get; set; }
        }

        public class EmployeeVModel
        {
            public int? EmpId { get; set; }
            public string EmpName { get; set; }
            public double TotalWorkingHours { get; set; }
        }


        public IList<TimeAttendance> TimeAttendanceList { get; set; }

        public IList<EmployeeVModel> GetEmployees { get; set; }

        //public string GetInOutTime(int EmpId)
        //{

        //}

        public void OnGet()
        {
            var query = from attendance in _context.Attendances
                        join employee in _context.Employees
                        on attendance.EmpId equals employee.EmpId
                        select new TimeAttendance
                        {
                            AttendId = attendance.AttendId,
                            EmpId = employee.EmpId,
                            EmpName = employee.EmpName,
                            Date = attendance.Date.Value,
                            InoutTime = "<span class='text-success'>" + attendance.ClockIn.Value.ToString("hh:mm tt") + "</span></br><span class='text-danger'>" + attendance.ClockOut.Value.ToString("hh:mm tt") + "</span>",
                        };
            TimeAttendanceList = query.ToList();

            var empQuery = (from emp in _context.Employees
                            join attendance in _context.Attendances
                            on emp.EmpId equals attendance.EmpId
                            group emp by new { emp.EmpId,emp.EmpName}
                            into gemp
                            select new EmployeeVModel()
                            {
                                EmpId = gemp.Key.EmpId,
                                EmpName = gemp.Key.EmpName
                            }).ToList();
            var empList = empQuery.ToList();
            foreach (var item in empList)
            {
                item.TotalWorkingHours = EmpTotalWorkingHours(item.EmpId.Value);
            }

            GetEmployees = empList;

            //TimeAttendanceList = TimeAttendanceList.Ea(t=>t.TotalWorkingHours = 1);
        }

        public double EmpTotalWorkingHours(int EmpId)
        {
            var list = _context.Attendances.Where(a => a.EmpId == EmpId).ToList();
            double tWH = list.Sum(a => a.TotalWorkingHour).Value; // Total Working Hours of Each Employee
            double tLM = list.Sum(a => a.LateMinute).Value; // Total Late Minutes of Each Employee
            return WorkingHoursCalculator.Calculate(tWH, tLM);
        }
    }
}
