<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'

const props = withDefaults(defineProps<{
    modelValue?: boolean,
    text?: string,
    description?: string,
    width?: string
}>(), {
    modelValue: false,
    text: 'Confirm cancel',
    description: 'There are some unsaved changes. Are you sure you want to leave?',
})

const visible: Ref<boolean> = ref(false)
watch(props, () => {
    visible.value = props.modelValue
})
watch(visible, () => {
    emits('update:modelValue', visible.value)
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void,
    (e: 'yes'): void,
    (e: 'cancel'): void
}>()
</script>

<template>
    <v-dialog v-model="visible" :max-width="width ?? 500">
        <template v-slot:activator="{ props }">
            <slot v-bind="{ props }"></slot>
        </template>
        <v-card>
            <v-card-title class="bg-primary">
                {{ text }}
                <v-icon class="float-end" @click="emits('cancel'); visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="my-2">
                <slot name="alert">
                    {{ description }}
                </slot>
            </v-card-text>
            <v-card-actions class="my-2 d-flex justify-end">
                <v-btn text="Cancel" variant="outlined" color="success" class="rounded-xl mx-2" width="100"
                    @click="emits('cancel'); visible = false" />
                <v-btn text="Yes" variant="elevated" color="danger" class="rounded-xl mx-2" width="100"
                    @click="emits('yes'); visible = false" />
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>