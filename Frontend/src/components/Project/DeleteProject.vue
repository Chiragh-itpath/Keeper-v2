<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { DeletePrompt } from '@/components/Custom'
import { ProjectStore } from '@/stores'

const { id } = defineProps<{
    id: string
}>()

const visible: Ref<boolean> = ref(false)
const { DeleteProject } = ProjectStore()
const deleteHandler = async (): Promise<void> => {
    await DeleteProject(id)
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
    <delete-prompt v-model="visible" @click:yes="deleteHandler" title="Delete Project">
        <template v-slot="{ props }">
            <slot :activator="props"></slot>
        </template>
    </delete-prompt>
</template>