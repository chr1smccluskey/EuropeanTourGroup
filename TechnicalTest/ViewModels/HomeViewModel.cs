namespace TechnicalTest.ViewModels;

using System.ComponentModel;

using TechnicalTest.Models;

public class HomeViewModel
{
    public required Event Event { get; set; }

    public string? Player { get; set; }

    public string? Match { get; set; }

    public string SortColumn { get; set; } = "Position";

    public string SortDirection { get; set; } = nameof(ListSortDirection.Ascending);
}
