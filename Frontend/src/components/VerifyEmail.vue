<script setup lang="ts">
import { onMounted, ref, watch, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import TextField from '@/components/Custom/TextField.vue'
import { UserStore, GlobalStore } from '@/stores'


defineProps<{
    modelValue: string
}>()
const { errors, Loading } = storeToRefs(GlobalStore())
const form = ref()

const { CheckEmail } = UserStore()
const email: Ref<string> = ref('')

const validateEmail = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const res = await CheckEmail(email.value)
    if (res) {
        emit('valid')
    }
}

const emit = defineEmits<{
    (e: 'valid'): void,
    (e: 'update:modelValue', value: string): void
}>()
watch(email, () => {
    if (email.value) {
        emit('update:modelValue', email.value.trim())
    }
})
onMounted(() => {
    errors.value = {}
})
</script>
<template>
    <v-form ref="form" validate-on="submit" @submit.prevent="validateEmail">
        <div>
            <text-field v-model="email" label="Email" placeholder="Enter your Email" is-required is-email
                :error-messages="errors.email" />
            <div class="text-center mt-10">
                <v-btn color="primary" @click="validateEmail" :loading="Loading" :disabled="Loading">
                    Verify Email
                </v-btn>
            </div>
        </div>
    </v-form>
</template>