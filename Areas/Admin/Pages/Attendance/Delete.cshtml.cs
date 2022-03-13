using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Payroll.Data;

namespace Payroll.Areas.Admin.Pages.Attendance
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Payroll.Data.PayrollContext _context;

        public DeleteModel(Payroll.Data.PayrollContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Attendance Attendance { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Attendance = await _context.Attendances.FirstOrDefaultAsync(m => m.AttendId == id);

            if (Attendance == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Attendance = await _context.Attendances.FindAsync(id);

            if (Attendance != null)
            {
                _context.Attendances.Remove(Attendance);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
