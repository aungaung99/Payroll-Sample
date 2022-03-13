using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Payroll.Areas.Admin.Pages.Departments
{
    [Authorize]
    public class ImportModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
