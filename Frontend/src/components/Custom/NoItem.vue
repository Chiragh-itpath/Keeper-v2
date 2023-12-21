<script setup lang="ts">
defineProps<{
    title?: string,
    subTitle?: string
}>()
</script>
<template>
    <v-col cols="12">
        <div class="d-flex justify-center align-center flex-column">
            <div class="bg-blue-grey-lighten-3 rounded-circle pa-10 mt-10">
                <v-icon color="primary" size="150">mdi-notebook-remove</v-icon>
            </div>
            <p class="text-h5 mt-10 text-primary">
                {{ title }}
            </p>
            <p class="text-grey-darken-1 mt-2 text-center mb-10">
                {{ subTitle }}
            </p>
        </div>
    </v-col>
</template>
