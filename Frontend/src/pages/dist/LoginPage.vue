<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { RouterEnum } from '@/Models/enum'
import type { ILogin } from '@/Models/UserModels'
import TextField from '@/components/Custom/TextField.vue'
import { AccountStore, GlobalStore } from '@/stores'
import { storeToRefs } from 'pinia'
const { loginUser } = AccountStore()
const { Loading, errors } = storeToRefs(GlobalStore())

const form = ref()
const loginForm = reactive<ILogin>({
    email: '',
    password: ''
})

const login = async (): Promise<void> => {
    errors.value = {}
    const { valid } = await form.value.validate()
    if (!valid) return
    await loginUser(loginForm)
}
onMounted(() => {
    errors.value = {}
})
</script>
<template>
    <v-container fluid class="h-screen bg-blue-grey-lighten-5">
        <v-row justify="center" align-content="center" class="h-100">
            <v-col cols="12" lg="4" md="6" sm="8">
                <v-card elevation="10" class="rounded-xl">
                    <v-card-title class="text-center mt-5">
                        <h2 class="text-teal">Keeper</h2>
                        Login
                    </v-card-title>
                    <v-card-subtitle class="text-center">
                        to continue to Keeper
                    </v-card-subtitle>
                    <v-card-text>
                        <v-form ref="form" @submit.prevent="login" validate-on="submit">
                            <div class="mx-5">
                                <text-field v-model="loginForm.email" label="Email" is-required is-email icon="mdi-email"
                                    :error-messages="errors.email" />
                                <text-field v-model="loginForm.password" label="Password" is-required is-password
                                    icon="mdi-lock" :error-messages="errors.password" />
                            </div>
                            <div class="text-right">
                                <router-link :to="{ name: RouterEnum.VERIFY_EMAIL }">
                                    Forgot Password?
                                </router-link>
                            </div>
                            <v-card-actions>
                                <div class="d-flex flex-column justify-center mx-auto">
                                    <v-btn type="submit" flatcolor="#5865f2" rounded="lg" size="large" variant="flat"
                                        color="teal" class="mt-4" :loading="Loading">
                                        Login
                                    </v-btn>
                                    <div class="mt-5">
                                        New User?
                                        <router-link :to="{ name: RouterEnum.SIGNUP }">
                                            Create an account
                                        </router-link>
                                    </div>
                                </div>
                            </v-card-actions>
                        </v-form>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
<style scoped>
.blur {
    opacity: 0.2;
}

img {
    width: 100vw;
    height: 100vh;
}
</style>
