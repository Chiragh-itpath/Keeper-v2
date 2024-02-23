<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import type { IProject } from '@/Models/ProjectModels'
import { ProjectStore } from '@/stores'
import { ManageUser, StatusList, ClientList, RuleBook } from '@/components/Project'

const window = ref(3)
const route = useRoute()
const projectStore = ProjectStore()
const project = ref<IProject>()
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
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
                        <v-tab value="0">Users</v-tab>
                        <v-tab value="1">Status List</v-tab>
                        <v-tab value="2">Client List</v-tab>
                        <v-tab value="3">Rule Book</v-tab>
                    </v-tabs>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="12">
                    <manage-user :project="project" v-if="window == 0" />
                    <status-list v-if="window == 1" />
                    <client-list v-if="window == 2" />
                    <rule-book v-if="window == 3" />
                </v-col>
            </v-row>
        </template>
    </v-container>
</template>