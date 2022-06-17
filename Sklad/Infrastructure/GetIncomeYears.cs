using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sklad.Infrastructure
{
    public static class GetIncomeYears
    {
        public static List<int> GetYears()
        {
            List<int> yearsList = new();
            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year-5; i--)
            {
                yearsList.Add(i);
            }
            return yearsList;
        }
    }
}
