<script setup lang="ts">
import { computed, ref, onMounted, type Ref, reactive, watch } from 'vue'
import { useDate, useDisplay } from 'vuetify'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { DatePicker, NoItem } from '@/components/Custom'
import { AddItem, ItemFilter, ItemCard, ItemGrid } from '@/components/Items'
import { ItemStore, ProjectStore, KeepStore, UserStore } from '@/stores'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IItem } from '@/Models/ItemModels'
import { ItemStatus, ItemType, Permission } from '@/Models/enum'

const route = useRoute()
const router = useRouter()
const dateHelper = useDate()
const { mdAndDown } = useDisplay()
const loading: Ref<boolean> = ref(false)
const view: Ref<'card' | 'grid'> = ref('card')
const project: Ref<IProject | undefined> = ref()
const keep: Ref<IKeep | undefined> = ref()
const { GetAllItems } = ItemStore()
const { Items } = storeToRefs(ItemStore())
const { User } = UserStore()
const itemToDisplay: Ref<IItem[]> = ref([])
const filters = reactive<{
    date?: Date | Date[],
    itemType?: ItemType[],
    itemStatus?: ItemStatus[],
    itemOwner?: string[]
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
            ...project.value.users.filter(x => x.isAccepted || !x.shareId).map(x => {
                return {
                    title: x.invitedUser.userName,
                    value: x.invitedUser.email
                }
            })
        )
    }
    if (keep.value) {
        _users.push(
            ...keep.value.users.filter(x => x.isAccepted).map(x => {
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
    if (!project.value || !keep.value) return false
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
watch([filters, Items], () => {
    itemToDisplay.value = Items.value.filter(itemFilterCallBack).sort((x, y) => x.status - y.status)
}, {
    deep: true
})
watch(mdAndDown, () => {
    view.value = mdAndDown.value ? 'card' : view.value
})
const isSameDate = (date1: Date, date2: Date | Date[]): boolean => {
    return Array.isArray(date2) ?
        date2.map(d => dateHelper.format(d, 'keyboardDate'))
            .includes(dateHelper.format(date1, 'keyboardDate')) :
        dateHelper.format(date1, 'keyboardDate') === dateHelper.format(date2, 'keyboardDate')
}
const itemFilterCallBack = (item: IItem): boolean => {
    return (
        !filters.date ||
        (Array.isArray(filters.date) && filters.date.length === 0) || isSameDate(new Date(item.createdOn), filters.date)
    ) &&
        (filters.itemType == undefined || filters.itemType.includes(item.type)) &&
        (filters.itemStatus == undefined || filters.itemStatus.includes(item.status)) &&
        (!filters.itemOwner || filters.itemOwner.includes(item.createdBy))

}
onMounted(async () => {
    loading.value = true
    await GetAllItems(keepId.value)
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    keep.value = await KeepStore().getSingleKeep(keepId.value)
    if (!hasAccess.value) router.go(-1)
    else loading.value = false
    itemToDisplay.value = Items.value.sort((x, y) => x.status - y.status)
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
            <v-col cols="auto" v-if="!mdAndDown">
                <v-btn-toggle v-model="view" mandatory color="primary" class="rounded-pill" density="compact">
                    <v-btn value="card" text="card" width="90">
                        <template v-slot:prepend>
                            <v-icon>mdi-card-text-outline</v-icon>
                        </template>
                    </v-btn>
                    <v-btn value="grid" text="grid" width="90">
                        <template v-slot:prepend>
                            <v-icon>mdi-table</v-icon>
                        </template>
                    </v-btn>
                </v-btn-toggle>
            </v-col>
            <v-col cols="auto">
                <date-picker v-model="filters.date"></date-picker>
            </v-col>
            <item-filter v-model:item-type="filters.itemType" v-model:item-status="filters.itemStatus" :users="users"
                v-model:item-owner="filters.itemOwner">
            </item-filter>
            <v-col>
                <add-item :keep="keep" :project="project" v-if="canCreate()"></add-item>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project && keep && view == 'card'">
            <template v-for="(item, index) of itemToDisplay" :key="index">
                <v-col cols="12" lg="4" md="6">
                    <item-card :item="item" :project="project" :keep="keep">
                    </item-card>
                </v-col>
            </template>
        </v-row>
        <v-row v-if="!loading && project && keep && view == 'grid' && itemToDisplay.length != 0" class="bg-white mt-5 mb-5">
            <v-col cols="12">
                <v-row class="border-b bg-primary">
                    <v-col cols="1">Task</v-col>
                    <v-col cols="1">Title</v-col>
                    <v-col>Description</v-col>
                    <v-col cols="1">Discussed With</v-col>
                    <v-col cols="1">Discussed By</v-col>
                    <v-col cols="1">Status</v-col>
                </v-row>
                <template v-for="(item, index) of itemToDisplay" :key="index">
                    <item-grid :item="item" :project="project" :keep="keep"></item-grid>
                </template>
            </v-col>
        </v-row>
        <v-row v-if="!loading && (!project || !keep || itemToDisplay.length == 0)" class="mt-10">
            <no-item>
                <template v-slot:title>
                    <span v-if="!project">No Project found with this id</span>
                    <span v-else-if="!keep">No Keep found with this id</span>
                    <span v-else>No Item found</span>
                </template>
                <template v-slot:subtitle v-if="!(!project || !keep)">
                    <span v-if="filters.date || filters.itemOwner || filters.itemStatus || filters.itemType">
                        No item found with specified filters
                    </span>
                    <span v-else>
                        Please click on add button to insert new record
                    </span>
                </template>
            </no-item>
        </v-row>
    </v-container>
</template>
