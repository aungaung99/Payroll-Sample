using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Payroll.Data;

namespace Payroll.Areas.Admin.Pages.Employee
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PayrollContext _context;

        public IndexModel(PayrollContext context)
        {
            _context = context;
        }

        public IList<Data.Employee> Employee { get;set; }

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees.ToListAsync();
        }
    }
}
