<script lang="ts" setup>
import { type Ref, ref, onMounted, watch } from 'vue'
import { useDate } from 'vuetify'
import { useRouter } from 'vue-router'
import { Permission, RouterEnum } from '@/Models/enum'
import { UserStore } from '@/stores'
import { EditKeep, InfoKeep, InviteKeep, DeleteKeep, ManageUser } from '@/components/keeps'
import { NoItem } from '@/components/Custom'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'

const dateHelper = useDate()
const props = withDefaults(defineProps<{
    keeps: IKeep[],
    date?: string | null | Date,
    tags: string[]
    project: IProject
}>(), {
    date: null
})

const router = useRouter()
const KeepsToDisplay: Ref<IKeep[]> = ref(props.keeps)
const { User } = UserStore()

const canEdit = (index: number): boolean => {
    if (props.project.createdBy == User.email) return true
    const keep = KeepsToDisplay.value[index]
    if (keep.createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser)
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    return false
}
const canDelete = (index: number): boolean => {
    if (props.project.createdBy == User.email) return true
    const keep = KeepsToDisplay.value[index]
    if (keep.createdBy == User.email) return true
    const projectUser = props.project.users.find(u => u.invitedUser.id == User.id)
    if (projectUser)
        return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
    return false
}
watch(props, () => {
    const filterFunction = (keep: IKeep) => {
        return (
            !props.date ||
            dateHelper.format(props.date, 'keyboardDate') == dateHelper.format(keep.createdOn, 'keyboardDate')) &&
            (props.tags.length == 0 || props.tags.includes(keep.tag))
    }
    KeepsToDisplay.value = props.keeps.filter(filterFunction);
})

onMounted(() => {
    KeepsToDisplay.value = props.keeps
})
</script>
<template>
    <no-item v-if="KeepsToDisplay.length == 0" title="No Keep Found"
        :sub-title="(date) ? 'There is no record on this date' : 'Please click on add button to insert new record'">
    </no-item>
    <v-col cols="12" lg="3" md="4" sm="6" xl="2" v-for="(keep, index) in KeepsToDisplay" :key="index">
        <v-hover v-slot="{ props, isHovering }">
            <v-card v-bind="props" :elevation="isHovering ? 8 : 3" class="cursor-pointer"
                @click="router.push({ name: RouterEnum.ITEM, params: { id: keep.projectId, keepId: keep.id } })">
                <v-card-title class="bg-primary d-flex">
                    <span class="text-truncate">{{ keep.title }}</span>
                    <v-spacer></v-spacer>
                    <v-menu location="bottom" width="250">
                        <template v-slot:default="{ isActive }">
                            <v-list>
                                <info-keep :keep="keep" @close="isActive.value = false" />
                                <invite-keep v-if="!project.isShared" :keep="keep" :project="project"
                                    @close="isActive.value = false" />
                                <manage-user v-if="!project.isShared" :keep="keep" @close="isActive.value = false" />
                                <edit-keep :keep="keep" v-if="canEdit(index)" @close="isActive.value = false" />
                                <delete-keep :id="keep.id" v-if="canDelete(index)" @close="isActive.value = false" />
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
    </v-col>
</template>
