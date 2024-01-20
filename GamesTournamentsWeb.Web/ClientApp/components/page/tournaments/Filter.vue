<script lang="ts" setup>
import { KeyValuePair } from '~/models/KeyValuePair'
import constants from '~/constants'

const tournamentsStore = useTournamentsStore()

const filter = computed(() => tournamentsStore.filter)

const teamSizes = computed(() => tournamentsStore.teamSizes.map(teamSize => new KeyValuePair(teamSize, teamSize)))
const regions = computed(() => tournamentsStore.regions)
const platforms = computed(() => tournamentsStore.platforms)

</script>

<template>
  <div class="form-container">
    <CommonInputText v-model="filter.name" :label="$t('common.name')" />

    <CommonWithLabel :label="$t('common.team_size')">
      <MultiSelect
        v-model="filter.teamSizes"
        :options="teamSizes"
        :placeholder="$t('common.team_size')"
        :virtualScrollerOptions="{ itemSize: constants.virtualScrollHeight }"
        class="w-full"
        display="chip"
        filter
        optionLabel="key"
        optionValue="value"
      />
    </CommonWithLabel>

    <CommonWithLabel :label="$t('common.region')">
      <MultiSelect
        v-model="filter.regionIds"
        :optionLabel="value => $t(`enums.region.${value.code}`)"
        :options="regions"
        :placeholder="$t('common.region')"
        :virtualScrollerOptions="{ itemSize: constants.virtualScrollHeight }"
        class="w-full"
        display="chip"
        optionValue="id"
      />
    </CommonWithLabel>

    <CommonWithLabel :label="$t('common.platform')">
      <MultiSelect
        v-model="filter.platformIds"
        :optionLabel="value => $t(`enums.platform.${value.code}`)"
        :options="platforms"
        :placeholder="$t('common.platform')"
        :virtualScrollerOptions="{ itemSize: constants.virtualScrollHeight }"
        class="w-full"
        display="chip"
        optionValue="id"
      />
    </CommonWithLabel>

    <CommonWithLabel :label="$t('common.my_tournaments')">
      <Checkbox v-model="filter.withMyTournaments" :binary="true" />
    </CommonWithLabel>
  </div>
</template>