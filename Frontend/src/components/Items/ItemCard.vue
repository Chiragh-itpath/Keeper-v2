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
                <edit-item v-model:item="item" :keep="keep" :project="project" v-if="canEdit"
                    v-slot="{ activator: editActivator }">
                    <v-tooltip location="top">
                        <template v-slot:activator="{ props: tooltip }">
                            <v-icon v-bind="mergeProps(editActivator, tooltip)"
                                size="small">mdi-note-edit-outline</v-icon>
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
                                            v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                                            :icon="item.type == ItemType.MAIL ? 'mdi-email' : 'mdi-file'">
                                        </v-icon>
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
                                                @close="isActive.value = false" v-slot="{ activator: editeditActivator }">
                                                <v-list-item v-bind="editeditActivator">
                                                    <v-icon>mdi-note-edit-outline</v-icon>
                                                    <span class="mx-3">Edit</span>
                                                </v-list-item>
                                            </edit-item>
                                            <delete-item :id="item.id" v-if="canDelete" @close="isActive.value = false"
                                                v-slot="{ activator: deleteeditActivator }">
                                                <v-list-item role="button" v-bind="deleteeditActivator">
                                                    <v-icon>mdi-delete-outline</v-icon>
                                                    <span class="mx-3">Delete</span>
                                                </v-list-item>
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
                    <v-card-text class="pa-2">
                        <v-sheet height="100" class="pa-4 ellipsis">
                            <span v-html="item.description"></span>
                        </v-sheet>
                    </v-card-text>
                    <v-card-actions>
                        <update-status :item="item" :status="item.status" v-if="canEdit">
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
                        <v-spacer></v-spacer>
                        <div class="me-2">
                            <span v-if="item.to" class="d-flex cursor-pointer" v-bind="props">
                                <template v-for="(client, index) in  item.to.split(/,/) " :key="index">
                                    <span v-if="client.trim() && index < 3" style="width: 22px;">
                                        <v-avatar color="primary" size="small" class="avatar-border">
                                            <span v-if="index === 2 && item.to.split(/,/).length > 3">
                                                {{ `+${item.to.split(/,/).length - index}` }}
                                                <v-tooltip activator="parent" location="top">
                                                    {{ item.to.split(/,/).splice(2).join(',') }}
                                                </v-tooltip>
                                            </span>
                                            <span v-else>
                                                {{ client.trim().charAt(0).toUpperCase() }}
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
    border: 1.5px solid #00695C !important;
}
</style>