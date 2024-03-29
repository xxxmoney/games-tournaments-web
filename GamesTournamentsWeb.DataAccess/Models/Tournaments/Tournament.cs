﻿using GamesTournamentsWeb.DataAccess.Models.Games;
using GamesTournamentsWeb.DataAccess.Models.Users;

namespace GamesTournamentsWeb.DataAccess.Models.Tournaments;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public int TeamSize { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
    public int PlatformId { get; set; }
    public Platform Platform { get; set; }
    public ICollection<Region> Regions { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string Info { get; set; }
    public string Rules { get; set; }
    public ICollection<Prize> Prizes { get; set; }
    public ICollection<Match> Matches { get; set; }
    public ICollection<Stream> Streams { get; set; }
    public int MinimumPlayers { get; set; }   
    public int MaximumPlayers { get; set; }   
    public bool AnyoneCanJoin { get; set; }   
    public ICollection<TournamentPlayer> Players { get; set; }   
    public ICollection<Account> Admins { get; set; }   
}

