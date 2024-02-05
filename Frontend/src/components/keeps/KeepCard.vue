<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { Permission, RouterEnum } from '@/Models/enum'
import { UserStore } from '@/stores'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import {
    InfoKeep,
    EditKeep,
    DeleteKeep,
    InviteKeep,
    ManageUser
} from '@/components/keeps'

const { keep, project } = defineProps<{
    keep: IKeep,
    project: IProject
}>()

const router = useRouter()
const { User } = UserStore()
const canEdit = computed((): boolean => {
    if (project.createdBy == User.email) return true
    if (keep.createdBy == User.email) return true
    const projectUser = project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser)
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    return false
})
const canDelete = computed((): boolean => {
    if (project.createdBy == User.email) return true
    if (keep.createdBy == User.email) return true
    const projectUser = project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser)
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    return false
})
</script>
<template>
    <v-hover v-slot="{ props, isHovering }">
        <v-card v-bind="props" :elevation="isHovering ? 8 : 3" class="cursor-pointer"
            @click="router.push({ name: RouterEnum.ITEM, params: { id: keep.projectId, keepId: keep.id } })">
            <v-card-title class="bg-primary d-flex">
                <span class="text-truncate">{{ keep.title }}</span>
                <v-spacer></v-spacer>
                <v-menu location="bottom" width="250">
                    <template v-slot:default="{ isActive }">
                        <v-list>
                            <info-keep :keep="keep" @close="isActive.value = false" v-slot="{ activator: infoActivator }">
                                <v-list-item role="button" v-bind="infoActivator">
                                    <v-icon>mdi-information-outline</v-icon>
                                    <span class="mx-3">Info</span>
                                </v-list-item>
                            </info-keep>
                            <invite-keep v-if="!project.isShared" :keep="keep" :project="project"
                                @close="isActive.value = false" v-slot="{ activator: inviteActivator }">
                                <v-list-item role="button" v-bind="inviteActivator">
                                    <v-icon>mdi-account-plus-outline</v-icon>
                                    <span class="mx-3">Invite</span>
                                </v-list-item>
                            </invite-keep>
                            <manage-user v-if="!project.isShared" :keep="keep" @close="isActive.value = false"
                                v-slot="{ activator: manageUserActivator }">
                                <v-list-item v-bind="manageUserActivator">
                                    <v-icon>mdi-account-multiple-outline</v-icon>
                                    <span class="mx-3">Manage</span>
                                </v-list-item>
                            </manage-user>
                            <edit-keep :keep="keep" v-if="canEdit" @close="isActive.value = false"
                                v-slot="{ activator: editActivator }">
                                <v-list-item role="button" v-bind="editActivator">
                                    <v-icon>mdi-folder-edit-outline</v-icon>
                                    <span class="mx-3">Edit</span>
                                </v-list-item>
                            </edit-keep>
                            <delete-keep :id="keep.id" v-if="canDelete" @close="isActive.value = false"
                                v-slot="{ activator: deleteActivator }">
                                <v-list-item role="button" v-bind="deleteActivator">
                                    <v-icon>mdi-delete-outline</v-icon>
                                    <span class="mx-3">Delete</span>
                                </v-list-item>
                            </delete-keep>
                        </v-list>
                    </template>
                    <template v-slot:activator="{ props }">
                        <v-icon v-bind="props" color="white">mdi-dots-vertical</v-icon>
                    </template>
                </v-menu>
            </v-card-title>
            <v-card-actions>
                <v-chip v-if="keep.tag" class="bg-primary">#{{ keep.tag }}</v-chip>
            </v-card-actions>
        </v-card>
    </v-hover>
</template>