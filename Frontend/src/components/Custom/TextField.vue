<script lang="ts" setup>
import {
    contactRules,
    emailRules,
    numberRules,
    passwordRule,
    requiredRule,
    urlRules
} from '@/data/ValidationRules'
import { ref, type Ref } from 'vue'

type TRule = (arg: string) => boolean | string

const Props = withDefaults(
    defineProps<{
        label?: string
        color?: string
        placeholder?: string
        isRequired?: boolean
        isEmail?: boolean
        isContact?: boolean
        isPassword?: boolean
        isNumber?: boolean
        isUrl?: boolean
        icon?: string
        ValidationRules?: TRule[],
        errorMessages?: string
    }>(),
    {
        label: '',
        color: 'primary',
        placeholder: '',
        isRequired: false,
        isEmail: false,
        isContact: false,
        isPassword: false,
        isNumber: false,
        isUrl: false,
        icon: '',
    }
)
const error: Ref<boolean> = ref(false)
const type: Ref<string> = ref(Props.isEmail ? 'email' : Props.isPassword ? 'password' : 'text')
let Rules: TRule[] = []

let isPasswordVisible: Ref<boolean> = ref(false)

if (Props.isRequired) Rules.push(requiredRule)

if (Props.isContact) Rules.push(contactRules)

if (Props.isEmail) Rules.push(emailRules)

if (Props.isPassword) Rules.push(passwordRule)

if (Props.isNumber) Rules.push(numberRules)

if (Props.isUrl) Rules.push(urlRules)

Rules.push(...(Props.ValidationRules || []))

const changeVisibility = (): void => {
    isPasswordVisible.value = !isPasswordVisible.value
    type.value = isPasswordVisible.value ? 'text' : 'password'
}
if (Props.errorMessages) {
    if (Props.errorMessages.length > 0) {
        error.value = true
    }
}
</script>
<template>
    <v-text-field :label="Props.label" :type="type" :color="Props.color" hide-details="auto" :rules="Rules"
        :append-inner-icon="!isPassword ? '' : isPasswordVisible ? 'mdi-eye' : 'mdi-eye-off'" :error="error"
        :placeholder="placeholder" :error-messages="errorMessages" @click:append-inner="changeVisibility" class="mb-4">
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