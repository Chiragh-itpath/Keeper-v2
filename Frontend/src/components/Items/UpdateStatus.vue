<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { StatusList } from './'
import type { IItem } from '@/Models/ItemModels'
import { ItemStatus } from '@/Models/enum'
import { GlobalStore, ItemStore } from '@/stores'
import { storeToRefs } from 'pinia'

const props = defineProps<{
    item: IItem
}>()
const selected = ref([props.item.status])
const isUpdating = ref(false)
const menu: Ref<boolean> = ref(false)
const itemStore = ItemStore()
const { Loading } = storeToRefs(GlobalStore())
const emit = defineEmits<{
    (e: 'update:status', status: ItemStatus): void
}>()
watch(selected, async (newValue, oldValue) => {
    if (isUpdating.value) return
    if (oldValue[0] == newValue[0]) return
    menu.value = false
    isUpdating.value = true
    if (await itemStore.updateStatus(props.item.id, newValue[0])) {
        emit('update:status', newValue[0])
    } else {
        selected.value = [props.item.status]
    }
    isUpdating.value = false
})
</script>
<template>
    <v-menu location="right" :close-on-content-click="false" v-model="menu" :transition="false" :disabled="Loading">
        <template v-slot:activator="{ props, isActive }">
            <slot :props="props" :is-active="isActive" v-if="!isUpdating"></slot>
            <v-progress-circular indeterminate v-if="isUpdating" color="primary"></v-progress-circular>
        </template>
        <v-list density="compact" select-strategy="single-independent" v-model:selected="selected" mandatory>
            <template v-for="(item, index) in StatusList" :key="index">
                <v-list-item :title="item.title" :value="item.value" color="primary" v-if="index != 0"></v-list-item>
            </template>
        </v-list>
    </v-menu>
</template>