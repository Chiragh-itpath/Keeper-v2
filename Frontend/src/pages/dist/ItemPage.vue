<script setup lang="ts">
import { computed, ref, onMounted, type Ref, reactive, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useDate, useDisplay } from 'vuetify'
import { useRoute, useRouter } from 'vue-router'
import { NoItem } from '@/components/Custom'
import { AddItem, ItemFilter, ItemCard, ItemGrid } from '@/components/Items'
import { ItemStore, ProjectStore, KeepStore, UserStore } from '@/stores'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IItem } from '@/Models/ItemModels'
import { ItemType, Permission } from '@/Models/enum'
import { ProjectSettingsService, type IClient, type IStatus } from "@/Services/ProjectSettings"

const loading: Ref<boolean> = ref(false)
const view: Ref<'card' | 'grid'> = ref('card')
const projectSettings = new ProjectSettingsService()
const { User } = UserStore()
const { Items } = storeToRefs(ItemStore())
const project: Ref<IProject | undefined> = ref()
const keep: Ref<IKeep | undefined> = ref()
const StatusList: Ref<IStatus[]> = ref([])
const ClientList: Ref<IClient[]> = ref([])
const dateHelper = useDate()
const filters = reactive<{
    date?: Date | Date[],
    itemType?: ItemType[],
    itemStatus?: string[],
    itemOwner?: string[]
}>({})

const itemToDisplay = computed(() => {
    const filtered = Items.value.filter(itemFilterCallBack).sort((x, y) => y.status - x.status);

    const query: Record<string, string> = {};

    if (filters.date && (!Array.isArray(filters.date) || filters.date.length > 0)) {
        query.date = Array.isArray(filters.date)
            ? filters.date.map(d => dateHelper.format(d, 'keyboardDate')).join(',')
            : dateHelper.format(filters.date, 'keyboardDate');
    }

    if (filters.itemType?.length) {
        query.itemType = filters.itemType.join(',');
    }

    if (filters.itemStatus?.length) {
        query.itemStatus = filters.itemStatus.join(',');
    }

    if (filters.itemOwner?.length) {
        query.itemOwner = filters.itemOwner.join(',');
    }

    router.replace({ query });

    return filtered;
});

const { mdAndDown } = useDisplay()
watch(mdAndDown, () => {
    view.value = mdAndDown.value ? 'card' : view.value
})
const route = useRoute()
const router = useRouter()
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const keepId = computed(() => {
    const id = route.params.keepId
    return Array.isArray(id) ? id.join('') : id
})
const { GetAllItems }: any = ItemStore()
const breadcrumbs = [
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
        title: 'Items',
        disabled: true
    }
]
onMounted(async () => {
    const query = { ...route.query };
    loading.value = true
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    keep.value = await KeepStore().getSingleKeep(keepId.value)
    await KeepStore().GetKeeps(projectId.value)
    if (!hasAccess.value) router.go(-1)
    await GetAllItems(keepId.value)
    StatusList.value = await projectSettings.GetAllStatus(projectId.value) ?? []
    ClientList.value = await projectSettings.GetAllClient(projectId.value) ?? []
    // const deletedClients = Items.value
    //     .map(x => x.to)
    //     .filter(x => !!x)
    //     .filter(x => !ClientList.value.map(y => y.name).includes(x!))
    //     .map((x): IClient => {
    //         return {
    //             id: '',
    //             name: x!,
    //             projectId: ''
    //         }
    //     })
    // ClientList.value.push(...deletedClients)
    loading.value = false
    breadcrumbs[0].title = project.value?.title ?? 'Projects'
    breadcrumbs[1].title = keep.value?.title ?? 'Keeps'
    if (query.date) {
        filters.date = query.date.includes(',')
            ? query.date.toString().split(',').map(d => new Date(d))
            : new Date(query.date.toString());
    }
    if (query.itemType) {
        filters.itemType = query.itemType.toString().split(',').map(type => parseInt(type));
    }
    if (query.itemStatus) {
        filters.itemStatus = query.itemStatus.toString().split(',').map(status => status);
    }
    if (query.itemOwner) {
        filters.itemOwner = query.itemOwner.toString().split(',');
    }
})
const hasAccess = computed((): boolean => {
    return (
        (project.value?.createdBy == User.email) ||
        (project.value?.users.some(u => u.invitedUser.id == User.id && u.isAccepted) ?? false) ||
        (keep.value?.users.some(u => u.invitedUser.id == User.id && u.isAccepted) ?? false)
    )
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
        (filters.itemStatus == undefined || filters.itemStatus.includes(item.statusId)) &&
        (!filters.itemOwner || filters.itemOwner.includes(item.createdBy))

}
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
const users = computed(() => {
    const _users: { title: string, subtitle: string, value: string }[] = []
    if (project.value) {
        _users.push(
            ...project.value.users.filter(x => x.isAccepted || !x.shareId).map(x => {
                return {
                    title: x.invitedUser.userName,
                    subtitle: x.invitedUser.email,
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
                    subtitle: x.invitedUser.email,
                    value: x.invitedUser.email
                }
            })
        )
    }
    return _users
})
const mapToClient = (client: IClient) => {
    return {
        title: client.name,
        value: client.name
    }
}
</script>

<template>
    <v-container class="px-10" fluid>
        <v-row v-if="loading" class="mt-10">
            <v-col v-for=" i  in  4" :key="i" cols="12" md="6">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <template v-if="!loading && project && keep">
            <v-row>
                <v-col cols="12">
                    <v-breadcrumbs divider="/" :items="breadcrumbs" class="px-0">
                    </v-breadcrumbs>
                </v-col>
            </v-row>
            <v-row class="align-center flex-wrap">
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
                <item-filter v-model:item-type="filters.itemType" v-model:item-status="filters.itemStatus"
                    :users="users" v-model:item-owner="filters.itemOwner" :status-list="StatusList" v-model:date="filters.date">
                </item-filter>
                <v-col>
                    <add-item v-if="canCreate()" :keep="keep" :project="project" :users="users"
                        :status-list="StatusList" :client-list="ClientList.map(mapToClient)">
                    </add-item>
                </v-col>
            </v-row>
            <v-row v-if="view == 'card' && itemToDisplay.length != 0">
                <template v-for="(item, index) of itemToDisplay" :key="index">
                    <v-col cols="12" lg="4" md="6">
                        <item-card :item="item" :project="project" :keep="keep" :status-list="StatusList"
                            :client-list="ClientList.map(mapToClient)">
                        </item-card>
                    </v-col>
                </template>
            </v-row>
            <v-row v-if="view == 'grid' && itemToDisplay.length != 0" class="bg-white mt-5 mb-5">
                <v-col cols="12">
                    <v-row class="border-b bg-primary">
                        <v-col cols="2">Title</v-col>
                        <v-col>Description</v-col>
                        <v-col cols="1">Discussed With</v-col>
                        <v-col cols="1">Discussed By</v-col>
                        <v-col cols="2">Status</v-col>
                    </v-row>

                    <template v-for="(item, index) of itemToDisplay" :key="index">
                        <item-grid :item="item" :project="project" :keep="keep"
                            :client-list="ClientList.map(mapToClient)" :status-list="StatusList">
                        </item-grid>
                    </template>
                </v-col>
            </v-row>
            <v-row v-if="itemToDisplay.length == 0" class="mt-10">
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
        </template>
    </v-container>
</template>
