﻿using System.Linq.Expressions;
using AutoMapper;
using GamesTournamentsWeb.Common.Enums.Tournament;
using GamesTournamentsWeb.Common.Models;
using GamesTournamentsWeb.DataAccess.Repositories;
using GamesTournamentsWeb.Infrastructure.Dto.Tournaments;
using GamesTournamentsWeb.Infrastructure.Helpers;
using GamesTournamentsWeb.Infrastructure.ViewModels.Tournaments;

namespace GamesTournamentsWeb.Infrastructure.Operations.Tournaments;

public interface ITournamentOperation : IOperation
{
    Task<PagedResult<TournamentOverview>> GetTournamentOverviewsPagedAsync(int? accountId, TournamentFilter filter);
    
    Task<Tournament> GetTournamentByIdAsync(int tournamentId);
    
    Task<Tournament> UpsertTournamentAsync(TournamentEdit tournamentEdit);
    
    Task<Tournament> SetBracketsFromTournamentMatchesAsync(int tournamentId);

    Task<ICollection<Match>> UpdateTournamentMatchAsync(MatchEdit matchEdit);
    
    Task DeleteTournamentByIdAsync(int tournamentId);
    
    Task<ICollection<TournamentExpectedWinnerStatisticsItem>> GetTournamentExpectedWinnerStatisticsAsync(int tournamentId);
}

public class TournamentOperation(IRepositoryProvider repositoryProvider, IMapper mapper, TimeProvider timeProvider) : ITournamentOperation
{
    public async Task<PagedResult<TournamentOverview>> GetTournamentOverviewsPagedAsync(int? accountId, TournamentFilter filter)
    {
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentRepository>();
        var pagedModels = await tournamentRepository.GetTournamentOverviewsFilteredPagedAsync(FilterTournaments(accountId, filter), filter.Page, Constants.PerPageCount);
        
        return pagedModels.WithData(mapper.Map<List<TournamentOverview>>(pagedModels.Results));
    }

    private static Expression<Func<DataAccess.Models.Tournaments.Tournament, bool>> FilterTournaments(int? accountId, TournamentFilter filter)
    {
        return tournament => (!accountId.HasValue || !filter.WithMyTournaments || tournament.Admins.Any(admin => admin.Id == accountId.Value))
            && (string.IsNullOrWhiteSpace(filter.Name) || tournament.Name.Contains(filter.Name))
            && (filter.GameId == null || tournament.GameId == filter.GameId)
            && (filter.TeamSizes == null || filter.TeamSizes.Length == 0 || filter.TeamSizes.Contains(tournament.TeamSize))
            && (filter.RegionIds == null || filter.RegionIds.Length == 0 || tournament.Regions.Any(region => filter.RegionIds.Contains(region.Id)))
            && (filter.PlatformIds == null || filter.PlatformIds.Length == 0 || filter.PlatformIds.Contains(tournament.PlatformId))
            && (filter.GenreIds == null || filter.GenreIds.Length == 0 || filter.GenreIds.Contains(tournament.Game.GenreId));
    }
    
    public async Task<Tournament> GetTournamentByIdAsync(int tournamentId)
    {
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentRepository>();
        
        var model = await tournamentRepository.GetTournamentByIdAsync(tournamentId);
        return await MapToTournamentAsync(model);
    }

