<script lang="ts" setup>
import constants from '~/constants'

const store = useAccountStore()

const isActive = computed({
  get: () => store.passwordChangeModalActive,
  set: (value) => {
    if (!value) {
      store.closePasswordChangeModal()
    }
  }
})

const close = () => {
  store.closePasswordChangeModal()
}
</script>

<template>
  <Dialog
    v-model:visible="isActive"
    :baseZIndex="constants.dialogBaseIndexZ"
    :draggable="false"
    :header="$t('common.password_change')"
    :resizable="false"
    modal
  >
    <div class="flex">
      <PageAccountPasswordChange class="mx-auto" @passwordChanged="close" />
    </div>
  </Dialog>
</template>
