<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { UserStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import { ItemType, Permission } from '@/Models/enum'
import { TypeList, UpdateStatus, InfoItem, EditItem } from '@/components/Items'
import { useTheme } from '@/composable/useTheme'
import type { IStatus } from '@/Models/ProjectSettings'

type ListItem = { title: string; subtitle?: string; value: string }
const props = defineProps<{
    item: IItem,
    project: IProject,
    keep: IKeep,
    clientList: ListItem[],
    statusList: IStatus[]
}>()
const { dark } = useTheme()
const item = ref(props.item)
const { User } = UserStore()
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
watch(props, () => {
    item.value = props.item
})
const getStatusTitle = (statusId: string): string => {
    const status = props.statusList.find(x => x.id == statusId)
    return status ? status.title : ''
}
</script>

<template>
    <info-item :item="item">
        <template v-slot:edit>
            <edit-item v-model:item="item" :keep="keep" :project="project" :client-list="clientList" v-if="canEdit">
                <template v-slot="{ activator }">
                    <v-icon v-bind="activator">mdi-note-edit-outline</v-icon>
                </template>
            </edit-item>
        </template>

        <template v-slot="{ activator }">
            <v-row class="border-b" :class="[{ 'bg-background': dark }]" v-bind="activator">
                <v-col cols="1">
                    <v-tooltip location="top">
                        <template v-if="item.type == ItemType.TICKET || item.type == ItemType.PR"
                            v-slot:activator="{ props }">
                            <v-chip color="primary" variant="flat" v-bind="props">
                                <a :href="item.url" target="_blank" rel="noopener noreferrer" class="text-white"
                                    @click.stop>
                                    {{ item.type == ItemType.TICKET ? '#' : '!' }}
                                    {{ item.number }}
                                </a>
                            </v-chip>
                        </template>

                        <template
                            v-else-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL || item.type == ItemType.CUSTOM"
                            v-slot:activator="{ props }">
                            <v-icon color="primary" v-bind="props"
                                v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                                :icon="item.type == ItemType.MAIL ? 'mdi-email-outline' : 'mdi-file-outline'">
                            </v-icon>
                            {{ item.title }}
                        </template>
                        {{ TypeList[item.type].title }}
                    </v-tooltip>
                </v-col>
                <v-col cols="1" class="text-truncate">
                    <v-tooltip activator="parent" location="top" max-width="250">
                        <span class="word-wrap">
                            {{ item.title }}
                        </span>
                    </v-tooltip>
                    {{ item.title }}
                </v-col>
                <v-col class="py-1">
                    <v-sheet max-height="110" class="ellipsis bg-transparent" @click.stop>
                        <span v-html="item.description" class="description" :class="[{ 'text-white': dark }]"></span>
                    </v-sheet>
                </v-col>
                <v-col cols="1">
                    <span v-if="item.to" class="d-flex cursor-pointer" v-bind="props">

                        <template v-for="(client, index) in  item.to.split(/,/).map(x => x.trim())" :key="index">
                            <span v-if="client.trim() && index < 3" style="width: 22px;">
                                <v-avatar color="primary" size="small" class="avatar-border">
                                    <span v-if="index === 2 && item.to.split(/,/).length > 3">
                                        {{ `+${item.to.split(/,/).length - index}` }}
                                        <v-tooltip activator="parent" location="top">
                                            {{ item.to.split(/,/).splice(2).join(',') }}
                                        </v-tooltip>
                                    </span>
                                    <span v-else>
                                        {{ client.split(' ').splice(0, 2).map(x => x.charAt(0).toUpperCase()).join('')
                                        }}
                                        <v-tooltip activator="parent" location="top">
                                            {{ client.trim() }}
                                        </v-tooltip>
                                    </span>
                                </v-avatar>
                            </span>
                        </template>
                    </span>
                </v-col>
                <v-col cols="1">

                    <template v-if="item.discussedBy">
                        <v-avatar color="primary" size="small" class="cursor-pointer avatar-border">
                            <v-tooltip activator="parent" location="top">
                                {{ item.discussedBy }}
                            </v-tooltip>
                            {{ item.discussedBy.split(' ').splice(0, 2).map(x => x.charAt(0).toUpperCase()).join('') }}
                        </v-avatar>
                    </template>
                </v-col>
                <v-col cols="2" class="d-flex justify-end">
                    <update-status :item="item" :status-list="statusList" v-if="canEdit">

                        <template v-slot="{ props, isActive }">
                            <v-chip color="primary" variant="flat" v-bind="props">
                                <span class="text-truncate overflow-hidden" style="max-width: 100px;">
                                    {{ getStatusTitle(item.statusId) }}
                                </span>
                                <template v-slot:append>
                                    <v-icon>{{ (isActive ? 'mdi-menu-up' : 'mdi-menu-down') }}</v-icon>
                                </template>
                            </v-chip>
                        </template>
                    </update-status>
                    <v-chip color="primary" variant="flat" v-else>
                        {{ getStatusTitle(item.statusId) }}
                    </v-chip>
                </v-col>
            </v-row>
        </template>
    </info-item>
</template>

<style>
.description>ol,
.description>ul {
    margin: 0 20px;
}

.word-wrap {
    word-wrap: break-word;
}
</style>