<script lang="ts" setup>
import {
    contactRules,
    emailRules,
    passwordRule,
    requiredRule,
    urlRules
} from '@/data/ValidationRules'
import { ref, watch, type Ref } from 'vue'

type TRule = (arg: string) => boolean | string

const Props = withDefaults(
    defineProps<{
        modelValue?: string
        label?: string
        color?: string
        placeholder?: string
        counter?: boolean
        isRequired?: boolean
        isEmail?: boolean
        isContact?: boolean
        isPassword?: boolean
        isNumber?: boolean
        isUrl?: boolean
        icon?: string
        maxLimit?: number
        ValidationRules?: TRule[]
        errorMessages?: string
        density?: 'default' | 'compact' | 'comfortable'
    }>(),
    {
        label: '',
        color: 'primary',
        placeholder: '',
        counter: false,
        isRequired: false,
        isEmail: false,
        isContact: false,
        isPassword: false,
        isNumber: false,
        isUrl: false,
        icon: '',
        maxLimit: 30,
        density: 'comfortable'
    }
)
const _value: Ref<string> = ref(Props.modelValue ?? '')
const error: Ref<boolean> = ref(Props.errorMessages ? true : false)

const type: Ref<string> = ref(Props.isEmail ? 'email' : Props.isPassword ? 'password' : 'text')
let Rules: TRule[] = []

let isPasswordVisible: Ref<boolean> = ref(false)

if (Props.isRequired) Rules.push(requiredRule)

if (Props.isContact) Rules.push(contactRules)

if (Props.isEmail) Rules.push(emailRules)

if (Props.isPassword) Rules.push(passwordRule)

if (Props.isUrl) Rules.push(urlRules)

Rules.push(...(Props.ValidationRules || []))

const changeVisibility = (): void => {
    isPasswordVisible.value = !isPasswordVisible.value
    type.value = isPasswordVisible.value ? 'text' : 'password'
}
watch(Props, () => {
    _value.value = Props.modelValue ?? '';
    error.value = Props.errorMessages ? true : false
})
watch(_value, () => {
    emits('update:modelValue', _value.value?.trim() ?? '')
})
defineOptions({
    inheritAttrs: false
})
const emits = defineEmits<{
    (e: 'update:modelValue', value: string): void
}>()
</script>

<template>
    <v-text-field v-model="_value" :label="Props.label" :type="type" :counter="counter" :color="Props.color"
        hide-details="auto" :rules="Rules" :density="density" :placeholder="placeholder" :error-messages="errorMessages"
        :append-inner-icon="!isPassword ? '' : isPasswordVisible ? 'mdi-eye' : 'mdi-eye-off'" :error="error"
        @click:append-inner="changeVisibility" class="mb-4" @input="() => {
        _value = _value.slice(0, Props.maxLimit)
        if (Props.isContact || Props.isNumber) {
            _value = _value.replace(/[^\d]/g, '')
        }
    }">
        <template v-slot:prepend-inner="{ isFocused }">
            <v-icon v-if="Props.icon" :icon="Props.icon"
                :class="isFocused.value == true ? 'text-primary' : 'text-grey'"></v-icon>
        </template>
    </v-text-field>
</template>

<style>
.v-icon {
    --v-medium-emphasis-opacity: 1 !important;
}
</style>