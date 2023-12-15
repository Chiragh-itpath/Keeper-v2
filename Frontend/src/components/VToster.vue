<script lang="ts" setup>
import { storeToRefs } from 'pinia'
import { GlobalStore } from '@/stores'

const { toasterText, tosterVisible, toasterColor } = storeToRefs(GlobalStore())

</script>
<template>
    <v-snackbar v-model="tosterVisible" :color="toasterColor" location="end top" @click="tosterVisible = false"
        role="button" timeout="1000">
        <div class="d-flex align-center">
            <div class="mx-3">
                <v-icon v-if="toasterColor == 'success'" size="x-large">mdi-check</v-icon>
                <v-icon v-if="toasterColor == 'warning'" size="x-large">mdi-alert</v-icon>
                <v-icon v-if="toasterColor == 'danger'" size="x-large">mdi-alert-circle-outline</v-icon>
            </div>
            <div class="d-flex flex-column">
                <div class="text-body">
                    <span v-if="toasterColor == 'success'">Success!</span>
                    <span v-if="toasterColor == 'warning'">Warning!</span>
                    <span v-if="toasterColor == 'danger'">Error!</span>
                </div>
                <span>
                    {{ toasterText }}
                </span>
            </div>
        </div>
    </v-snackbar>
</template>
@/stores
