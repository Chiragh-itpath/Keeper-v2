<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { UserStore } from '@/stores'
import { ContactStore } from '@/stores'
import { debounce } from 'lodash'
const visible: Ref<boolean> = ref(false)
const email: Ref<string> = ref('')
const selectedEmail: Ref<string | undefined> = ref()
const items: Ref<string[]> = ref([])
const error: Ref<string> = ref('')
const { SearchEmail } = UserStore()
const { AddContact } = ContactStore()
const isLoading: Ref<boolean> = ref(false)

const inputHandler = debounce(async (): Promise<void> => {
    error.value = ''
    if (email.value.trim() == '') {
        isLoading.value = false
        return
    }
    const res = await SearchEmail(email.value)
    items.value = res;
    isLoading.value = false
}, 500)
const submitHandler = async () => {
    if (selectedEmail.value == '' || selectedEmail.value == null) {
        error.value = 'Please enter valid email'
        return
    }
    await AddContact(selectedEmail.value)
    visible.value = false
    items.value = []
    email.value = ''
    selectedEmail.value = ''
}
watch(visible, () => {
    if (visible.value) {
        email.value = ''
        items.value = []
        error.value = ''
    }
})
</script>
<template>
    <v-btn color="primary" prepend-icon="mdi-plus" @click="visible = !visible" width="100%">New Contact</v-btn>
    <v-dialog v-model="visible" max-width="600">
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Contact</span>
                <span class="float-right">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-autocomplete :items="items" v-model:search="email" v-model:model-value="selectedEmail"
                    placeholder="Enter email address" color="primary" :loading="isLoading" label="Email"
                    prepend-inner-icon="mdi-email" :multiple="false" hide-no-data :error-messages="error" @update:search="() => {
                        isLoading = true
                        inputHandler()
                    }">
                    <template v-slot:chip="{ props, item }">
                        <v-chip color="primary" v-bind="props" v-if="item.title">
                            <template v-slot:prepend>
                                <v-avatar class="text-uppercase bg-red me-2">{{ item.value.slice(0, 1) }}</v-avatar>
                            </template>
                            {{ item.value }}
                        </v-chip>
                    </template>
                </v-autocomplete>
            </v-card-text>
            <v-card-actions class="justify-end px-4 pb-4">
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" @click="submitHandler">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>