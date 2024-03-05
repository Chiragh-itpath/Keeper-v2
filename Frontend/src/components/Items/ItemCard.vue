<script setup lang="ts">
import { computed, mergeProps, ref, watch } from 'vue'
import { EditItem, DeleteItem, TypeList, UpdateStatus, InfoItem } from '@/components/Items'
import { ItemType, Permission } from '@/Models/enum'
import type { IItem } from '@/Models/ItemModels'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import { UserStore } from '@/stores'
import { useMenu } from '@/composable/useMenu'
import type { IStatus } from '@/Models/ProjectSettings'

type ListItem = { title: string; subtitle?: string; value: string }
const { keep, project, item, statusList } = defineProps<{
    project: IProject
    keep: IKeep
    item: IItem
    clientList: ListItem[],
    statusList: IStatus[]
}>()
const { menu, menuHide, close } = useMenu()
const { User } = UserStore()
const _item = ref(item)
const canEdit = computed((): boolean => {
    if (project.createdBy == User.email) return true
    if (item.createdBy == User.email) return true
    const projectUser = project.users.find((u) => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    }
    const keepUser = keep.users.find((u) => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.EDIT || keepUser.permission == Permission.ALL
    }
    return false
})
const canDelete = computed((): boolean => {
    if (project.createdBy == User.email) return true
    if (item.createdBy == User.email) return true
    const projectUser = project.users.find((u) => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.ALL
    }
    const keepUser = keep.users.find((u) => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.ALL
    }
    return false
})
watch(item, () => {
    _item.value = item
})
const getStatusTitle = (statusId: string): string => {
    const status = statusList.find(x => x.id == statusId)
    return status ? status.title : ''
}
</script>

<template>
    <v-hover v-slot="{ props: hover, isHovering }">
        <info-item :item="item">
            <template v-slot:edit>
                <edit-item v-model:item="_item" :keep="keep" :project="project" :client-list="clientList" v-if="canEdit"
                    v-slot="{ activator: editActivator }">
                    <v-tooltip location="top">
                        <template v-slot:activator="{ props: tooltip }">
                            <v-icon v-bind="mergeProps(editActivator, tooltip)" size="small"
                                icon="mdi-note-edit-outline" />
                        </template>
                        Edit
                    </v-tooltip>
                </edit-item>
            </template>

            <template v-slot="{ activator: info }">
                <v-card v-bind="mergeProps(hover, info)" :elevation="isHovering ? 8 : 3" class="cursor-pointer"
                    :ripple="false">
                    <v-card-title class="bg-primary">
                        <v-row class="align-center">
                            <v-col cols="auto" class="px-1">
                                <v-tooltip location="top">
                                    <template v-slot:activator="{ props }">
                                        <v-chip class="me-1" v-bind="props"
                                            v-if="item.type == ItemType.TICKET || item.type == ItemType.PR">
                                            <a :href="item.url" target="_blank" class="text-white" @click.stop>
                                                {{ item.type == 0 ? '#' : '!' }} {{ item.number }}
                                            </a>
                                        </v-chip>
                                        <v-icon size="small" v-bind="props"
                                            :icon="item.type == ItemType.MAIL ? 'mdi-email' : 'mdi-file'"
                                            v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL">
                                        </v-icon>
                                    </template>
                                    {{ TypeList[item.type].title }}
                                </v-tooltip>
                            </v-col>
                            <v-col class="px-0 text-white text-truncate cursor-pointer">
                                {{ item.title }}
                            </v-col>
                            <v-col v-if="canEdit || canDelete" cols="auto" class="px-1">
                                <v-menu location="bottom" width="250" v-model="menu" :class="[{ invisible: menuHide }]">
                                    <v-list>
                                        <edit-item :item="item" :keep="keep" :project="project"
                                            :client-list="clientList" v-if="canEdit" @close="close"
                                            v-slot="{ activator: editeditActivator }">
                                            <v-list-item v-bind="editeditActivator" @click="menuHide = true">
                                                <v-icon>mdi-note-edit-outline</v-icon>
                                                <span class="mx-3">Edit</span>
                                            </v-list-item>
                                        </edit-item>
                                        <delete-item :id="item.id" v-if="canDelete" @close="close"
                                            v-slot="{ activator: deleteeditActivator }">
                                            <v-list-item role="button" v-bind="deleteeditActivator"
                                                @click="menuHide = true">
                                                <v-icon>mdi-delete-outline</v-icon>
                                                <span class="mx-3">Delete</span>
                                            </v-list-item>
                                        </delete-item>
                                    </v-list>

                                    <template v-slot:activator="{ props }">
                                        <v-icon v-bind="props" color="white">mdi-dots-vertical</v-icon>
                                    </template>
                                </v-menu>
                            </v-col>
                        </v-row>
                    </v-card-title>
                    <v-card-text class="pa-2" @click.stop>
                        <v-sheet height="100" class="pa-4 ellipsis">
                            <span v-html="item.description"></span>
                        </v-sheet>
                    </v-card-text>
                    <v-card-actions>
                        <update-status :item="item" :status-list="statusList" v-if="canEdit">

                            <template v-slot="{ props, isActive }">
                                <v-chip color="primary" variant="flat" v-bind="props">
                                    {{ getStatusTitle(item.statusId) }}
                                    <template v-slot:append>
                                        <v-icon :icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'" />
                                    </template>
                                </v-chip>
                            </template>
                        </update-status>
                        <v-chip color="primary" variant="flat" v-else>
                            {{ getStatusTitle(item.statusId) }}
                        </v-chip>
                        <v-spacer></v-spacer>
                        <div class="me-2">
                            <span v-if="item.to" class="d-flex cursor-pointer">

                                <template v-for="(client, index) in item.to.split(/,/).map(x => x.trim())" :key="index">
                                    <span v-if="client.trim() && index < 3" style="width: 22px">
                                        <v-avatar color="primary" size="small" class="avatar-border">
                                            <span v-if="index === 2 && item.to.split(/,/).length > 3">
                                                {{ `+${item.to.split(/,/).length - index}` }}
                                                <v-tooltip activator="parent" location="top">
                                                    {{ item.to.split(/,/).splice(2).join(',') }}
                                                </v-tooltip>
                                            </span>
                                            <span v-else>
                                                {{ client.split(' ').splice(0, 2).map((x) =>
        x[0].toUpperCase()).join('') }}
                                                <v-tooltip activator="parent" location="top">
                                                    {{ client.trim() }}
                                                </v-tooltip>
                                            </span>
                                        </v-avatar>
                                    </span>
                                </template>
                            </span>
                        </div>
                    </v-card-actions>
                </v-card>
            </template>
        </info-item>
    </v-hover>
</template>

<style>
.ellipsis {
    display: -webkit-box;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    -webkit-line-clamp: 4;
    -webkit-box-orient: vertical;
    white-space: normal;
}

.avatar-border {
    border: 1.5px solid #00695c !important;
}
</style>
