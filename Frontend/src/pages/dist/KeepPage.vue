<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { AddKeep, AllKeeps } from '@/components/keeps'
import { DatePicker, TagSelector, NoItem } from '@/components/Custom'
import { KeepStore, ProjectStore, UserStore } from '@/stores'
import type { IProject } from '@/Models/ProjectModels'
import { Permission } from '@/Models/enum'

const date = ref()
const route = useRoute()
const loading: Ref<boolean> = ref(false)
const { User } = UserStore()
const selectedTags: Ref<string[]> = ref([])
const project: Ref<IProject | undefined> = ref()
const { Keeps, keepTags } = storeToRefs(KeepStore())
const router = useRouter()
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const hasAccess = computed((): boolean => {
    return (
        (project.value?.users.some(u => u.invitedUser.id == UserStore().User.id) ?? false) ||
        Keeps.value.length > 0
    )
})
const canCreate = (): boolean => {
    if (!project.value) return false
    if (project.value.createdBy == User.email) return true
    const projectUser = project.value.users.find(u => u.invitedUser.id == User.id)
    if (!projectUser) return false
    return projectUser.permission == Permission.CREATE || projectUser.permission == Permission.ALL
}
onMounted(async () => {
    loading.value = true
    await KeepStore().GetKeeps(projectId.value)
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    loading.value = false
    if (!hasAccess.value) router.go(-1)
})
</script>
<template>
    <v-container class="overflow-auto px-10" fluid>
        <v-row v-if="loading" class="mt-10">
            <v-col v-for="i in 16" :key="i" cols="12" sm="6" md="4" lg="3">
                <v-skeleton-loader type="text,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project">
            <v-col cols="12">
                <v-breadcrumbs divider="/" class="px-0" :items="[
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
            <v-col class="my-auto d-flex justify-end">
                <add-keep :project-id="projectId" v-if="canCreate()"></add-keep>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project" class="mt-10">
            <all-keeps :date="date" :keeps="Keeps" :tags="selectedTags" :project="project"></all-keeps>
        </v-row>
        <v-row v-if="!loading && !project" class="mt-15">
            <no-item title="No Project Found with this id" back-button></no-item>
        </v-row>
    </v-container>
</template>
