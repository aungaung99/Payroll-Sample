using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll
{
    public static class WorkingHoursCalculator
    {
        public static double Calculate(double thrs, double lmin)
        {
            decimal totalminutes = (decimal)Math.Round(thrs * 60.00, 2);
            decimal lateminutes = (decimal)Math.Round(lmin / 60.00, 2);
            decimal actualminites = Math.Round(totalminutes - lateminutes, 2);
            return (double)Math.Round(actualminites / 60, 2);
        }
    }
}
