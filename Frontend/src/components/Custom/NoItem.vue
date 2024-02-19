<script setup lang="ts">
import { RouterEnum } from '@/Models/enum'
import { useTheme } from '@/composable/useTheme'

withDefaults(defineProps<{
    title?: string,
    subTitle?: string,
    backButton?: boolean
}>(), {
    backButton: false
})
const { dark } = useTheme()
</script>
<template>
    <v-col cols="12">
        <div class="d-flex justify-center align-center flex-column pa-5">
            <div class="rounded-circle pa-10 mt-10" :class="`bg-blue-grey-lighten-${dark ? '1' : '3'}`">
                <v-icon color="primary" size="150">mdi-notebook-remove</v-icon>
            </div>
            <p class="text-h5 mt-10 text-primary">
                <slot name="title">
                    {{ title }}
                </slot>
            </p>
            <p class="text-grey-darken-1 mt-2 text-center mb-10">
                <slot name="subtitle">
                    {{ subTitle }}
                </slot>
            </p>
            <p v-if="backButton">
                <router-link :to="{ name: RouterEnum.PROJECT }" class="text-black text-decoration-underline">
                    All Projects
                </router-link>
            </p>
        </div>
    </v-col>
</template>
