﻿namespace GamesTournamentsWeb.Infrastructure.Dto.Tournaments;

public class Match
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public int? NextMatchId { get; set; }
    public Team FirstTeam { get; set; }
    public int? FirstTeamId { get; set; }
    public Team SecondTeam { get; set; }
    public int? SecondTeamId { get; set; }
    public Team Winner { get; set; }
    public int? WinnerId { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsRunning => StartDate.HasValue && !EndDate.HasValue && !WinnerId.HasValue;
}
