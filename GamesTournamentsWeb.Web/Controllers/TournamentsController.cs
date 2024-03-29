﻿using GamesTournamentsWeb.Infrastructure.Dto.Tournaments;
using GamesTournamentsWeb.Infrastructure.Operations.Tournaments;
using GamesTournamentsWeb.Infrastructure.ViewModels.Tournaments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesTournamentsWeb.Web.Controllers;

public class TournamentsController(ITournamentOperation tournamentOperation) : BaseController
{
    [AllowAnonymous]
    [HttpPost("overviews")]
    public async Task<IActionResult> GetTournamentOverviews(TournamentFilter filter)
    {
        return Ok(await tournamentOperation.GetTournamentOverviewsPagedAsync(filter));
    }
    
    [AllowAnonymous]
    [HttpGet("{tournamentId}")]
    public async Task<IActionResult> GetTournament(int tournamentId)
    {
        return Ok(await tournamentOperation.GetTournamentByIdAsync(tournamentId));
    }
    
    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertTournament(TournamentEdit tournamentEdit)
    {
        return Ok(await tournamentOperation.UpsertTournamentAsync(tournamentEdit));
    }
    
    [HttpDelete("{tournamentId}/delete")]
    public async Task<IActionResult> DeleteTournament(int tournamentId)
    {
        await tournamentOperation.DeleteTournamentByIdAsync(tournamentId);
        return Ok();
    }
    
    
    
}