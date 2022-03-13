using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OfficeOpenXml;

using Payroll.Areas.Admin.Model;
using Payroll.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        PayrollContext _context;
        public UploadController(PayrollContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGet(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new APIResponseModel()
                {
                    Errors = new { message = "Please send a valid request. IFormFile type required" }
                });
            }

            System.IO.Stream stream = file.OpenReadStream();
            try
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; //package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {

                            string code = worksheet.Cells[row, 1].Value?.ToString();
                            string name = worksheet.Cells[row, 2].Value?.ToString();

                            Data.Department department = new()
                            {
                                DeptCode = code,
                                DeptName = name
                            };
                            _context.Departments.Add(department);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(new APIResponseModel()
                            {
                                Errors = new { message = "Error in read excel file. Please check excel data", exception_message = ex.Message }
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                return Ok(new APIResponseModel() { });
            }
            catch (Exception e)
            {
                return BadRequest(new APIResponseModel()
                {
                    Errors = new { message = "Error in read excel file. Please check excel data", exception_message = e.Message }
                });
            }
        }
    }
}
