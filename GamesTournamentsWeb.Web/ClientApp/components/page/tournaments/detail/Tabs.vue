<script lang="ts" setup>
const mainStore = useMainStore()
const tournamentsStore = useTournamentsStore()

const tournamentDetail = computed(() => tournamentsStore.tournamentDetail)
const isAdmin = computed(() => tournamentDetail.value?.admins?.some(admin => admin.id === mainStore.account?.id) ?? false)

</script>

<template>
  <TabView class="max-w-xs md:max-w-full">
    <TabPanel :header="$t('tournament_detail.overview')">
      <PageTournamentsDetailTabOverview class="text-area" />
    </TabPanel>
    <TabPanel :header="$t('tournament_detail.rules')">
      <PageTournamentsDetailTabRules class="text-area" />
    </TabPanel>
    <TabPanel :header="$t('tournament_detail.prizes')">
      <PageTournamentsDetailTabPrizes class="text-area" />
    </TabPanel>
    <TabPanel :header="$t('tournament_detail.players')">
      <PageTournamentsDetailTabPlayers class="text-area" />
    </TabPanel>
    <TabPanel :header="$t('tournament_detail.matches')">
      <PageTournamentsDetailTabMatches class="text-area" />
    </TabPanel>
    <TabPanel :header="$t('tournament_detail.streams')">
      <PageTournamentsDetailTabStreams class="text-area" />
    </TabPanel>
    <TabPanel v-if="isAdmin" :header="$t('tournament_detail.admin')">
      <PageTournamentsDetailTabAdmin class="text-area" />
    </TabPanel>
  </TabView>
</template>
