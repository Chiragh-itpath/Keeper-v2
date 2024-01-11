<script setup lang="ts">
import { ref, reactive, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { RouterEnum } from '@/Models/enum'
import { TextField } from '@/components/Custom'
import { AccountStore, GlobalStore } from '@/stores'
import type { IRegister } from '@/Models/UserModels'
const { registerUser } = AccountStore()
const { Loading, errors } = storeToRefs(GlobalStore())
const form = ref()
const signupClicked: Ref<boolean> = ref(false)
const SignUpForm = reactive<IRegister>({
    userName: '',
    email: '',
    mobile: '',
    password: '',
    confirmPassword: ''
})

const checkPassword = (): boolean | string => {
    return SignUpForm.password == SignUpForm.confirmPassword
        ? true
        : 'Password and Confirm Password must be same'
}

async function register(): Promise<void> {
    signupClicked.value = true
    errors.value = {}
    const { valid } = await form.value.validate()
    if (!valid) return
    await registerUser(SignUpForm)
}
onMounted(() => {
    errors.value = {}
})
</script>
<template>
    <v-container fluid class="my-10 mb-md-0">
        <v-row justify="center">
            <v-col cols="12" sm="8" md="6" lg="4" xl="3">
                <v-card class="pa-4 elevation-12 rounded-xl">
                    <v-card-title class="text-center mt-5">
                        <h2 class="text-teal">Keeper</h2>
                        Sign Up
                    </v-card-title>
                    <v-card-subtitle class="text-center mb-5">
                        to continue to Keeper
                    </v-card-subtitle>
                    <v-form @submit.prevent="register" ref="form" :validate-on="signupClicked ? 'input' : 'submit'">
                        <div class="mx-5">
                            <text-field v-model="SignUpForm.userName" label="Full Name" icon="mdi-account" is-required />
                            <text-field v-model="SignUpForm.email" label="Email" icon="mdi-email" is-required is-email
                                :error-messages="errors.email" />
                            <text-field v-model="SignUpForm.mobile" label="Mobile Number" icon="mdi-account" is-required
                                is-contact :max-limit="10" />
                            <text-field v-model="SignUpForm.password" label="Password" icon="mdi-lock" is-required
                                is-password :max-limit="16" />
                            <text-field v-model="SignUpForm.confirmPassword" label="Confirm Password" icon="mdi-lock"
                                is-required is-password :-validation-rules="[checkPassword]" :max-limit="16" />
                        </div>
                        <v-card-actions>
                            <div class="d-flex flex-column justify-center mx-auto">
                                <v-btn type="submit" flatcolor="#5865f2" rounded="lg" size="large" variant="flat"
                                    color="teal" class="mt-4" :loading="Loading">
                                    Sign Up
                                </v-btn>
                                <div class="mt-5">
                                    Already have an account?
                                    <router-link :to="{ name: RouterEnum.LOGIN }">Sign In</router-link>
                                </div>
                            </div>
                        </v-card-actions>
                    </v-form>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
