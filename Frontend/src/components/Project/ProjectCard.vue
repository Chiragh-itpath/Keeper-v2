<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { UserStore } from '@/stores'
import type { IProject } from '@/Models/ProjectModels'
import { Permission, RouterEnum } from '@/Models/enum'
import {
    EditProject,
    InfoProject,
    DeleteProject,
    InviteProject
} from '@/components/Project'
import { useMenu } from '@/composable/useMenu'

const { project } = defineProps<{
    project: IProject
}>()
const { menu, menuHide, close } = useMenu()
const router = useRouter()
const { User } = UserStore()
const canEdit = computed((): boolean => {
    if (project.createdBy == User.email) return true
    const projectUser = project.users.find(u => u.invitedUser.id == User.id)
    if (!projectUser) return false
    return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
})
</script>
<template>
    <v-hover v-slot="{ isHovering, props }">
        <v-card :elevation="isHovering ? 8 : 3" v-bind="props" class="cursor-pointer"
            @click="router.push({ name: RouterEnum.KEEP, params: { id: project.id } })">
            <v-card-title class="bg-primary d-flex">
                <span class="text-truncate">{{ project.title }}</span>
                <v-spacer></v-spacer>
                <v-menu v-model="menu" location="right" width="250" :class="[{ 'invisible': menuHide }]">
                    <template v-slot:activator="{ props: menu }">
                        <v-icon v-bind="menu" color="grey-lighten-4">mdi-dots-vertical</v-icon>
                    </template>
                    <v-list>
                        <info-project :project="project" @close="close" v-slot="{ activator: infoActivator }">
                            <v-list-item v-bind="infoActivator" @click="menuHide = true">
                                <v-icon>mdi-information-outline</v-icon>
                                <span class="mx-3">Info</span>
                            </v-list-item>
                        </info-project>
                        <invite-project :project="project" v-if="!project.isShared" @close="close"
                            v-slot="{ activator: inviteActivator }" @click="menuHide = true">
                            <v-list-item v-bind="inviteActivator" @click="menuHide = true">
                                <v-icon>mdi-account-plus-outline</v-icon>
                                <span class="mx-3">Invite</span>
                            </v-list-item>
                        </invite-project>
                        <v-list-item
                            @click="router.push({ name: RouterEnum.PROJECT_SETTINGS, params: { id: project.id } })">
                            <v-icon>mdi-folder-wrench-outline</v-icon>
                            <span class="mx-3">Settings</span>
                        </v-list-item>
                        <edit-project :project="project" v-if="canEdit" @close="close"
                            v-slot="{ activator: editActivator }">
                            <v-list-item v-bind="editActivator" @click="menuHide = true">
                                <v-icon>mdi-folder-edit-outline</v-icon>
                                <span class="mx-3">Edit</span>
                            </v-list-item>
                        </edit-project>
                        <delete-project :id="project.id" v-if="!project.isShared" @close="close"
                            v-slot="{ activator: deleteActivator }">
                            <v-list-item v-bind="deleteActivator" @click="menuHide = true">
                                <v-icon>mdi-delete-outline</v-icon>
                                <span class="mx-3">Delete</span>
                            </v-list-item>
                        </delete-project>
                    </v-list>
                </v-menu>
            </v-card-title>
            <v-card-text>
                <v-sheet height="100" class="mt-3 ">
                    <span v-if="project.description">
                        {{ project.description }}
                    </span>
                    <span v-else class="text-grey">
                        No Description
                    </span>
                </v-sheet>
            </v-card-text>
            <v-card-actions>
                <v-chip v-if="project.tag" class="bg-primary">{{ project.tag }}</v-chip>
                <v-spacer></v-spacer>
                <v-icon color="primary" v-if="project.isShared">mdi-account-multiple-outline</v-icon>
            </v-card-actions>
        </v-card>
    </v-hover>
</template>