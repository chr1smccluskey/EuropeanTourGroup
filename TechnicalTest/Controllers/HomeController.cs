namespace TechnicalTest.Controllers;

using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using TechnicalTest.Models;
using TechnicalTest.Services;
using TechnicalTest.ViewModels;

public class HomeController : Controller
{
    private EventService eventService { get; set; }

    public HomeController(EventService eventService) => this.eventService = eventService;

    public IActionResult Index(HomeViewModel model)
    {
        this.ViewData[nameof(HomeViewModel.SortColumn)] = model.SortColumn;
        this.ViewData[nameof(HomeViewModel.SortDirection)] = model.SortDirection;

        model.Event = this.ApplyEventFiltering(model);

        return this.View(model);
    }

    private Event ApplyEventFiltering(HomeViewModel model)
    {
        var eventData = this.eventService.GetEvent();

        if (eventData.RoundSummaries != null && !string.IsNullOrWhiteSpace(model.SortColumn) && !string.IsNullOrWhiteSpace(model.SortDirection))
        {
            Func<RoundSummary, object>? orderByFunc = null;
            var defaultNumericalValue = model.SortDirection == nameof(ListSortDirection.Ascending) ? int.MaxValue : int.MinValue;

            orderByFunc = model.SortColumn switch
            {
                "Match" => x => x.Match ?? defaultNumericalValue,
                "Position" => x => x.PositionDesc ?? defaultNumericalValue,
                "Name" => x => x.Name,
                "Par" => x => x.Par ?? defaultNumericalValue,
                "R1" => x => x.Round1?.Strokes ?? defaultNumericalValue,
                "R2" => x => x.Round2?.Strokes ?? defaultNumericalValue,
                "R3" => x => x.Round3?.Strokes ?? defaultNumericalValue,
                "R4" => x => x.Round4?.Strokes ?? defaultNumericalValue,
                "Holes" => x => x.LatestRound?.HolesPlayed ?? defaultNumericalValue,
                _ => x => x.PositionDesc ?? defaultNumericalValue,
            };

            eventData.RoundSummaries = model.SortDirection == nameof(ListSortDirection.Ascending)
                ? [.. eventData.RoundSummaries.OrderBy(orderByFunc)]
                : [.. eventData.RoundSummaries.OrderByDescending(orderByFunc)];
        }

        if (eventData.RoundSummaries != null && (!string.IsNullOrWhiteSpace(model.Player) || !string.IsNullOrWhiteSpace(model.Match)))
        {
            if (!string.IsNullOrWhiteSpace(model.Player))
            {
                eventData.RoundSummaries = [.. eventData.RoundSummaries.Where(x => x.PlayerId.ToString() == model.Player || x.Name.Contains(model.Player, StringComparison.OrdinalIgnoreCase))];
            }

            if (!string.IsNullOrWhiteSpace(model.Match))
            {
                eventData.RoundSummaries = [.. eventData.RoundSummaries.Where(x => x.Match.ToString() == model.Match)];
            }
        }

        return eventData;
    }
}
