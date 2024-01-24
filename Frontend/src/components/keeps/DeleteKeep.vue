<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { KeepStore } from '@/stores'
import { DeletePropmt } from '@/components/Custom'

const props = defineProps<{
    id: string
}>()
const visible: Ref<boolean> = ref(false)
const { DeleteKeep } = KeepStore()

const deleteHandler = async (): Promise<void> => {
    await DeleteKeep(props.id)
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
    <delete-propmt v-model="visible" @click:yes="deleteHandler" title="Delete Keep">
        <template v-slot="{ props }">
            <slot :activator="props"></slot>
        </template>
    </delete-propmt>
</template>