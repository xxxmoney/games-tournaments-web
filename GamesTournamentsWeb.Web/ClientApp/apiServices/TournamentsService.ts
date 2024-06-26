import type { TournamentOverview } from '~/models/tournaments/TournamentOverview'
import type { TournamentFilter } from '~/models/tournaments/TournamentFilter'
import type { Region } from '~/models/tournaments/Region'
import type { Platform } from '~/models/tournaments/Platform'
import type { Tournament } from '~/models/tournaments/Tournament'
import type { Currency } from '~/models/tournaments/Currency'
import type { PagedResult } from '~/models/PagedResult'
import type { TournamentEdit } from '~/models/tournaments/TournamentEdit'
import type { MatchEdit } from '~/models/tournaments/MatchEdit'
import type { Match } from '~/models/tournaments/Match'
import type { TournamentPlayer } from '~/models/tournaments/TournamentPlayer'
import type { TournamentCommentEdit } from '~/models/tournaments/TournamentCommentEdit'
import type { TournamentComment } from '~/models/tournaments/TournamentComment'

export const TournamentsService = {
  async getTournamentOverviews (filter: TournamentFilter): Promise<PagedResult<TournamentOverview>> {
    const api = useApi()
    const result = await api.post<PagedResult<TournamentOverview>>('tournaments/overviews', filter)
    return result.data
  },

  getTeamSizes (): Promise<number[]> {
    const result = Array.from(Array(256).keys()).map(i => i + 1)
    return Promise.resolve(result)
  },

  async getRegions (): Promise<Region[]> {
    const api = useApi()
    const result = await api.get<Region[]>('regions')
    return result.data
  },

  async getPlatforms (): Promise<Platform[]> {
    const api = useApi()
    const result = await api.get<Platform[]>('platforms')
    return result.data
  },

  async getCurrencies (): Promise<Currency[]> {
    const api = useApi()
    const result = await api.get<Currency[]>('currencies')
    return result.data
  },

  async getTournamentById (tournamentId: number): Promise<Tournament> {
    const api = useApi()
    const result = await api.get<Tournament>(`tournaments/${tournamentId}`)
    return result.data
  },

  async upsertTournament (tournament: TournamentEdit): Promise<Tournament> {
    const api = useApi()
    const result = await api.post('tournaments/upsert', tournament)
    return result.data
  },

  async joinTournament (tournamentId: number, gameUsername: string): Promise<Tournament> {
    const api = useApi()
    const result = await api.post<Tournament>(`tournaments/${tournamentId}/join/${encodeURIComponent(gameUsername)}`)
    return result.data
  },

  async deleteTournamentById (tournamentId: number): Promise<void> {
    const api = useApi()
    await api.delete(`tournaments/${tournamentId}/delete`)
  },

  async updateMatch (match: MatchEdit): Promise<Match[]> {
    const api = useApi()
    const result = await api.put('tournaments/match/update', match)
    return result.data
  },

  async createTournamentComment (tournamentCommentEdit: TournamentCommentEdit): Promise<TournamentComment> {
    const api = useApi()
    const result = await api.post('tournaments/comment/create', tournamentCommentEdit)
    return result.data
  },

  async setTournamentPlayerExpectedWinner (tournamentPlayerId: number, expectedWinnerId: number): Promise<TournamentPlayer> {
    const api = useApi()
    const result = await api.put(`tournaments/player/${tournamentPlayerId}/expected-winner/set/${expectedWinnerId}`)
    return result.data
  }
}
