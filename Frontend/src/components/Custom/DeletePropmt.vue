<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'

const props = withDefaults(defineProps<{
    modelValue: boolean
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(false)
watch(props,() => {
    visible.value = props.modelValue
})
watch(visible, () => {
    emits('update:modelValue', visible.value)
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void,
    (e: 'click:yes'): void
}>()

</script>
<template>
    <v-dialog v-model="visible" transition="scale-transition" max-width="400">
        <v-card>
            <v-card-title class="bg-primary" >Delete <slot></slot></v-card-title>
            <v-card-text class="my-2">
                Are you sure want to delete this?
            </v-card-text>
            <v-card-actions class="my-2 d-flex justify-end">
                <v-btn variant="outlined" color="success" class="rounded-xl mx-2" width="100" @click="visible = false">Cancel</v-btn>
                <v-btn variant="elevated" color="danger" class="rounded-xl mx-2" width="100" @click="emits('click:yes')">Yes</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>