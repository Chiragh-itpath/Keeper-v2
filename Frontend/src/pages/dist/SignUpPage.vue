<script setup lang="ts">
import { ref, reactive, type Ref } from 'vue'
import { RouterEnum } from '@/Models/enum'
import TextField from '@/components/Custom/TextField.vue'
import { AccountStore, GlobalStore } from '@/stores'
import type { IRegister } from '@/Models/UserModels'
import { storeToRefs } from 'pinia'
const { registerUser } = AccountStore()
const { Loading, errors } = storeToRefs(GlobalStore())
const form = ref()
const signupClicked: Ref<boolean> = ref(false)
const SignUpForm = reactive<IRegister>({
    userName: '',
    email: '',
    contact: '',
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

</script>
<template>
    <v-container fluid class=" h-screen">
        <v-row justify="center" align-content="center" class="h-100">
            <v-col cols="12" sm="9" md="6" lg="4">
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
                            <text-field v-model="SignUpForm.userName" label="Username" icon="mdi-account" is-required />
                            <text-field v-model="SignUpForm.email" label="Email" icon="mdi-email" is-required is-email
                                :error-messages="errors.email" />
                            <text-field v-model="SignUpForm.contact" label="Contact" icon="mdi-account" is-required
                                is-contact />
                            <text-field v-model="SignUpForm.password" label="Password" icon="mdi-lock" is-required
                                is-password />
                            <text-field v-model="SignUpForm.confirmPassword" label="Confirm Password" icon="mdi-lock"
                                is-required is-password :-validation-rules="[checkPassword]" />
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
