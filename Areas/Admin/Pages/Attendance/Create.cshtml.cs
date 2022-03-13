using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Pages.Attendance
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Payroll.Data.PayrollContext _context;

        public CreateModel(Payroll.Data.PayrollContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Data.Attendance Attendance { get; set; }

        [BindProperty]
        public string ClockIn { get; set; }
        [BindProperty]
        public string ClockOut { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string getDate = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime clockIn = DateTime.ParseExact(getDate + " " + ClockIn, "yyyy-MM-dd h:mm tt", CultureInfo.GetCultureInfo("en-US"));
            DateTime clockOut = DateTime.ParseExact(getDate + " " + ClockOut, "yyyy-MM-dd h:mm tt", CultureInfo.GetCultureInfo("en-US"));

            Attendance.AttendId = GetAttendanceId();
            Attendance.Date = DateTime.Now;
            Attendance.ClockIn = clockIn;
            Attendance.ClockOut = clockOut;
            _context.Attendances.Add(Attendance);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public string GetAttendanceId()
        {
            string date = $"{DateTime.Now.Year.ToString()[2..]}{DateTime.Now.Month:D2}{DateTime.Now.Day:D2}";
            Data.Attendance result = _context.Attendances.Where(a => a.ClockIn.Value.Date == DateTime.Now.Date).OrderByDescending(a => a.Date.Value).ToList().FirstOrDefault();
            if (result == null)
            {
                return $"{date}001";
            }
            else
            {
                string maxId = $"{int.Parse(result.AttendId[6..]) + 1:D3}";
                return $"{date}{maxId}";
            }

        }
    }
}
