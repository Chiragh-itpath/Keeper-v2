<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { UserStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import { ItemType, Permission } from '@/Models/enum'
import { StatusList, TypeList, UpdateStatus, InfoItem, EditItem } from '@/components/Items'
const props = defineProps<{
    item: IItem,
    project: IProject,
    keep: IKeep
}>()
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
</script>
<template>
    <info-item :item="item">
        <template v-slot:edit>
            <edit-item v-model:item="item" :keep="keep" :project="project" v-if="canEdit">
                <template v-slot="{ activator }">
                    <v-icon v-bind="activator">mdi-folder-edit-outline</v-icon>
                </template>
            </edit-item>
        </template>
        <template v-slot="{ activator }">
            <v-row class="border-b" v-bind="activator">
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
                        <template v-else-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                            v-slot:activator="{ props }">
                            <v-icon color="primary" v-bind="props"
                                :icon="item.type == ItemType.MAIL ? 'mdi-email-outline' : 'mdi-file-outline'">
                            </v-icon>
                            {{ item.title }}
                        </template>
                        {{ TypeList[item.type].title }}
                    </v-tooltip>
                </v-col>
                <v-col cols="7" class="py-1">
                    <v-sheet max-height="110" class="ellipsis" @click.stop>
                        <span v-html="item.description" class="description"></span>
                    </v-sheet>
                </v-col>
                <v-col cols="1">
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
                </v-col>
                <v-col cols="1">
                    <template v-if="item.discussedBy">
                        <v-avatar color="primary" size="small" class="cursor-pointer">
                            <v-tooltip activator="parent" location="top">
                                {{ item.discussedBy }}
                            </v-tooltip>
                            {{ item.discussedBy.substring(0, 1).toUpperCase() }}
                        </v-avatar>
                    </template>
                </v-col>
                <v-col cols="2">
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
</style>