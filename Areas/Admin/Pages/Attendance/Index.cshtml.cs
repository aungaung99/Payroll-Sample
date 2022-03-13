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
    public class IndexModel : PageModel
    {
        private readonly Payroll.Data.PayrollContext _context;

        public IndexModel(Payroll.Data.PayrollContext context)
        {
            _context = context;
        }

        public IList<Data.Attendance> Attendance { get;set; }

        public async Task OnGetAsync()
        {
            Attendance = await _context.Attendances.ToListAsync();
        }
    }
}
