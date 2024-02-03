export default defineNuxtPlugin({
  async setup (_) {
    const mainStore = useMainStore()
    const accountStore = useAccountStore()
    const tournamentsStore = useTournamentsStore()
    const gamesStore = useGamesStore()

    await Promise.all([
      mainStore.initialize(),
      accountStore.initialize(),
      tournamentsStore.initialize(),
      gamesStore.initialize()
    ])
  },
  hooks: {
    'vue:setup' () {
      const mainStore = useMainStore()

      const { setLocale } = useI18n()
      setLocale(mainStore.locale)
    }
  }
})
