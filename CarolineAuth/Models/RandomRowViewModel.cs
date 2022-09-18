namespace CarolineAuth.Models;

public enum SearchOptions
{
    English,
    Japanese
}

public class RandomRowViewModel
{
    public Row Row { get; set; }
    public SearchOptions SearchOption { get; set; }
}