    public async Task<Tournament> UpsertTournamentAsync(TournamentEdit tournamentEdit)
    {
        DataAccess.Models.Tournaments.Tournament tournamentModel;
        
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentRepository>();
        var accountRepository = scope.Provide<IAccountRepository>();
        var regionRepository = scope.Provide<IRegionRepository>();
        
        if (UpsertHelper.EntityExists(tournamentEdit.Id))
        {
            tournamentModel = await tournamentRepository.GetTournamentByIdAsync(tournamentEdit.Id!.Value);
            tournamentRepository.UpdateTournament(tournamentModel);
        }
        else
        {
            tournamentModel = new DataAccess.Models.Tournaments.Tournament();
            await tournamentRepository.AddTournamentAsync(tournamentModel);
        }
        
        // Set admins
        tournamentModel.Admins ??= new List<DataAccess.Models.Users.Account>();
        tournamentModel.Admins.Clear();
        foreach (var adminId in tournamentEdit.AdminIds ?? Array.Empty<int>().ToList())
        {
            var admin = await accountRepository.GetAccountByIdAsync(adminId);
            tournamentModel.Admins.Add(admin);
        }
        
        // Set regions
        tournamentModel.Regions ??= new List<DataAccess.Models.Tournaments.Region>();
        tournamentModel.Regions.Clear();
        foreach (var regionId in tournamentEdit.RegionIds ?? Array.Empty<int>().ToList())
        {
            var region = await regionRepository.GetRegionByIdAsync(regionId);
            tournamentModel.Regions.Add(region);
        }
        
        var editPlayerNames = (tournamentEdit.Players ?? Array.Empty<TournamentPlayerEdit>().ToList()).Select(p => p.GameUsername).OrderBy(_ => _).ToList();
        var modelPlayerNames = (tournamentModel.Players ?? Array.Empty<DataAccess.Models.Tournaments.TournamentPlayer>().ToList()).Select(p => p.GameUsername).OrderBy(_ => _).ToList();
        // Check whether there was a change in player names
        var arePlayersUpdated = !editPlayerNames.SequenceEqual(modelPlayerNames);
        
        mapper.Map(tournamentEdit, tournamentModel);
        
        await scope.SaveChangesAsync();

        // Recalculate matches if this is update (not insert) and players were updated
        if (UpsertHelper.EntityExists(tournamentEdit.Id) && arePlayersUpdated)
        {
            return await SetBracketsFromTournamentMatchesAsync(tournamentEdit.Id!.Value);
        }

        return await MapToTournamentAsync(tournamentModel);
    }

    public async Task<Tournament> SetBracketsFromTournamentMatchesAsync(int tournamentId)
    {
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentRepository>();
        var teamRepository = scope.Provide<ITeamRepository>();
        
        var tournamentModel = await tournamentRepository.GetTournamentByIdAsync(tournamentId);
        
        // Get current teams
        var currentTeamIds = GetTeamIds(tournamentModel);
        var currentTeams = await teamRepository.GetTeamsByIdsAsync(currentTeamIds);
        
        // Clear current matches
        tournamentModel.Matches.Clear();
        
        // Clear current teams
        teamRepository.RemoveTeams(currentTeams);
        
        // Save removal of teams and matches
        await scope.SaveChangesAsync();

        // Create teams from players
        var teams = new List<DataAccess.Models.Tournaments.Team>();
        var acceptedPlayers = tournamentModel.Players.Where(player => player.StatusId == (int)TournamentPlayerStatusEnum.Accepted).ToList();
        for (var i = 0; i < acceptedPlayers.Count; i += tournamentModel.TeamSize)
        {
            var players = acceptedPlayers.Skip(i).Take(tournamentModel.TeamSize).ToList();
            var team = new DataAccess.Models.Tournaments.Team
            {
                Name = tournamentModel.TeamSize == 1 ? players.Single().GameUsername : $"Team {i / tournamentModel.TeamSize + 1}",
                Players = players
            };
            teams.Add(team);
            await teamRepository.AddTeamAsync(team);
        }
        
        // Save teams
        await scope.SaveChangesAsync();
        tournamentRepository.UpdateTournament(tournamentModel);
        
        // Create initial matches
        foreach (var teamPair in teams.Chunk(2))
        {
            var firstTeam = teamPair[0];
            var secondTeam = teamPair.Length == 2 ? teamPair[1] : null;
            tournamentModel.Matches.Add(new DataAccess.Models.Tournaments.Match
            {
                TournamentId = tournamentModel.Id,
                Tournament = tournamentModel,
                FirstTeamId = firstTeam.Id,
                SecondTeamId = secondTeam?.Id,
                WinnerId = secondTeam == null ? firstTeam.Id : null
            });
        }
        var oddInitialMatch = tournamentModel.Matches.Count % 2 != 0 ? tournamentModel.Matches.Last() : null;
        var initialMatches = tournamentModel.Matches.Except([oddInitialMatch]).ToList();
        
        // Create new matches that will be used as subsequent matches
        var subsequentMatchesCount = TournamentHelper.CalculateTotalMatchesCount(tournamentModel.Matches.Count) - tournamentModel.Matches.Count;   
        var subsequentMatches = Enumerable.Range(0, subsequentMatchesCount).Select(_ => new DataAccess.Models.Tournaments.Match
        {
            TournamentId = tournamentModel.Id,
            Tournament = tournamentModel
        }).ToList();
        
        // Create subsequent matches
        foreach (var match in subsequentMatches)
        {
            tournamentModel.Matches.Add(match);
        }
        
        // Save matches
        await scope.SaveChangesAsync();
        tournamentRepository.UpdateTournament(tournamentModel);
        
        // Set initial matches as previous for first round
        var previousMatches = initialMatches;
        var initialSubsequentMatchesCount = subsequentMatches.Count;
        // Set subsequent matches to previous matches while there are subsequent matches
        while (subsequentMatches.Count != 0)
        {
            // Set subsequent matches - each subsequent match has previous two matches
            foreach (var matchPair in previousMatches.Chunk(2))
            {
                var firstMatch = matchPair[0];
                // If there is an odd number of initial matches, take the odd one from initial matches
                var secondMatch = matchPair.Length == 2 ? matchPair[1] : oddInitialMatch!;
                
                // If this is a first round and there is an even number of matches, skip this one
                if (firstMatch == secondMatch)
                {
                    continue;
                }
                
                // Take one from subsequent matches as next match
                var nextMatch = subsequentMatches.First();
                subsequentMatches.RemoveAt(0);
                firstMatch.NextMatchId = nextMatch.Id;
                secondMatch.NextMatchId = nextMatch.Id;
            }
            
            // Recalculate previous matches for next round
            previousMatches = tournamentModel.Matches
                .Where(m => m.NextMatchId == null && tournamentModel.Matches.Any(m2 => m2.NextMatchId == m.Id)).ToList();

            // Check for preventing cycling while checking firstMatch == secondMatch when adding first match
            if (initialSubsequentMatchesCount <= 1 && initialMatches.Count <= 1)
            {
                break;
            }
        }
        
        // Save matches
        await scope.SaveChangesAsync();
        tournamentRepository.UpdateTournament(tournamentModel);

        return await MapToTournamentAsync(tournamentModel);
    }

