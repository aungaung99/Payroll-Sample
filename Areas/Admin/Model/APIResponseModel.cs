using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Areas.Admin.Model
{
    public class APIResponseModel
    {
        public dynamic Meta { get; set; }
        public dynamic Errors { get; set; }
        public dynamic Data { get; set; }
        public dynamic Links { get; set; }
    }
}
