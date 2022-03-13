using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Payroll.Areas.Admin.Model;
using Payroll.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Payroll.WorkingHoursCalculator;

namespace Payroll.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatatablesController : ControllerBase
    {
        private readonly PayrollContext _context;
        public DatatablesController(PayrollContext context)
        {
            _context = context;
        }

        [HttpGet("departments")]
        public async Task<IActionResult> Departments()
        {
            List<Department> list = await _context.Departments.ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        [HttpGet("employee")]
        public async Task<IActionResult> Employee(string deptcode, string hireddate)
        {
            IQueryable<EmployeeModel> query = from employee in _context.Employees
                                              join department in _context.Departments
                                              on employee.DeptCode equals department.DeptCode
                                              select new EmployeeModel()
                                              {
                                                  DeptCode = employee.DeptCode,
                                                  DeptName = department.DeptName,
                                                  Dob = employee.Dob,
                                                  EmpId = employee.EmpId,
                                                  EmpName = employee.EmpName,
                                                  HiredDate = employee.HiredDate,
                                                  Salary = employee.Salary
                                              };

            if (!string.IsNullOrEmpty(deptcode))
            {
                query = query.Where(d => d.DeptCode == deptcode);
            }

            if (!string.IsNullOrEmpty(hireddate))
            {
                DateTime hiredDatePara = DateTime.Parse(hireddate);
                query = query.Where(d => d.HiredDate.Value.Date == hiredDatePara.Date);
            }

            List<EmployeeModel> list = await query.ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> Attendance(string startdate, string enddate)
        {
            IQueryable<Attendance> attendanceList = _context.Attendances.Where(a => a.ClockIn.Value.Date >= DateTime.Now.Date && a.ClockIn.Value.Date <= DateTime.Now.Date);
            List<Attendance> atdList = attendanceList.ToList();
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                DateTime dtpStartDate = DateTime.Parse(startdate);
                DateTime dtpEndDate = DateTime.Parse(enddate);

                attendanceList = _context.Attendances.Where(a => a.ClockIn.Value.Date >= dtpStartDate.Date && a.ClockIn.Value.Date <= dtpEndDate.Date);
            }

            IQueryable<AttendanceModel> query = from attendance in attendanceList
                                                join employee in _context.Employees
                                                on attendance.EmpId equals employee.EmpId
                                                join department in _context.Departments
                                                on employee.DeptCode equals department.DeptCode
                                                select new AttendanceModel()
                                                {
                                                    AttendId = attendance.AttendId,
                                                    EmpId = attendance.EmpId,
                                                    EmpName = employee.EmpName,
                                                    ClockIn = attendance.ClockIn.Value.ToString("dd/MMM/yyyy hh:mm:ss tt"),
                                                    ClockOut = attendance.ClockOut.Value.ToString("dd/MMM/yyyy hh:mm:ss tt"),
                                                    LateMinute = attendance.LateMinute,
                                                    TotalWorkingHour = attendance.TotalWorkingHour,
                                                    DeptName = department.DeptName
                                                };


            List<AttendanceModel> list = await query.ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        [HttpGet("report-employee")]
        public async Task<IActionResult> ReportEmployee(string deptCode)
        {
            IQueryable<EmployeeModel> query = from employee in _context.Employees
                                              join department in _context.Departments
                                              on employee.DeptCode equals department.DeptCode
                                              select new EmployeeModel()
                                              {
                                                  DeptCode = employee.DeptCode,
                                                  DeptName = department.DeptName,
                                                  Dob = employee.Dob,
                                                  EmpId = employee.EmpId,
                                                  EmpName = employee.EmpName,
                                                  HiredDate = employee.HiredDate,
                                                  Salary = employee.Salary
                                              };

            if (!string.IsNullOrEmpty(deptCode) && deptCode != "0")
            {
                query = query.Where(d => d.DeptCode == deptCode);
            }

            List<EmployeeModel> list = await query.OrderByDescending(e => e.HiredDate).ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        [HttpGet("report-attendance")]
        public async Task<IActionResult> ReportAttendance(string name, string startdate, string enddate)
        {
            IQueryable<Attendance> attendanceList = _context.Attendances.Where(a => a.ClockIn.Value.Date >= DateTime.Now.Date && a.ClockIn.Value.Date <= DateTime.Now.Date);
            List<Attendance> atdList = attendanceList.ToList();
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                DateTime dtpStartDate = DateTime.Parse(startdate);
                DateTime dtpEndDate = DateTime.Parse(enddate);

                attendanceList = _context.Attendances.Where(a => a.ClockIn.Value.Date >= dtpStartDate.Date && a.ClockIn.Value.Date <= dtpEndDate.Date);
            }

            IQueryable<AttendanceModel> query = from attendance in attendanceList
                                                join employee in _context.Employees
                                                on attendance.EmpId equals employee.EmpId
                                                join department in _context.Departments
                                                on employee.DeptCode equals department.DeptCode
                                                select new AttendanceModel()
                                                {
                                                    AttendId = attendance.AttendId,
                                                    EmpId = attendance.EmpId,
                                                    EmpName = employee.EmpName,
                                                    ClockIn = attendance.ClockIn.Value.ToString("dd/MMM/yyyy hh:mm:ss tt"),
                                                    ClockOut = attendance.ClockOut.Value.ToString("dd/MMM/yyyy hh:mm:ss tt"),
                                                    LateMinute = attendance.LateMinute,
                                                    TotalWorkingHour = attendance.TotalWorkingHour,
                                                    DeptCode = employee.DeptCode,
                                                    DeptName = department.DeptName
                                                };

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.EmpName.ToLower().Contains(name.ToLower()));
            }

            List<AttendanceModel> list = await query.ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        [HttpGet("finance-payroll")]
        public async Task<IActionResult> Payroll()
        {
            IQueryable<PayrollModel> query = from attenance in _context.Attendances
                                             join employee in _context.Employees
                                             on attenance.EmpId equals employee.EmpId
                                             join dept in _context.Departments
                                             on employee.DeptCode equals dept.DeptCode
                                             group attenance by new { employee.EmpId, employee.EmpName, employee.Salary }
                                             into gattendance
                                             select new PayrollModel()
                                             {
                                                 EmpId = gattendance.Key.EmpId,
                                                 EmpName = gattendance.Key.EmpName,
                                                 Salary = gattendance.Key.Salary,
                                                 TotalWorkingHour = Calculate(gattendance.Sum(g => g.TotalWorkingHour.Value), gattendance.Sum(g => g.LateMinute.Value)),
                                                 TotalAmount = Calculate(gattendance.Sum(g => g.TotalWorkingHour.Value), gattendance.Sum(g => g.LateMinute.Value)) * gattendance.Key.Salary
                                             };
            List<PayrollModel> list = await query.ToListAsync();
            return Ok(new APIResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
            });
        }

        //[HttpGet("finance-timeattendance")]
        //public async Task<IActionResult> TimeAttendance()
        //{
        //    var query=from attendance in _context.Attendances
        //              join employee in _context.Employees
        //              on attendance.EmpId equals employee.EmpId
        //              group attendance by new { employee.EmpId,employee.EmpName }

                      
        //}
    }
}
