<script setup lang="ts">
import { ref, onMounted, type Ref } from 'vue'
import { AddProject, AllProject } from '@/components/Project'
import { DatePicker, TagSelector } from '@/components/Custom'
import { storeToRefs } from 'pinia'
import { ProjectStore } from '@/stores'

const { Projects, ProjectTags } = storeToRefs(ProjectStore())
const date = ref('');
const loading: Ref<boolean> = ref(false)
const selectedTags: Ref<string[]> = ref([])
const filter = ref(0)

onMounted(async () => {
    loading.value = true
    await ProjectStore().GetProjects()
    loading.value = false
})
</script>
<template>
    <v-container class="pa-10" fluid>
        <v-row v-if="loading">
            <v-col v-for=" i in 8" :key="i" cols="12" sm="6" md="4" lg="3">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row class="align-center" v-if="!loading">
            <v-col cols="auto">
                <v-btn-toggle color="primary" density="comfortable" mandatory class="rounded-pill" v-model="filter">
                    <v-btn prepend-icon="mdi-account">
                        All
                    </v-btn>
                    <v-btn prepend-icon="mdi-account-multiple">Shared</v-btn>
                </v-btn-toggle>
            </v-col>
            <v-col cols="auto" class="d-flex">
                <tag-selector :items="ProjectTags" v-model:selected="selectedTags"></tag-selector>
                <span class="mx-2"></span>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col class="d-flex justify-end">
                <add-project></add-project>
            </v-col>
        </v-row>
        <v-row v-if="!loading" class="mt-10">
            <all-project :date="date" :projects="Projects" :tags="selectedTags" :is-shared="filter == 1"></all-project>
        </v-row>
    </v-container>
</template>
