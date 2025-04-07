<script setup lang="ts">
import { computed, onMounted, ref, watch, type Ref } from 'vue'
import { TypeList } from '@/components/Items'
import { DatePicker } from '@/components/Custom'
import { ItemType } from '@/Models/enum'
import type { IStatus } from '@/Models/ProjectSettings'

const props = defineProps<{
    projectId?: string
    users: { title: string; value: string }[]
    itemType?: ItemType[]
    itemStatus?: string[]
    itemOwner?: string[],
    statusList: IStatus[],
    date?: Date | Date[]
}>()

const selectedType = ref<ItemType[]>([])
const selectedStatus = ref<string[]>([])
const selectedUser = ref<string[]>([])
const statusList = ref(props.statusList)
const searchText: Ref<string | undefined> = ref()
const displayUser = ref(props.users)
const userMenu: Ref<boolean> = ref(false)
const dateRef = ref(props.date)

watch(searchText, () => {
    displayUser.value = props.users.filter((x) => {
        return (
            !searchText.value ||
            x.title.toLowerCase().startsWith(searchText.value.toLowerCase()) ||
            x.value.toLowerCase().startsWith(searchText.value.toLowerCase())
        )
    })
})
watch([() => props.itemType, () => props.itemStatus, () => props.itemOwner], () => {
    selectedType.value = props.itemType ?? []
    selectedStatus.value = props.itemStatus ?? []
    selectedUser.value = props.itemOwner ?? []
})
const emits = defineEmits<{
    (e: 'update:itemType', type?: ItemType[]): void
    (e: 'update:itemStatus', type?: string[]): void
    (e: 'update:itemOwner', owner?: string[]): void
    (e: 'update:date', date?: Date | Date[] | undefined): void
}>()

const hasFilters = computed(() => {
    return (
        selectedType.value.length > 0 ||
        selectedStatus.value.length > 0 ||
        selectedUser.value.length > 0 ||
        props.date
    )
})

const clearFilters = () => {
    selectedType.value = []
    selectedStatus.value = []
    selectedUser.value = []
    dateRef.value = undefined
    emits('update:itemType', undefined)
    emits('update:itemStatus', undefined)
    emits('update:itemOwner', undefined)
    emits('update:date', undefined)
}

const updateDate = (date: Date | Date[] | undefined) => {
    dateRef.value = date
    if (date) {
        if (Array.isArray(date) && date.length == 0) {
            emits('update:date', undefined)
            return
        }
        emits('update:date', date)
    } else {
        emits('update:date', undefined)
    }
}

onMounted(() => {
    selectedType.value = props.itemType ?? []
    selectedStatus.value = props.itemStatus ?? []
    selectedUser.value = props.itemOwner ?? []
    console.log(displayUser)
})
</script>

<template>
    <v-col cols="auto">
        <date-picker :modelValue="dateRef" @update:modelValue="updateDate"></date-picker>
    </v-col>
    <v-col cols="auto">
        <v-menu :transition="false" width="150" :close-on-content-click="false">
            <template v-slot:activator="{ props: menu, isActive }">
                <v-btn v-bind="menu" class="rounded-lg" variant="outlined" color="primary">
                    {{
                        selectedType.length == 0
                            ? 'Type'
                            : selectedType.length == 1
                                ? `${TypeList[selectedType[0]].title}`
                                : `Selected (${selectedType.length})`
                    }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" v-if="selectedType.length != 0"
                            @click.stop="selectedType = []; emits('update:itemType')" />
                        <v-icon>{{ isActive ? 'mdi-menu-up' : 'mdi-menu-down' }}</v-icon>
                    </template>
                </v-btn>
            </template>
            <v-list color="primary" density="compact" v-model:selected="selectedType" select-strategy="classic"
                :items="TypeList"
                @update:selected="() => emits('update:itemType', selectedType.length == 0 ? undefined : selectedType)">

                <template v-slot:item="{ props }">
                    <v-list-item v-bind="props"> </v-list-item>
                </template>
            </v-list>
        </v-menu>
    </v-col>
    <v-col cols="auto" class="px-2">
        <v-menu :transition="false" :close-on-content-click="false">

            <template v-slot:activator="{ props, isActive }">
                <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary">
                    {{selectedStatus.length == 0 ? 'Status' : selectedStatus.length == 1
                        ? `${statusList.find(x => x.id == selectedStatus[0])?.title}`
                        : `Selected (${selectedStatus.length})` }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" v-if="selectedStatus.length != 0"
                            @click.stop="selectedStatus = []; emits('update:itemStatus')" />
                        <v-icon>{{ isActive ? 'mdi-menu-up' : 'mdi-menu-down' }}</v-icon>
                    </template>
                </v-btn>
            </template>
            <v-list max-height="300" density="compact" v-model:selected="selectedStatus" select-strategy="classic"
                color="primary"
                @update:selected="emits('update:itemStatus', selectedStatus.length == 0 ? undefined : selectedStatus)">

                <template v-for="(status, index) in statusList" :key="index">
                    <v-list-item :title="status.title" :value="status.id">
                    </v-list-item>
                </template>
            </v-list>
        </v-menu>
    </v-col>
    <v-col cols="auto" class="px-2">
        <v-menu width="300" :transition="false" :close-on-content-click="false" v-model="userMenu"
            @update:model-value="searchText = undefined">

            <template v-slot:activator="{ props, isActive }">
                <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary"
                    :class="[{ 'text-lowercase': selectedUser.length }]">
                    {{selectedUser.length == 0 ? 'Owner' : selectedUser.length == 1 ?
                        `${displayUser.find((x) => x.value == selectedUser[0])?.title}` :
                        `Selected (${selectedUser.length})` }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" v-if="selectedUser.length != 0"
                            @click.stop="selectedUser = []; emits('update:itemOwner')" />
                        <v-icon>{{ isActive ? 'mdi-menu-up' : 'mdi-menu-down' }}</v-icon>
                    </template>
                </v-btn>
            </template>
            <v-sheet>
                <v-text-field hide-details density="comfortable" color="primary" placeholder="search for user"
                    prepend-inner-icon="mdi-magnify" v-model="searchText">
                </v-text-field>
            </v-sheet>
            <v-list max-height="200" v-model:selected="selectedUser" select-strategy="classic" color="primary"
                @update:selected="emits('update:itemOwner', selectedUser.length == 0 ? undefined : selectedUser)">

                <template v-for="(user, index) in displayUser" :key="index">
                    <v-list-item :title="user.title" :subtitle="user.value" :value="user.value">
                        <template v-slot:prepend>
                            <v-avatar color="primary" density="compact">
                                {{ user.value.slice(0, 1).toUpperCase() }}
                            </v-avatar>
                        </template>
                    </v-list-item>
                </template>
                <v-list-item v-if="displayUser.length == 0" title="No user found" class="text-grey"></v-list-item>
            </v-list>
        </v-menu>
    </v-col>
    <v-col cols="auto" class="px-2" v-if="hasFilters">
        <v-btn class="rounded-lg" variant="tonal" color="primary" @click="clearFilters">
            Clear Filters
        </v-btn>
    </v-col>
</template>
