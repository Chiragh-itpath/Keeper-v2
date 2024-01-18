<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { useDate } from 'vuetify'
import { UserStore } from '@/stores'
import { EditItem, InfoItem, DeleteItem, UpdateStatus, StatusList, TypeList } from '@/components/Items/'
import { NoItem } from '@/components/Custom/'
import type { IItem } from '@/Models/ItemModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'
import { ItemStatus, ItemType, Permission } from '@/Models/enum'

const dateHelper = useDate()
const props = defineProps<{
    items: IItem[],
    date?: Date,
    project: IProject,
    keep: IKeep,
    itemType?: ItemType,
    itemStatus?: ItemStatus,
    itemOwner?: string
}>()
const itemToDisplay = ref(props.items)
const { User } = UserStore()
const id: Ref<string> = ref('')
const visible: Ref<boolean> = ref(false)

const canEdit = (index: number): boolean => {
    if (props.project.createdBy == User.email) return true
    if (itemToDisplay.value[index].createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    }
    const keepUser = props.keep.users.find(u => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.EDIT || keepUser.permission == Permission.ALL
    }
    return false
}
const canDelete = (index: number): boolean => {
    if (props.project.createdBy == User.email) return true
    if (itemToDisplay.value[index].createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser) {
        return projectUser.permission == Permission.ALL
    }
    const keepUser = props.keep.users.find(u => u.invitedUser.id == User.id)
    if (keepUser) {
        return keepUser.permission == Permission.ALL
    }
    return false
}
const redirectionHandler = (link?: string) => {
    if (link) window.open(link, '_blank')
}
const openModel = (_id: string) => {
    visible.value = true
    id.value = _id
}
watch(props, () => {
    itemToDisplay.value = props.items.filter(x => {
        return (
            (
                !props.date ||
                dateHelper.format(x.createdOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate') ||
                dateHelper.format(x.updatedOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate')
            ) &&
            (props.itemType == undefined || x.type == props.itemType) &&
            (props.itemStatus == undefined || x.status == props.itemStatus) &&
            (!props.itemOwner || x.createdBy == props.itemOwner || x.updatedBy == props.itemOwner)
        )
    })
})
</script>
<template>
    <info-item :id="id" v-model="visible"></info-item>
    <no-item v-if="itemToDisplay.length == 0" title="No Item Found" :sub-title="date ? 'No record found on this date' :
        'Please click on add button to insert new record'">
    </no-item>
    <v-col cols="12" lg="4" md="6" v-for="(item, index) of itemToDisplay" :key="index">
        <v-hover v-slot="{ isHovering, props }">
            <v-card v-bind="props" :elevation="isHovering ? 8 : 3">
                <v-card-title class="bg-primary">
                    <v-row class="align-center">
                        <v-col cols="auto" class="px-1">
                            <v-tooltip location="top">
                                <template v-slot:activator="{ props }">
                                    <v-chip class="me-1" v-bind="props"
                                        v-if="item.type == ItemType.TICKET || item.type == ItemType.PR"
                                        @click="() => !!item.url ? redirectionHandler(item.url) : openModel(item.id)">
                                        {{ item.type == 0 ? '#' : '!' }} {{ item.number }}
                                    </v-chip>
                                    <v-icon size="small" v-bind="props"
                                        v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                                        :icon="item.type == ItemType.MAIL ? 'mdi-email-outline' : 'mdi-file'"></v-icon>
                                </template>
                                {{ TypeList[item.type].title }}
                            </v-tooltip>
                        </v-col>
                        <v-col class="px-0 text-white text-truncate cursor-pointer" @click="openModel(item.id)">
                            {{ item.title }}
                        </v-col>
                        <v-col v-if="canEdit(index) || canDelete(index)" cols="auto" class="px-1">
                            <v-menu location="bottom" width="250">
                                <template v-slot="{ isActive }">
                                    <v-list>
                                        <edit-item :item="item" :keep="keep" :project="project" v-if="canEdit(index)"
                                            @close="isActive.value = false" />
                                        <delete-item :id="item.id" v-if="canDelete(index)"
                                            @close="isActive.value = false"></delete-item>
                                    </v-list>
                                </template>
                                <template v-slot:activator="{ props }">
                                    <v-icon v-bind="props" color="white">mdi-dots-vertical</v-icon>
                                </template>
                            </v-menu>
                        </v-col>
                    </v-row>
                </v-card-title>
                <v-card-text class="" @click="openModel(item.id)">
                    <v-sheet height="100" class="pa-4 text-truncate">
                        <div v-if="item.description">
                            <div v-html="item.description"></div>
                        </div>
                        <div v-else class="text-grey">No Description</div>
                    </v-sheet>
                </v-card-text>
                <v-card-actions>
                    <update-status :item="item" :status="item.status" v-if="canEdit(index)">
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
        </v-hover>
    </v-col>
</template>
