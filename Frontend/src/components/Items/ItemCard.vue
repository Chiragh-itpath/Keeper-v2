<script setup lang="ts">
import { computed, mergeProps, ref, watch } from 'vue'
import { EditItem, DeleteItem, TypeList, StatusList, UpdateStatus, InfoItem } from '@/components/Items'
import { ItemType, Permission } from '@/Models/enum'
import type { IItem } from '@/Models/ItemModels'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import { UserStore } from '@/stores'

const props = defineProps<{
    project: IProject,
    keep: IKeep,
    item: IItem,
    canEdit: boolean,
    canDelete: boolean
}>()
const { User } = UserStore()
const item = ref(props.item)
const canEdit = computed((): boolean => {
    if (props.project.createdBy == User.email) return true
    if (props.item.createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    }
    const keepUser = props.keep.users.find(u => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.EDIT || keepUser.permission == Permission.ALL
    }
    return false
})

const canDelete = computed((): boolean => {
    if (props.project.createdBy == User.email) return true
    if (props.item.createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.ALL
    }
    const keepUser = props.keep.users.find(u => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.ALL
    }
    return false
})
watch(props, () => {
    item.value = props.item
})
</script>
<template>
    <v-hover v-slot="{ props: hover, isHovering }">
        <info-item :item="item">
            <template v-slot:edit>
                <edit-item v-model:item="item" :keep="keep" :project="project" v-if="canEdit" v-slot="{ props: edit }">
                    <v-tooltip location="top">
                        <template v-slot:activator="{ props: tooltip }">
                            <v-icon v-bind="mergeProps(edit, tooltip)" size="small">mdi-folder-edit-outline</v-icon>
                        </template>
                        Edit
                    </v-tooltip>
                </edit-item>
            </template>
            <template v-slot="{ props: info }">
                <v-card v-bind="mergeProps(hover, info)" :elevation="isHovering ? 8 : 3" class="cursor-pointer"
                    :ripple="false">
                    <v-card-title class="bg-primary">
                        <v-row class="align-center">
                            <v-col cols="auto" class="px-1">
                                <v-tooltip location="top">
                                    <template v-slot:activator="{ props }">
                                        <v-chip class="me-1" v-bind="props"
                                            v-if="item.type == ItemType.TICKET || item.type == ItemType.PR">
                                            {{ item.type == 0 ? '#' : '!' }} {{ item.number }}
                                        </v-chip>
                                        <v-icon size="small" v-bind="props"
                                            v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                                            :icon="item.type == ItemType.MAIL ? 'mdi-email-outline' : 'mdi-file'"></v-icon>
                                    </template>
                                    {{ TypeList[item.type].title }}
                                </v-tooltip>
                            </v-col>
                            <v-col class="px-0 text-white text-truncate cursor-pointer">
                                {{ item.title }}
                            </v-col>
                            <v-col v-if="canEdit || canDelete" cols="auto" class="px-1">
                                <v-menu location="bottom" width="250">
                                    <template v-slot="{ isActive }">
                                        <v-list>
                                            <edit-item :item="item" :keep="keep" :project="project" v-if="canEdit"
                                                @close="isActive.value = false" v-slot="{ props }">
                                                <v-list-item v-bind="props">
                                                    <v-icon>mdi-folder-edit-outline</v-icon>
                                                    <span class="mx-3">Edit</span>
                                                </v-list-item>
                                            </edit-item>
                                            <delete-item :id="item.id" v-if="canDelete" @close="isActive.value = false">
                                            </delete-item>
                                        </v-list>
                                    </template>
                                    <template v-slot:activator="{ props }">
                                        <v-icon v-bind="props" color="white">mdi-dots-vertical</v-icon>
                                    </template>
                                </v-menu>
                            </v-col>
                        </v-row>
                    </v-card-title>
                    <v-card-text>
                        <v-sheet height="100" class="pa-4 text-truncate">
                            <div v-if="item.description">
                                <div v-html="item.description"></div>
                            </div>
                            <div v-else class="text-grey">No Description</div>
                        </v-sheet>
                    </v-card-text>
                    <v-card-actions>
                        <update-status :item="item" v-model:status="item.status" v-if="canEdit">
                            <template v-slot="{ props, isActive }">
                                <v-chip color="primary" variant="flat" v-bind="props">
                                    {{ StatusList[item.status].title }}
                                    <template v-slot:append>
                                        <v-icon>{{ (isActive ? 'mdi-menu-up' : 'mdi-menu-down') }}</v-icon>
                                    </template>
                                </v-chip>
                            </template>
                        </update-status>
                        <v-chip color="primary" variant="flat" v-else>
                            {{ StatusList[item.status].title }}
                        </v-chip>
                    </v-card-actions>
                </v-card>
            </template>

        </info-item>
    </v-hover>
</template>