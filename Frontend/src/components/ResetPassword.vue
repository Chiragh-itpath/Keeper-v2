<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { AccountStore, GlobalStore } from '@/stores'
import TextField from '@/components/Custom/TextField.vue'
import { RouterEnum } from '@/Models/enum'

const form = ref()
const props = defineProps<{
    email: string
}>()
const errorMessages: Ref<string> = ref('')
const Password: Ref<string> = ref('')
const confirmPassword: Ref<string> = ref('')
const router = useRouter()

const { PasswordReset } = AccountStore()
const { Loading } = storeToRefs(GlobalStore())
const ChangePassWord = async () => {
    errorMessages.value = ''
    const { valid } = await form.value.validate()
    if (!valid) return
    if (Password.value != confirmPassword.value) {
        errorMessages.value = 'Password and confirm Password must be same'
        return
    }
    await PasswordReset({
        email: props.email,
        password: Password.value
    })
    router.push({ name: RouterEnum.LOGIN })
}

</script>
<template>
    <v-form ref="form" validate-on="submit">
        <text-field label="Password" v-model="Password" placeholder="Enter new Password" is-required is-password />
        <text-field label="Confirm Password" v-model="confirmPassword" is-required is-password
            placeholder="Enter confirm Password" :error-messages="errorMessages" />
        <div class="d-flex justify-center pa-4">
            <v-btn color="primary" @click="ChangePassWord" :loading="Loading" :disabled="Loading">Change Password</v-btn>
        </div>
    </v-form>
</template>