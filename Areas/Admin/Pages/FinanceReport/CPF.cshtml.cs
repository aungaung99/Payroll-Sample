using Microsoft.AspNetCore.Mvc.RazorPages;

using Payroll.Data;

using System;
using System.Collections.Generic;
using System.Linq;

using static Payroll.WorkingHoursCalculator;

namespace Payroll.Areas.Admin.Pages.FinanceReport
{
    public class CPFModel : PageModel
    {
        private readonly PayrollContext _context;

        public CPFModel(PayrollContext context)
        {
            _context = context;
        }

        public List<CPFVModel> CPFModelList { get; set; }

        public class CPFVModel
        {
            public int EmpId { get; set; }
            public string EmpName { get; set; }
            public int Age { get; set; }
            public double TotalWage { get; set; }
            public double ByEmployer { get; set; }
            public double ByEmployerRate { get; set; }
            public double ByEmployee { get; set; }
            public double ByEmployeeRate { get; set; }
            public double TotalAmount { get; set; }
        }

        public void OnGet()
        {
            var query = from attenance in _context.Attendances.Where(a => a.Date.Value.Date.Month == DateTime.Now.Month)
                        join employee in _context.Employees
                        on attenance.EmpId equals employee.EmpId
                        join dept in _context.Departments
                        on employee.DeptCode equals dept.DeptCode
                        group attenance by new { employee.EmpId, employee.EmpName, employee.Salary, employee.Dob }
                        into gattendance
                        select new
                        {
                            gattendance.Key.EmpId,
                            gattendance.Key.EmpName,
                            gattendance.Key.Salary,
                            gattendance.Key.Dob,
                            TotalWorkingHour = Calculate(gattendance.Sum(g => g.TotalWorkingHour.Value), gattendance.Sum(g => g.LateMinute.Value)),
                            TotalAmount = Calculate(gattendance.Sum(g => g.TotalWorkingHour.Value), gattendance.Sum(g => g.LateMinute.Value)) * gattendance.Key.Salary
                        };
            CPFModelList = new List<CPFVModel>();

            foreach (var emp in query.ToList())
            {
                int age = DateTime.Now.Year - emp.Dob.Value.Year;

                double ByEmployerRate = CalcByEmployerRate(age);
                double ByEmployer = Math.Round((ByEmployerRate / 100) * emp.TotalAmount.Value, 2);

                double ByEmployeeRate = CalcByEmployeeRate(age);
                double ByEmployee = Math.Round((ByEmployerRate / 100) * emp.TotalAmount.Value, 2);

                CPFVModel model = new()
                {
                    EmpId = emp.EmpId,
                    EmpName = emp.EmpName,
                    Age = age,
                    TotalWage = Math.Round(emp.TotalAmount.Value, 2), // Total Salary by TotalWorkingHour
                    ByEmployerRate = ByEmployerRate,
                    ByEmployer = ByEmployer,
                    ByEmployeeRate = ByEmployeeRate,
                    ByEmployee = ByEmployee,
                    TotalAmount = ByEmployer + ByEmployee
                };

                CPFModelList.Add(model);
            }
        }

        public double CalcByEmployerRate(int age)
        {
            double percent = 0;
            if (age > 70)
            {
                percent = 7.5;
            }
            else if (age == 70 || age > 65)
            {
                percent = 8;
            }
            else if (age == 65 || age > 60)
            {
                percent = 10;
            }
            else if (age == 60 || age > 55)
            {
                percent = 14;
            }
            else if (age <= 55)
            {
                percent = 17;
            }

            return percent;
        }

        public double CalcByEmployeeRate(int age)
        {
            double percent = 0;
            if (age > 70)
            {
                percent = 5;
            }
            else if (age == 70 || age > 65)
            {
                percent = 6;
            }
            else if (age == 65 || age > 60)
            {
                percent = 8.5;
            }
            else if (age == 60 || age > 55)
            {
                percent = 14;
            }
            else if (age <= 55)
            {
                percent = 20;
            }

            return percent;
        }
    }
}
