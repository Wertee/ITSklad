using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklad.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Applicant { get; set; }
        public int CurrentCount { get; set; }
        public bool CanBeGiven { get; set; }
        public int CountToGive { get; set; }
        public int YearOfIncome { get; set; }
        public bool Used { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}