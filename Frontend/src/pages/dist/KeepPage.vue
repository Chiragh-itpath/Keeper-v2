<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { AddKeep, AllKeeps } from '@/components/keeps'
import { DatePicker, TagSelector } from '@/components/Custom'
import { KeepStore } from '@/stores'

const date = ref()
const route = useRoute()
const loading: Ref<boolean> = ref(false)
const selectedTags: Ref<string[]> = ref([])
const { GetKeeps } = KeepStore()
const { Keeps, keepTags } = storeToRefs(KeepStore())
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
onMounted(async () => {
    loading.value = true
    await GetKeeps(projectId.value)
    loading.value = false
})
</script>
<template>
    <v-container class="overflow-auto px-10" fluid>
        <v-row>
            <v-col cols="12">
                <v-breadcrumbs divider="/" :items="[
                    {
                        title: 'Projects',
                        disabled: false,
                        to: '/Project'
                    },
                    {
                        title: 'Keep',
                        disabled: true
                    }
                ]">
                    <template v-slot:title="{ item }">
                        {{ item.title }}</template>
                </v-breadcrumbs>
            </v-col>
            <v-col class="d-flex">
                <tag-selector :items="keepTags" v-model:selected="selectedTags"></tag-selector>
                <span class="mx-2"></span>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" xl="2" class="my-auto d-flex justify-end">
                <add-keep :project-id="projectId"></add-keep>
            </v-col>
        </v-row>
        <v-row v-if="loading">
            <v-col v-for="i in 10" :key="i" cols="3">
                <v-skeleton-loader type="text,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-else class="mt-10">
            <all-keeps :date="date" :keeps="Keeps" :tags="selectedTags"></all-keeps>
        </v-row>
    </v-container>
</template>
