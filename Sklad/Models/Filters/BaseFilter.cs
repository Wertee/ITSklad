namespace Sklad.Models.Filters;

public abstract class BaseFilter
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool ExactMatch { get; set; }
}