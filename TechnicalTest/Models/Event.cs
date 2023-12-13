namespace TechnicalTest.Models;

public class Event
{
    public string? Tour { get; set; }

    public string? Name { get; set; }

    public string? Course { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public required List<RoundSummary> RoundSummaries { get; set; }
}
