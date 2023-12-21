<script lang="ts" setup>
import { type Ref, ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { RouterEnum } from '@/Models/enum'
import { KeepStore } from '@/stores'
import { EditKeep, InfoKeep, InviteKeep } from '@/components/keeps'
import { HoverEffect, NoItem, DeletePropmt } from '@/components/Custom'
import type { IKeep } from '@/Models/KeepModels'
import { useDate } from 'vuetify'
const dateHelper = useDate()
const props = withDefaults(defineProps<{
    keeps: IKeep[],
    date?: string | null,
    tags: string[]
}>(), {
    date: null
})

const router = useRouter()
const id: Ref<string> = ref('')
const keep: Ref<IKeep | undefined> = ref()
const KeepsToDisplay: Ref<IKeep[]> = ref(props.keeps)

const projectId: Ref<string> = ref('')
const editVisible: Ref<boolean> = ref(false)
const deleteVisible: Ref<boolean> = ref(false)
const infoVisible: Ref<boolean> = ref(false)
const inviteVisible: Ref<boolean> = ref(false)
const { DeleteKeep } = KeepStore()

const deleteHandler = async (): Promise<void> => {
    await DeleteKeep(id.value)
    deleteVisible.value = false
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
                        <v-list>
                            <v-list-item role="button" class="ma-0 pa-0"
                                @click="() => { infoVisible = true; id = keep.id; projectId = keep.projectId }">
                                <hover-effect icon="information-outline" icon-color="info">
                                    Info
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0"
                                @click="() => { inviteVisible = true; id = keep.id; projectId = keep.projectId }">
                                <hover-effect icon="account-plus-outline" icon-color="info">
                                    Invite
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                @click="() => { editVisible = true; id = keep.id }">
                                <hover-effect icon="folder-edit-outline" icon-color="edit">
                                    Edit
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                @click="() => { deleteVisible = true; id = keep.id }">
                                <hover-effect icon="delete-outline" icon-color="delete">
                                    Delete
                                </hover-effect>
                            </v-list-item>
                        </v-list>
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
    <edit-keep v-model="editVisible" :id="id" :project-id="projectId"></edit-keep>
    <delete-propmt v-model="deleteVisible" @click:yes="deleteHandler">Keep</delete-propmt>
    <invite-keep v-model="inviteVisible" :id="id" :project-id="projectId"></invite-keep>
    <info-keep v-model="infoVisible" :id="id" :keep="keep"></info-keep>
</template>
