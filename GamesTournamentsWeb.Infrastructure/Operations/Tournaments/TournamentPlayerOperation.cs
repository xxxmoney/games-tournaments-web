﻿using AutoMapper;
using GamesTournamentsWeb.Common.Enums.Tournament;
using GamesTournamentsWeb.DataAccess.Repositories;
using GamesTournamentsWeb.Infrastructure.Dto.Tournaments;
using GamesTournamentsWeb.Infrastructure.Exceptions;
using GamesTournamentsWeb.Infrastructure.Helpers;

namespace GamesTournamentsWeb.Infrastructure.Operations.Tournaments;

public interface ITournamentPlayerOperation : IOperation
{
    Task<TournamentPlayer> UpsertTournamentPlayerStatusAsync(int? id, int accountId, TournamentPlayerStatusEnum status, string gameUsername = null, int? tournamentId = null);
    
    Task<ICollection<TournamentPlayer>> GetTournamentPlayersForTournamentAsync(int tournamentId);
    
    Task<ICollection<TournamentPlayer>> GetTournamentPlayersForAccountAsync(int accountId);
}

public class TournamentPlayerOperation(IRepositoryProvider repositoryProvider, IMapper mapper) : ITournamentPlayerOperation
{
    public async Task<TournamentPlayer> UpsertTournamentPlayerStatusAsync(int? id, int accountId, TournamentPlayerStatusEnum status, string gameUsername = null, int? tournamentId = null)
    {
        using var scope = repositoryProvider.CreateScope();
        var repository = scope.Provide<ITournamentPlayerRepository>();

        DataAccess.Models.Tournaments.TournamentPlayer tournamentPlayer;
        if (UpsertHelper.EntityExists(id))
        {
            tournamentPlayer = await repository.GetTournamentPlayerByIdAsync(id!.Value);
            repository.UpdateTournamentPlayer(tournamentPlayer);
            
            if (tournamentPlayer.AccountId != accountId)
            {
                throw new UnauthorizedUpdateException();
            }
        }
        else
        {
            if (!tournamentId.HasValue)
            {
                throw new ArgumentException("Tournament ID is required for new tournament player");
            }
            
            tournamentPlayer = new DataAccess.Models.Tournaments.TournamentPlayer
            {
                AccountId = accountId,
                GameUsername = gameUsername,
                StatusId = (int)status,
                TournamentId = tournamentId.Value
            };
            await repository.AddTournamentPlayerAsync(tournamentPlayer);
        }
        
        tournamentPlayer.StatusId = (int)status;
        if (status == TournamentPlayerStatusEnum.Accepted)
        {
            if (string.IsNullOrWhiteSpace(gameUsername))
            {
                throw new AccountInvitationGameUsernameEmpty();
            }
            
            tournamentPlayer.GameUsername = gameUsername;
        }

        await scope.SaveChangesAsync();
        
        return mapper.Map<TournamentPlayer>(tournamentPlayer);
    }

    public async Task<ICollection<TournamentPlayer>> GetTournamentPlayersForTournamentAsync(int tournamentId)
    {
        using var scope = repositoryProvider.CreateScope();
        var repository = scope.Provide<ITournamentPlayerRepository>();
        
        var models = await repository.GetTournamentPlayersForTournamentAsync(tournamentId);

        return mapper.Map<List<TournamentPlayer>>(models);
    }

    public async Task<ICollection<TournamentPlayer>> GetTournamentPlayersForAccountAsync(int accountId)
    {
        using var scope = repositoryProvider.CreateScope();
        var repository = scope.Provide<ITournamentPlayerRepository>();
        
        var models = await repository.GetActiveTournamentPlayersForAccountAsync(accountId);

        return mapper.Map<List<TournamentPlayer>>(models);
    }
}