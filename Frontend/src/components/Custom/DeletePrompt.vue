<script setup lang="ts">
import { GlobalStore } from '@/stores'
import { storeToRefs } from 'pinia'
import { ref, watch, type Ref } from 'vue'

const props = withDefaults(defineProps<{
    modelValue?: boolean,
    title?: string,
    subtitle?: string,
    width?: string
}>(), {
    modelValue: false,
    subtitle: 'Are you sure want to delete this?'
})

const visible: Ref<boolean> = ref(false)
const { Loading } = storeToRefs(GlobalStore())
watch(props, () => {
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
    <v-dialog v-model="visible" :max-width="width ?? 400">
        <template v-slot:activator="{ props }">
            <slot v-bind="{ props }"></slot>
        </template>
        <v-card>
            <v-card-title class="bg-primary">
                {{ title }}
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="my-2">
                <slot name="alert">
                    {{ subtitle }}
                </slot>
            </v-card-text>
            <v-card-actions class="my-2 d-flex justify-end">
                <v-btn text="Cancel" variant="outlined" color="success" class="rounded-xl mx-2" width="100"
                    @click="visible = false" />
                <v-btn text="yes" variant="elevated" color="danger" class="rounded-xl mx-2" width="100"
                    @click="emits('click:yes'); visible = !visible" :loading="Loading" />
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>