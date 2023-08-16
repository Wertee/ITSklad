namespace Sklad.Models.Filters;

public class ProductFilter:BaseFilter
{
    public string Applicant { get; set; }
    public int YearOfIncome { get; set; }
}