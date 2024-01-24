<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { DeletePropmt } from '@/components/Custom/'
import { ItemStore } from '@/stores'
const { id } = defineProps<{
    id: string
}>()
const visible: Ref<boolean> = ref(false)
const { DeleteItem } = ItemStore()
const deleteHandler = async (): Promise<void> => {
    await DeleteItem(id)
    visible.value = false
}
watch(visible, () => {
    if (!visible.value) emits('close')
})
const emits = defineEmits<{
    (e: 'close'): void
}>()
</script>
<template>
    <delete-propmt v-model="visible" @click:yes="deleteHandler" title="Delete Item">
        <template v-slot="{ props }">
            <slot :activator="props"></slot>
        </template>
    </delete-propmt>
</template>