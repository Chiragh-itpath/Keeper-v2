<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { StatusList, TypeList } from '@/components/Items'
import { ItemStatus, ItemType } from '@/Models/enum'

const props = defineProps<{
    users: { title: string, value: string }[],
    itemType?: ItemType[],
    itemStatus?: ItemStatus[],
    itemOwner?: string[]
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
    (e: 'update:itemType', type?: ItemType[]): void,
    (e: 'update:itemStatus', type?: ItemStatus[]): void,
    (e: 'update:itemOwner', owner?: string[]): void,
}>()
</script>
<template>
    <v-col cols="auto" class="px-2">
        <v-menu :transition="false" width="150" :close-on-content-click="false">
            <template v-slot:activator="{ props: menu, isActive }">
                <v-btn v-bind="menu" class="rounded-lg" variant="outlined" color="primary">
                    {{ (selectedType.length == 0) ?
                        'Type' :
                        (selectedType.length == 1) ?
                            `${TypeList[selectedType[0]].title}` :
                            `Selected (${selectedType.length})`
                    }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" v-if="selectedType.length != 0"
                            @click.stop="selectedType = []; emits('update:itemType')" />
                        <v-icon>{{ isActive ? 'mdi-menu-up' : 'mdi-menu-down' }}</v-icon>
                    </template>
                </v-btn>
            </template>
            <v-list density="compact" v-model:selected="selectedType" select-strategy="classic" :items="TypeList"
                color="primary"
                @update:selected="() => emits('update:itemType', selectedType.length == 0 ? undefined : selectedType)">
                <template v-slot:item="{ props }">
                    <v-list-item v-bind="props">
                    </v-list-item>
                </template>
            </v-list>
        </v-menu>
    </v-col>
    <v-col cols="auto" class="px-2">
        <v-menu :transition="false" :close-on-content-click="false">
            <template v-slot:activator="{ props, isActive }">
                <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary">
                    {{
                        (selectedStatus.length == 0) ?
                        'Status' :
                        (selectedStatus.length == 1) ?
                            `${StatusList[selectedStatus[0]].title}` :
                            `Selected (${selectedStatus.length})`
                    }}
                    <template v-slot:append>
                        <v-icon icon="mdi-close" v-if="selectedStatus.length != 0"
                            @click.stop="selectedStatus = []; emits('update:itemStatus')" />
                        <v-icon>{{ isActive ? 'mdi-menu-up' : 'mdi-menu-down' }}</v-icon>
                    </template>
                </v-btn>
            </template>
            <v-list density="compact" v-model:selected="selectedStatus" select-strategy="classic" color="primary"
                @update:selected="emits('update:itemStatus', selectedStatus.length == 0 ? undefined : selectedStatus)">
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
                    :class="[{ 'text-lowercase': selectedUser.length }]">
                    {{
                        (selectedUser.length == 0) ?
                        'Owner' :
                        (selectedUser.length == 1) ?
                            `${displayUser.find(x => x.value == selectedUser[0])?.title}` :
                            `Selected (${selectedUser.length})`
                    }}
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
</template>