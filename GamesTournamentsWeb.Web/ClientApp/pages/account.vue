<script lang="ts" setup>
definePageMeta({
  middleware: 'auth'
})

const mainStore = useMainStore()
const accountStore = useAccountStore()

const account = computed(() => mainStore.account)
if (account.value) {
  await accountStore.getAccountInfo()
}
</script>

<template>
  <div v-if="account" class="form-container-lg mx-auto">
    <PageAccountUserInfo />
    <PageAccountActions />

    <PageAccountPasswordChangeDialog />
    <PageAccountHistoryDialog />
    <PageAccountInvitationsDialog />
  </div>
</template>
