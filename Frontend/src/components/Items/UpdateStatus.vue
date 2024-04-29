<script setup lang="ts">
import { ref, watch, type Ref, computed } from 'vue'
import { GlobalStore, ItemStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IStatus } from '@/Models/ProjectSettings'
import type { IItem } from '@/Models/ItemModels'

const props = defineProps<{
    item: IItem
    statusList: IStatus[]
}>()

const menu: Ref<boolean> = ref(false)
const isUpdating = ref(false)
const selected: Ref<string[]> = ref([props.item.statusId])
const items = computed(() => {
    return props.statusList.map(x => {
        return {
            title: x.title,
            value: x.id
        }
    })
})
const itemStore = ItemStore()
const { Loading } = storeToRefs(GlobalStore())

watch(props, () => {
    selected.value = [props.item.statusId]
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
        <v-list max-height="300" density="compact" select-strategy="single-independent" :selected="selected" mandatory
            @update:selected="UpdateStatusHandler">

            <template v-for="(item, index) in items" :key="index">
                <v-list-item :title="item.title" :value="item.value" color="primary">
                </v-list-item>
            </template>
        </v-list>
    </v-menu>
</template>