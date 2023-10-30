<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { UserStore } from '@/stores'
import { ContactStore } from '@/stores'

const visible: Ref<boolean> = ref(false)
const email: Ref<string> = ref('')
const emailList: Ref<string[]> = ref([])
const error: Ref<string> = ref('')
const { SearchEmail } = UserStore()
const { AddContact } = ContactStore()
const inputHandler = async (): Promise<void> => {
    error.value = ''
    const res = await SearchEmail(email.value)
    emailList.value = res;
}
const submitHandler = async () => {
    const find = emailList.value.find(x => x == email.value)
    if (!find) {
        error.value = 'Please select email'
        return
    }
    await AddContact(email.value)
    visible.value = false
}
watch(visible, () => {
    if (visible.value) {
        email.value = ''
        emailList.value = []
        error.value = ''
    }
})
</script>
<template>
    <v-btn color="primary" append-icon="mdi-plus" @click="visible = !visible" width="100%">New Contact</v-btn>
    <v-dialog v-model="visible" max-width="600">
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Contact</span>
                <span class="float-right">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-combobox :items="emailList" v-model:search="email" placeholder="Enter email address" color="primary"
                    label="Email" prepend-inner-icon="mdi-email" :multiple="false" @update:search="inputHandler"
                    :error-messages="error">
                    <template v-slot:chip="{ item }">
                        <v-chip color="primary">
                            <template v-slot:prepend>
                                <v-avatar class="text-uppercase bg-red me-2">{{ item.value.slice(0, 1) }}</v-avatar>
                            </template>
                            {{ item.value }}
                        </v-chip>
                    </template>
                </v-combobox>
            </v-card-text>
            <v-card-actions class="justify-end px-4 pb-4">
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" @click="submitHandler">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>