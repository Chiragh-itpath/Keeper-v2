<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { StatusList, TypeList } from '@/components/Items'
import { ItemStatus, ItemType } from '@/Models/enum'
const props = defineProps<{
    users: { title: string, value: string }[],
    itemType?: ItemType,
    itemStatus?: ItemStatus,
    itemOwner?: string
}>()
const selectedType = ref([])
const selectedStatus = ref([])
const selectedUser = ref([])
const searchText: Ref<string | undefined> = ref()
const displayUser = ref(props.users)
const userMenu: Ref<boolean> = ref(false)
watch(searchText, () => {
    displayUser.value = props.users.filter(x => {
        return (
            !searchText.value ||
            x.title.toLowerCase().startsWith(searchText.value.toLowerCase()) ||
            x.value.toLowerCase().startsWith(searchText.value.toLowerCase())
        )
    })
})
const emits = defineEmits<{
    (e: 'update:itemType', type: ItemType | undefined): void,
    (e: 'update:itemStatus', type: ItemStatus | undefined): void,
    (e: 'update:itemOwner', owner: string | undefined): void,
}>()
</script>
<template>
    <v-col cols="auto" class="px-2">
        <v-menu :transition="false" width="150">
            <template v-slot:activator="{ props, isActive }">
                <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary"
                    :append-icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'" v-if="selectedType.length == 0">
                    Type
                </v-btn>
                <v-btn class="rounded-lg" variant="flat" color="primary" v-else>
                    <template v-slot:append>
                        <v-icon class="ms-end" icon="mdi-close"
                            @click="selectedType = []; emits('update:itemType', undefined)">
                        </v-icon>
                    </template>
                    {{ TypeList[selectedType[0]].title }}
                </v-btn>
            </template>
            <v-list density="compact" v-model:selected="selectedType" select-strategy="single-independent" :items="TypeList"
                @update:selected="() => emits('update:itemType', selectedType[0])">
            </v-list>
        </v-menu>
    </v-col>
    <v-col cols="auto" class="px-2">
        <v-menu :transition="false">
            <template v-slot:activator="{ props, isActive }">
                <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary"
                    :append-icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'" v-if="selectedStatus.length == 0">
                    Status
                </v-btn>
                <v-btn class="rounded-lg" variant="flat" color="primary" v-else>
                    <template v-slot:append>
                        <v-icon class="ms-end" icon="mdi-close"
                            @click="selectedStatus = []; emits('update:itemStatus', undefined)">
                        </v-icon>
                    </template>
                    {{ StatusList[selectedStatus[0]].title }}
                </v-btn>
            </template>
            <v-list density="compact" v-model:selected="selectedStatus"
                @update:selected="emits('update:itemStatus', selectedStatus[0])">
                <template v-for="(item, index) in StatusList" :key="index">
                    <v-list-item :title="item.title" :value="item.value">
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
                    :append-icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'" v-if="selectedUser.length == 0">
                    Owner
                </v-btn>
                <v-btn class="rounded-lg text-lowercase" variant="flat" color="primary" v-else>
                    {{ selectedUser[0] }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" @click="selectedUser = []; emits('update:itemOwner', undefined)"></v-icon>
                    </template>
                </v-btn>
            </template>
            <v-sheet>
                <v-text-field hide-details density="comfortable" color="primary" placeholder="search for user"
                    prepend-inner-icon="mdi-magnify" v-model="searchText">
                </v-text-field>
            </v-sheet>
            <v-list max-height="200" v-model:selected="selectedUser"
                @update:selected="userMenu = false; emits('update:itemOwner', selectedUser[0])">
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
</template>