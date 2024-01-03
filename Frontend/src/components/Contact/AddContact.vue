<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { ContactStore, GlobalStore, UserStore } from '@/stores'
import { debounce } from 'lodash'
import { storeToRefs } from 'pinia'
import type { IUser } from '@/Models/UserModels'

const visible: Ref<boolean> = ref(false)
const email: Ref<string> = ref('')
const items: Ref<any[]> = ref([])
const selected = ref()
const selectedUsers: Ref<IUser[]> = ref([])
const menu: Ref<boolean> = ref(false)
const { SearchEmail } = UserStore()
const { AddContact } = ContactStore()
const { Contacts } = storeToRefs(ContactStore())
const isLoading: Ref<boolean> = ref(false)
const { Loading } = storeToRefs(GlobalStore())
const inputHandler = debounce(async (): Promise<void> => {
    isLoading.value = true
    menu.value = false
    if (email.value.trim() == '') {
        items.value = []
        isLoading.value = false
        return
    }
    const res = await SearchEmail(email.value)
    items.value = res.filter(x =>
        !selectedUsers.value.some(user => user.id === x.id)
    ).filter(x =>
        !Contacts.value.some(user => user.addedPerson.id === x.id)
    ).map(x => {
        return {
            title: x.userName,
            subtitle: x.email,
            value: x
        }
    })
    isLoading.value = false
    menu.value = true
}, 500)
const submitHandler = async () => {
    await AddContact(selectedUsers.value.map(x => x.id))
    visible.value = false
}
const removeSelection = (id: string) => {
    selectedUsers.value.splice(selectedUsers.value.findIndex(x => x.id == id), 1)
}
watch(visible, () => {
    if (visible.value) {
        email.value = ''
        items.value = []
        selected.value = undefined
        selectedUsers.value = []
    }
})
watch(email, inputHandler);
watch(selected, () => {
    if (selected.value == undefined) return
    selectedUsers.value.push(selected.value)
    items.value = []
    selected.value = undefined
    email.value = ''
})
</script>
<template>
    <v-btn color="primary" prepend-icon="mdi-plus" @click="visible = !visible">New Contact</v-btn>
    <v-dialog v-model="visible" max-width="700">
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Contact</span>
                <span class="float-right">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-autocomplete :items="items" v-model:search="email" :active="false" v-model:menu="menu"
                    v-model:model-value="selected" placeholder="Enter email address" color="primary" :loading="isLoading"
                    label="Email" :hide-no-data="isLoading || email == ''" :menu-props="{ closeOnBack: true }">
                    <template v-slot:item="{ props, item }">
                        <v-list-item v-bind="props" :title="item.title" :subtitle="item.raw.subtitle" :value="item.value">
                        </v-list-item>
                    </template>
                    <template v-slot:chip="{ item, props }">
                        <v-chip v-bind="props" color="primary">
                            <template v-slot:prepend>
                                <v-avatar color="red" class="me-2">{{ item.raw.subtitle?.slice(0, 1) }}</v-avatar>
                            </template>
                            {{ item.raw.subtitle }}
                        </v-chip>
                    </template>
                    <template v-slot:no-data>
                        <v-list-item>No User found with this Email</v-list-item>
                    </template>
                </v-autocomplete>
                <v-list max-height="200" class="overflow-y-auto px-5">
                    <v-list-item v-for="(user, index) in selectedUsers" :key="index" class="border mb-3 px-2 py-3">
                        <template v-slot:prepend>
                            <v-avatar color="primary">
                                {{ user.email.slice(0, 1) }}
                            </v-avatar>
                        </template>
                        <template v-slot:title>
                            {{ user.userName }}
                        </template>
                        <template v-slot:subtitle>
                            {{ user.email }}
                        </template>
                        <template v-slot:append>
                            <v-icon color="danger" @click="() => removeSelection(user.id)">mdi-delete</v-icon>
                        </template>
                    </v-list-item>
                </v-list>
            </v-card-text>
            <v-card-actions class="justify-end px-4 pb-4">
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" @click="submitHandler"
                    :loading="Loading" :disabled="Loading || selectedUsers.length == 0">
                    Add
                    <span v-if="selectedUsers.length > 0">({{ selectedUsers.length }})</span>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>