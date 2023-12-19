<script setup lang="ts">
import { ref, watch } from 'vue'


const props = withDefaults(defineProps<{
    modelValue: string,
    label?: string,
    class?: string
    length?: number,
    variant?: 'outlined' | 'plain' | 'underlined' | 'filled' | 'solo' | 'solo-inverted' | 'solo-filled',
    error?: string
}>(), {
    class: ''
})

const otp = ref(props.modelValue)

watch(otp, () => {
    emits('update:modelValue', otp.value)
    placeholder.value = otp.value.length > 0 ? '' : '0'
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: string): void
}>()
defineOptions({
    inheritAttrs: false
})
const length = props.length ?? 6
const variant = props.variant ?? 'filled'
const placeholder = ref('0')

</script>
<template>
    <v-otp-input v-model="otp" :length="length" label="hello" class="text-primary" :class="''" base-color="primary"
        :variant="variant" :error="error != undefined && error != ''" :placeholder="placeholder">
    </v-otp-input>
    <div class="text-danger text-center mb-2">{{ error }}</div>
</template>