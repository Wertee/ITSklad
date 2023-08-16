using System;

namespace Sklad.Models.Filters;

public class OutcomeFilter:BaseFilter
{
    public string Recipient { get; set; }
    public int YearOfOutcome { get; set; }
}