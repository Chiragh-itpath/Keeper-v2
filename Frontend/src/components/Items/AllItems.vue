<script setup lang="ts">
import { ref, watch, type Ref, reactive } from 'vue'
import { useDate } from 'vuetify'
import { UserStore } from '@/stores'
import { EditItem, InfoItem, DeleteItem } from '@/components/Items/'
import { NoItem } from '@/components/Custom/'
import type { IItem } from '@/Models/ItemModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'
import { Permission } from '@/Models/enum'

const dateHelper = useDate()
const props = defineProps<{
    items: IItem[],
    date: Date | null | string,
    project: IProject,
    keep: IKeep
}>()
const itemToDisplay = ref(props.items)
const { User } = UserStore()
const id: Ref<string> = ref('')

const visible = reactive<{
    info: boolean
}>({
    info: false
})


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
watch(props, () => {
    itemToDisplay.value = props.items.filter(x => {
        return (
            !props.date ||
            dateHelper.format(x.createdOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate')
        )
    })
})
</script>
<template>
    <info-item :id="id" v-model="visible.info"></info-item>
    <no-item v-if="itemToDisplay.length == 0" title="No Item Found"
        :sub-title="date ? 'No record found on this date' : 'Please click on add button to insert new record'">
    </no-item>
    <v-col cols="12" lg="4" md="6" v-for="(item, index) of itemToDisplay" :key="index">
        <v-hover v-slot="{ isHovering, props }">
            <v-card v-bind="props" :elevation="isHovering ? 8 : 3" @click="() => { visible.info = true; id = item.id }">
                <v-card-title class="bg-primary d-flex">
                    <span class="text-white text-truncate">{{ item.title }}</span>
                    <v-spacer></v-spacer>
                    <div v-if="canEdit(index) || canDelete(index)">
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
                    </div>
                </v-card-title>
                <v-card-text class="">
                    <v-sheet height="100" class="pa-4 text-truncate">
                        <div v-if="item.description">
                            <div v-html="item.description"></div>
                        </div>
                        <div v-else class="text-grey">No Description</div>
                    </v-sheet>
                </v-card-text>
            </v-card>
        </v-hover>
    </v-col>
</template>
