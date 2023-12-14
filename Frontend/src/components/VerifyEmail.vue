<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useRouter } from 'vue-router'
import TextField from '@/components/Custom/TextField.vue'
import { UserStore, AccountStore, GlobalStore } from '@/stores'
import { RouterEnum } from '@/Models/enum'


const { email, serverOtp } = storeToRefs(AccountStore())
const { errors, Loading } = storeToRefs(GlobalStore())
const form = ref()
const router = useRouter()
const { CheckEmail } = UserStore()
const { fetchOtp } = AccountStore()


const validateEmail = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const res = await CheckEmail(email.value)
    if (res) {
        serverOtp.value = await fetchOtp(email.value)
        router.push({ name: RouterEnum.VERIFY_OTP })
    }
}

onMounted(() => {
    errors.value = {}
    form.value.reset()
})
</script>
<template>
    <v-form ref="form" validate-on="submit" @submit.prevent="validateEmail">
        <div>
            <text-field v-model="email" label="Email" placeholder="Enter your Email" is-required is-email
                :error-messages="errors.email" />
            <div class="text-center">
                <v-btn color="primary" @click="validateEmail" :loading="Loading" :disabled="Loading">
                    Verify Email
                </v-btn>
            </div>
        </div>
    </v-form>
</template>