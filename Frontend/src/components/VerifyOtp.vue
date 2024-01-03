<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { OtpInput } from '@/components/Custom/'
import { GlobalStore } from '@/stores'
import { storeToRefs } from 'pinia'


const props = defineProps<{
    opt: string
}>()

const otp: Ref<string> = ref('')
const errorMessages: Ref<string> = ref('')
const { Loading } = storeToRefs(GlobalStore())

const form = ref()

const VerifyOTP = async () => {
    errorMessages.value = ''
    if (otp.value.length < 6) {
        errorMessages.value = 'Enter valid otp'
        return;
    }
    if (otp.value != props.opt) {
        errorMessages.value = 'Wrong otp'
        return
    }
    emit('valid')
}
const emit = defineEmits<{
    (e: 'valid'): void
}>()

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