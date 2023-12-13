namespace TechnicalTest.Services;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using TechnicalTest.Extensions;
using TechnicalTest.Models;

public class EventService
{
    public Event GetEvent()
    {
        return new Event
        {
            Tour = "DP World Tour",
            Name = "The 151st Open Championship",
            Course = "Royal Liverpool Golf Club, Hoylake, England",
            StartDate = new DateOnly(2023, 7, 20),
            EndDate = new DateOnly(2023, 7, 23),
            RoundSummaries = this.GetRoundSummaries(),
        };
    }

    private List<RoundSummary> GetRoundSummaries()
    {
        var roundSummaries = new List<RoundSummary>();
        var rounds = GetRounds();

        if (rounds == null)
        {
            return roundSummaries;
        }

        var playerIds = rounds.Select(x => x.PlayerId).Distinct();

        foreach (var playerId in playerIds)
        {
            roundSummaries.Add(new RoundSummary
            {
                PlayerId = playerId,
                Round1 = rounds.Find(x => x.PlayerId == playerId && x.RoundNo == 1),
                Round2 = rounds.Find(x => x.PlayerId == playerId && x.RoundNo == 2),
                Round3 = rounds.Find(x => x.PlayerId == playerId && x.RoundNo == 3),
                Round4 = rounds.Find(x => x.PlayerId == playerId && x.RoundNo == 4),
            });
        }

        return [.. roundSummaries.OrderBy(x => x.PositionDesc)];
    }

    private static List<Round> GetRounds()
    {
        var rounds = new List<Round>();

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Scores.xlsx");

        using var spreadsheetDocument = SpreadsheetDocument.Open(path, false);

        if (spreadsheetDocument == null)
        {
            return rounds;
        }

        var sheets = spreadsheetDocument.WorkbookPart?.Workbook?.GetFirstChild<Sheets>()?.Elements<Sheet>();
        var relationshipId = sheets?.First()?.Id?.Value;

        if (relationshipId == null)
        {
            return rounds;
        }

        var worksheetPart = spreadsheetDocument.WorkbookPart?.GetPartById(relationshipId) as WorksheetPart;
        var worksheet = worksheetPart?.Worksheet;
        var sheetData = worksheet?.GetFirstChild<SheetData>();
        var rows = sheetData?.Descendants<Row>();
        var headerRow = true;

        foreach (var row in rows ?? Enumerable.Empty<Row>())
        {
            if (headerRow)
            {
                headerRow = false;

                continue;
            }

            rounds.Add(new Round
            {
                PositionDesc = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 0)),
                PlayerId = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 1)),
                FirstName = spreadsheetDocument.GetCellValue(row, 2)?.ToString() ?? string.Empty,
                LastName = spreadsheetDocument.GetCellValue(row, 3)?.ToString() ?? string.Empty,
                Score = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 4)),
                RoundNo = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 5)),
                Strokes = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 6)),
                HolesPlayed = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 7)),
                IncompleteHoles = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 8)),
                Match = Convert.ToInt32(spreadsheetDocument.GetCellValue(row, 9)),
            });
        }

        return rounds;
    }
}
