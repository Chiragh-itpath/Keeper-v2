<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import type { IProject } from '@/Models/ProjectModels'
import { ProjectStore, UserStore } from '@/stores'
import { ManageUser, StatusList, ClientList, RuleBook } from '@/components/Project'

const tabs = {
    manageUser: 'manage user',
    statusList: 'status list',
    clientList: 'client List',
    ruleBook: 'rule book'
} as const

const window = ref<typeof tabs[keyof typeof tabs]>('manage user')
const route = useRoute()
const projectStore = ProjectStore()
const project = ref<IProject>()

const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})

const { User } = UserStore()
const isOwner = computed(() => {
    return !!project.value && project.value.createdBy == User.email
})
onMounted(async () => {
    project.value = await projectStore.GetSingalProject(projectId.value)
})
</script>

<template>
    <v-container fluid>
        <template v-if="project">
            <v-row no-gutters>
                <v-col>
                    <v-breadcrumbs divider="/" :items="[
            {
                title: 'Projects',
                to: '/Project'
            },
            {
                title: 'Settings'
            }
        ]">
                    </v-breadcrumbs>
                </v-col>
                <v-col class="d-flex justify-end">
                    <v-tabs color="primary" v-model="window">
                        <v-tab :value="tabs.manageUser">Users</v-tab>
                        <v-tab :value="tabs.statusList">Status List</v-tab>
                        <v-tab :value="tabs.clientList">Client List</v-tab>
                        <v-tab :value="tabs.ruleBook">Rule Book</v-tab>
                    </v-tabs>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="12">
                    <manage-user v-if="window == 'manage user'" :project="project" :is-owner="isOwner" />
                    <status-list v-if="window == 'status list'" :project-id="project.id" :is-owner="isOwner" />
                    <client-list v-if="window == 'client List'" :project-id="project.id" :is-owner="isOwner" />
                    <rule-book v-if="window == 'rule book'" :project-id="project.id" :is-owner="isOwner" />
                </v-col>
            </v-row>
        </template>
    </v-container>
</template>