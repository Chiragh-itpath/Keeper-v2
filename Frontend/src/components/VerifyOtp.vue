<script setup lang="ts">
import { ref, type Ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import OtpInput from '@/components/Custom/OtpInput.vue'
import { AccountStore, GlobalStore } from '@/stores'
import { storeToRefs } from 'pinia'
import { RouterEnum } from '@/Models/enum'


const otp: Ref<string> = ref('')
const otpFromServer: Ref<string> = ref('')
const errorMessages: Ref<string> = ref('')
const { email } = storeToRefs(AccountStore())
const { fetchOtp } = AccountStore()
const { Loading } = storeToRefs(GlobalStore())
const form = ref()
const router = useRouter()

const VerifyOTP = async () => {
    errorMessages.value = ''
    if (otp.value.length < 6) {
        errorMessages.value = 'Enter valid otp'
        return;
    }

    if (otp.value != otpFromServer.value) {
        errorMessages.value = 'Wrong otp'
        return
    }
    router.push({ name: RouterEnum.PASSWORD_RESET })
}
onMounted(async () => {
    if (email.value == '') {
        router.push({ name: RouterEnum.VERIFY_EMAIL })
        return
    }
    otpFromServer.value = await fetchOtp(email.value)
})
</script>
<template>
    <v-form ref="form" validate-on="submit">
        <p class="ma-2 text-grey">Please check your mail for OTP</p>
        <otp-input v-model="otp" :error="errorMessages"></otp-input>
        <div class="text-center">
            <v-btn color="primary" @click="VerifyOTP" :loading="Loading">
                Verify OTP
            </v-btn>
        </div>
    </v-form>
</template>