    public async Task<ICollection<Match>> UpdateTournamentMatchAsync(MatchEdit matchEdit)
    {
        using var scope = repositoryProvider.CreateScope();
        var matchRepository = scope.Provide<IMatchRepository>();
        var teamRepository = scope.Provide<ITeamRepository>();
        var tournamentRepository = scope.Provide<ITournamentRepository>();

        var matchModel = await matchRepository.GetMatchByIdAsync(matchEdit.MatchId);
        DataAccess.Models.Tournaments.Match nextMatchModel = null;
        matchRepository.UpdateMatch(matchModel);

        // Set winner of match
        if (matchEdit.EndMatch)
        {
            if (!matchEdit.WinnerId.HasValue)
            {
                throw new InvalidOperationException("Winner must be set when ending match");
            }
            matchModel.WinnerId = matchEdit.WinnerId.Value;
            // Match has finished, set end date
            matchModel.EndDate = timeProvider.GetLocalNow();
            
            // Set winner as one of participants to next match if there is any
            if (matchModel.NextMatchId.HasValue)
            {
                nextMatchModel = await matchRepository.GetMatchByIdAsync(matchModel.NextMatchId.Value);
                matchRepository.UpdateMatch(nextMatchModel);

                if (!nextMatchModel.FirstTeamId.HasValue)
                {
                    nextMatchModel.FirstTeamId = matchModel.WinnerId;
                }
                else if (!nextMatchModel.SecondTeamId.HasValue)
                {
                    nextMatchModel.SecondTeamId = matchModel.WinnerId;
                }
            }
            // This is final match, finish tournament
            else
            {
                var tournament = await tournamentRepository.GetTournamentByIdAsync(matchModel.TournamentId);
                tournamentRepository.UpdateTournament(tournament);
                tournament.EndDate = timeProvider.GetLocalNow();
            }
        }
        // Start match
        else if (matchEdit.StartMatch)
        {
            // Match has started, set start date
            matchModel.StartDate = timeProvider.GetLocalNow();
        }

        // Save changes
        await scope.SaveChangesAsync();
        
        var match = mapper.Map<Match>(matchModel);
        var nextMatch = nextMatchModel != null ? mapper.Map<Match>(nextMatchModel) : null;
        
        // Get teams for match
        var teamIds = new [] {match.FirstTeamId, match.SecondTeamId, match.WinnerId, nextMatch?.FirstTeamId, nextMatch?.SecondTeamId, nextMatch?.WinnerId}.Where(id => id.HasValue).Select(id => id.Value).ToArray();
        var teamModels = await teamRepository.GetTeamsByIdsAsync(teamIds);
        var teamByTeamId = mapper.Map<List<Team>>(teamModels).ToDictionary(key => key.Id);
        
        // Set teams to match
        match.FirstTeam = match.FirstTeamId.HasValue ? teamByTeamId[match.FirstTeamId.Value] : null;
        match.SecondTeam = match.SecondTeamId.HasValue ? teamByTeamId[match.SecondTeamId.Value] : null;
        match.Winner = match.WinnerId.HasValue ? teamByTeamId[match.WinnerId.Value] : null;
        
        // Set teams to next match if exists
        if (nextMatch != null)
        {
            nextMatch.FirstTeam = nextMatch.FirstTeamId.HasValue ? teamByTeamId[nextMatch.FirstTeamId.Value] : null;
            nextMatch.SecondTeam = nextMatch.SecondTeamId.HasValue ? teamByTeamId[nextMatch.SecondTeamId.Value] : null;
            nextMatch.Winner = nextMatch.WinnerId.HasValue ? teamByTeamId[nextMatch.WinnerId.Value] : null;
        }
        
        // Return all updated matches
        return new [] { match, nextMatch }.Where(m => m != null).ToList();
    }

