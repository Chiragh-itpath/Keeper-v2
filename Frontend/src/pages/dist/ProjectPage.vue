<script setup lang="ts">
import { ref, onMounted, type Ref, watch } from 'vue'
import { useDate } from 'vuetify'
import { storeToRefs } from 'pinia'
import { ProjectStore } from '@/stores'
import { AddProject, ProjectCard } from '@/components/Project'
import { DatePicker, TagSelector, NoItem } from '@/components/Custom'
import type { IProject } from '@/Models/ProjectModels'

const dateHelper = useDate()

const { Projects, ProjectTags } = storeToRefs(ProjectStore())
const date: Ref<undefined | Date | Date[]> = ref(undefined);
const loading: Ref<boolean> = ref(false)
const selectedTags: Ref<string[]> = ref([])
const filter = ref(0)
const ProjectsToDisplay: Ref<IProject[]> = ref([])
const filterFunction = (project: IProject) => {
    return (
        !date.value ||
        (Array.isArray(date.value) && date.value.length === 0) || isSameDate(new Date(project.createdOn), date.value)) &&
        (selectedTags.value.length === 0 || selectedTags.value.includes(project.tag)) &&
        (filter.value == 0 || project.isShared)
}
const isSameDate = (date1: Date, date2: Date | Date[]): boolean => {
    return Array.isArray(date2) ?
        date2.map(d => dateHelper.format(d, 'keyboardDate'))
            .includes(dateHelper.format(date1, 'keyboardDate')) :
        dateHelper.format(date1, 'keyboardDate') === dateHelper.format(date2, 'keyboardDate')
}
watch([selectedTags, date, filter], () => {
    ProjectsToDisplay.value = Projects.value.filter(filterFunction)
})
onMounted(async () => {
    loading.value = true
    await ProjectStore().GetProjects()
    ProjectsToDisplay.value = Projects.value
    loading.value = false
})
</script>
<template>
    <v-container class="pa-10 h-100" fluid>
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
                <date-picker v-model="date"></date-picker>
            </v-col>
            <v-col class="d-flex justify-end">
                <add-project></add-project>
            </v-col>
        </v-row>
        <v-row v-if="!loading" class="mt-10">
            <template v-for="( project, index ) in  ProjectsToDisplay " :key="index">
                <v-col cols="12" lg="3" md="4" sm="6">
                    <project-card :project="project"></project-card>
                </v-col>
            </template>
            <no-item v-if="ProjectsToDisplay.length == 0" title="No Project Found"
                :sub-title="date ? 'There is no project on this date' : filter == 1 ? 'There is no shared projects' : 'Please click on add button to insert new record'">
            </no-item>
        </v-row>
    </v-container>
</template>
