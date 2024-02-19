<script setup lang="ts">
import { useTheme } from '@/composable/useTheme'
const { currentTheme, toggle } = useTheme()
</script>
<template>
    <v-btn-toggle density="compact" mandatory class="rounded-pill me-3" :model-value="currentTheme" color="primary">
        <v-btn icon="mdi-white-balance-sunny" width="40" value="light" @click="() => toggle('light')" size="small"></v-btn>
        <v-btn icon="mdi-weather-night" width="40" value="dark" @click="() => toggle('dark')" size="small"></v-btn>
    </v-btn-toggle>
</template>
