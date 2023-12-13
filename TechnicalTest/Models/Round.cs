namespace TechnicalTest.Models;

public class Round
{
    public int PositionDesc { get; set; }

    public int PlayerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Score { get; set; }

    public int RoundNo { get; set; }

    public int Strokes { get; set; }

    public int HolesPlayed { get; set; }

    public int IncompleteHoles { get; set; }

    public int Match { get; set; }
}
