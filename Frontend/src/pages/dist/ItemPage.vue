<script setup lang="ts">
import { computed, ref, onMounted, type Ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AddItem, AllItems, ItemFilter } from '@/components/Items'
import { DatePicker, NoItem } from '@/components/Custom'
import { ItemStore, ProjectStore, KeepStore, UserStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import { ItemStatus, ItemType, Permission } from '@/Models/enum'

const route = useRoute()
const { GetAllItems } = ItemStore()
const { Items } = storeToRefs(ItemStore())
const { User } = UserStore()
const loading: Ref<boolean> = ref(false)
const project: Ref<IProject | undefined> = ref()
const keep: Ref<IKeep | undefined> = ref()
const router = useRouter()
const filters = reactive<{
    date?: Date,
    itemType?: ItemType,
    itemStatus?: ItemStatus,
    itemOwner?: string
}>({})
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const keepId = computed(() => {
    const id = route.params.keepId
    return Array.isArray(id) ? id.join('') : id
})
const hasAccess = computed((): boolean => {
    return (
        (project.value?.createdBy == User.email) ||
        (project.value?.users.some(u => u.invitedUser.id == User.id && u.isAccepted) ?? false) ||
        (keep.value?.users.some(u => u.invitedUser.id == User.id && u.isAccepted) ?? false)
    )
})
const users = computed(() => {
    const _users: { title: string, value: string }[] = []
    if (project.value) {
        _users.push(
            ...project.value.users.map(x => {
                return {
                    title: x.invitedUser.userName,
                    value: x.invitedUser.email
                }
            })
        )
    }
    if (keep.value) {
        _users.push(
            ...keep.value.users.map(x => {
                return {
                    title: x.invitedUser.userName,
                    value: x.invitedUser.email
                }
            })
        )
    }
    return _users
})
const breadcrumbsItems = [
    {
        title: 'Projects',
        disabled: false,
        to: '/Project'
    },
    {
        title: 'Keeps',
        disabled: false,
        to: `/Project/${projectId.value}`
    },
    {
        title: 'Item',
        disabled: true
    }
]
const canCreate = (): boolean => {
    if (!project.value) return false
    if (!keep.value) return false
    if (project.value.createdBy == User.email) return true
    const projectUser = project.value.users.find(u => u.invitedUser.id == User.id)
    const keepUser = keep.value.users.find(u => u.invitedUser.id == User.id)
    return (
        projectUser?.permission == Permission.CREATE ||
        projectUser?.permission == Permission.ALL ||
        keepUser?.permission == Permission.CREATE ||
        keepUser?.permission == Permission.ALL
    )
}
onMounted(async () => {
    loading.value = true
    await GetAllItems(keepId.value)
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    keep.value = await KeepStore().getSingleKeep(keepId.value)
    if (!hasAccess.value) router.go(-1)
    else loading.value = false
})
</script>
<template>
    <v-container class="px-10 pt-5" fluid>
        <v-row v-if="loading">
            <v-col v-for=" i  in  4" :key="i" cols="12" md="6">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project && keep" class="align-center flex-wrap">
            <v-col cols="12">
                <v-breadcrumbs divider="/" :items="breadcrumbsItems"></v-breadcrumbs>
            </v-col>
            <v-col cols="auto">
                <date-picker label="Select a date" v-model="filters.date"></date-picker>
            </v-col>
            <item-filter v-model:item-type="filters.itemType" v-model:item-status="filters.itemStatus" :users="users"
                v-model:item-owner="filters.itemOwner">
            </item-filter>
            <v-col>
                <add-item :keep="keep" :project="project" v-if="canCreate()"></add-item>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project && keep">
            <all-items :items="Items" :project="project" :keep="keep" :date="filters.date" :item-type="filters.itemType"
                :item-status="filters.itemStatus" :item-owner="filters.itemOwner"></all-items>
        </v-row>
        <v-row v-if="!loading && (!project || !keep)">
            <no-item :title="!project ? 'No Project found with this id' : 'No Keep found with this id'"></no-item>
        </v-row>
    </v-container>
</template>
