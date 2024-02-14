<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { StatusList } from '@/components/Items'
import type { IItem } from '@/Models/ItemModels'
import { ItemStatus } from '@/Models/enum'
import { GlobalStore, ItemStore } from '@/stores'
import { storeToRefs } from 'pinia'

const props = defineProps<{
    item: IItem,
    status: ItemStatus
}>()
const selected = ref([props.status])
const isUpdating = ref(false)
const menu: Ref<boolean> = ref(false)
const itemStore = ItemStore()
const { Loading } = storeToRefs(GlobalStore())

watch(props, () => {
    selected.value = [props.status]
})
const UpdateStatusHandler = async (status: any) => {
    if (isUpdating.value) return
    menu.value = false
    isUpdating.value = true
    await itemStore.updateStatus(props.item.id, status[0])
    isUpdating.value = false
} 
</script>
<template>
    <v-menu location="right" :close-on-content-click="false" v-model="menu" :transition="false" :disabled="Loading">
        <template v-slot:activator="{ props, isActive }">
            <slot :props="props" :is-active="isActive" v-if="!isUpdating"></slot>
            <v-progress-circular indeterminate v-if="isUpdating" color="primary"></v-progress-circular>
        </template>
        <v-list density="compact" select-strategy="single-independent" :selected="selected" mandatory
            @update:selected="UpdateStatusHandler">
            <template v-for="(item, index) in StatusList" :key="index">
                <v-list-item :title="item.title" :value="item.value" color="primary"></v-list-item>
            </template>
        </v-list>
    </v-menu>
</template>