    public async Task DeleteTournamentByIdAsync(int tournamentId)
    {
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentRepository>();
        var teamRepository = scope.Provide<ITeamRepository>();
        
        var tournament = await tournamentRepository.GetTournamentByIdAsync(tournamentId);
        
        // Get team ids from matches
        var teamIds = GetTeamIds(tournament);
        // Remove tournament
        tournamentRepository.DeleteTournament(tournament);
        
        // Remove teams
        var teams = await teamRepository.GetTeamsByIdsAsync(teamIds);
        teamRepository.RemoveTeams(teams);
        
        await scope.SaveChangesAsync();
    }

    public async Task<ICollection<TournamentExpectedWinnerStatisticsItem>> GetTournamentExpectedWinnerStatisticsAsync(int tournamentId)
    {
        using var scope = repositoryProvider.CreateScope();
        var tournamentRepository = scope.Provide<ITournamentPlayerRepository>();
        
        var items = await tournamentRepository.GetTournamentExpectedWinnerStatisticsAsync(tournamentId);
        return mapper.Map<List<TournamentExpectedWinnerStatisticsItem>>(items);
    }

    private static int[] GetTeamIds(DataAccess.Models.Tournaments.Tournament tournament)
    {
        return (tournament.Matches ?? Array.Empty<DataAccess.Models.Tournaments.Match>()).SelectMany(m => new [] { m.FirstTeamId, m.SecondTeamId }).Distinct()
            .Where(id => id.HasValue).Select(id => id.Value).ToArray();
    }

    private async Task<Tournament> MapToTournamentAsync(DataAccess.Models.Tournaments.Tournament tournamentModel)
    {
        var statisticsTask = this.GetTournamentExpectedWinnerStatisticsAsync(tournamentModel.Id);
        
        var teamIds = GetTeamIds(tournamentModel);
        
        using var scope = repositoryProvider.CreateScope();
        var teamRepository = scope.Provide<ITeamRepository>();
        
        // Get teams for tournament
        var teamModels = await teamRepository.GetTeamsByIdsAsync(teamIds);
        var teams = mapper.Map<List<Team>>(teamModels).ToDictionary(key => key.Id);
        
        var result = mapper.Map<Tournament>(tournamentModel);
        
        // Set teams to result
        foreach (var match in result.Matches)
        {
            match.FirstTeam = match.FirstTeamId.HasValue ? teams[match.FirstTeamId.Value] : null;
            match.SecondTeam = match.SecondTeamId.HasValue ? teams[match.SecondTeamId.Value] : null;
            match.Winner = match.WinnerId.HasValue ? teams[match.WinnerId.Value] : null;
        }
        
        // Set statistics
        result.ExpectedWinnerStatistics = await statisticsTask;

        return result;
    }

  
}