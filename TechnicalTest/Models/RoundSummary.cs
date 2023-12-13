namespace TechnicalTest.Models;

public class RoundSummary
{
    public int PlayerId { get; set; }

    public Round? Round1 { get; set; }

    public Round? Round2 { get; set; }

    public Round? Round3 { get; set; }

    public Round? Round4 { get; set; }

    public Round? LatestRound => this.Round4 ?? this.Round3 ?? this.Round2 ?? this.Round1;

    public int? Match => this.LatestRound?.Match;

    public int? PositionDesc => this.LatestRound?.PositionDesc;

    public string Name => $"{this.LatestRound?.FirstName?.Trim()} {this.LatestRound?.LastName?.Trim()}";

    public int? Par => this.LatestRound?.Score;

    public bool? MissedCut => this.Round4 == null && this.Round3 == null;
}
