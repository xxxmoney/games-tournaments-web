<script lang="ts" setup>
import useVuelidate from '@vuelidate/core'
import constants from '~/constants'

const { required } = useValidators()

const edit = useTournamentEdit()
const isTournamentFinished = useIsTournamentDetailFinished()
const isTournamentStarted = useIsTournamentDetailStarted()

const rules = {
  rules: { required, $autoDirty: true }
}

const v$ = useVuelidate(rules, edit)
const { validate } = useValidate(v$.value.$validate)

useTournamentEditNextStepRequestWithValidate(constants.tournamentEditSteps.rules, validate)

defineExpose({
  validate
})
</script>

<template>
  <h1 class="heading mb-lg">{{ $t('tournament_edit.steps.rules') }}</h1>

  <div class="form-container">
    <CommonWithErrors :errors="v$.rules.$errors">
      <CommonWithLabel
        v-tooltip="$t('tournament_edit.write_rules_tooltip')"
        :label="$t('common.rules')"
        class="gap"
      >
        <CommonTextEditor v-model="edit.rules" :disabled="isTournamentFinished || isTournamentStarted" />
      </Commonwithlabel>
    </CommonWithErrors>
  </div>
</template>
