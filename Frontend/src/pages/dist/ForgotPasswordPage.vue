<script setup lang="ts">
import { ref, type Ref } from 'vue'
import VerifyEmail from '@/components/VerifyEmail.vue'
import VerifyOtp from '@/components/VerifyOtp.vue'
import ResetPassword from '@/components/ResetPassword.vue'
import { AccountStore } from '@/stores'

const email: Ref<string> = ref('')
const otp: Ref<string> = ref('')
const window: Ref<'email' | 'otp' | 'reset'> = ref('email')
const { fetchOtp } = AccountStore()
const getOtp = async () => {
    const serverOtp = await fetchOtp(email.value)
    if (serverOtp != '') {
        otp.value = serverOtp
        window.value = 'otp'
    }
}
</script>
<template>
    <v-container fill-height fluid class="bg-blue-grey-lighten-5">
        <v-row justify="center" align-content="center" class="h-screen">
            <v-col cols="12" xl="3" lg="4" md="6" sm="9">
                <v-card elevation="10" class="rounded-xl">
                    <v-card-title class="text-center mt-5">
                        <h2 class="text-teal">Keeper</h2>
                    </v-card-title>
                    <v-card-text>
                        <div class="text-center text-h6 mb-6">Reset Your Password</div>
                        <v-window v-model="window" :touch="false" translate="yes">
                            <v-window-item value="email">
                                <verify-email v-model="email" @valid="getOtp"></verify-email>
                            </v-window-item>
                            <v-window-item value="otp">
                                <verify-otp :opt="otp" @valid="() => window = 'reset'"></verify-otp>
                            </v-window-item>
                            <v-window-item value="reset">
                                <reset-password :email="email"></reset-password>
                            </v-window-item>
                        </v-window>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
<style scoped>
.h-screen {
    height: calc(100vh - 10px) !important;
}
</style>