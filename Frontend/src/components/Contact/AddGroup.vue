<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { GlobalStore, GroupStore } from '@/stores'
import TextField from '@/components/Custom/TextField.vue'
import type { IContact } from '@/Models/ContactModels'
import type { IAddGroup } from '@/Models/GroupModels'

const props = defineProps<{
    contacts: IContact[]
}>()

const form = ref()
const visible: Ref<boolean> = ref(false)
const name: Ref<string> = ref('')
const Contacts: Ref<IContact[]> = ref(props.contacts)
const contactList: Ref<string[]> = ref([])
const window: Ref<'group' | 'contact'> = ref('group')

const { AddGroup } = GroupStore()
const { Loading } = storeToRefs(GlobalStore())

const validateForm = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return
    window.value = 'contact'
}
const searchHandler = (searchVal: string | null) => {
    if (searchVal && searchVal != '') {
        Contacts.value = props.contacts.filter(x => x.firstName.startsWith(searchVal) || x.email.startsWith(searchVal))
    } else {
        Contacts.value = props.contacts
    }
}
const selectHandler = (selected: unknown[]) => {
    contactList.value = selected.map(item => String(item))
}
const handleSubmit = async () => {
    const { valid } = await form.value.validate()
    if (!valid) {
        window.value = 'group'
        return
    }
    const group: IAddGroup = {
        name: name.value,
        contactId: contactList.value
    }
    await AddGroup(group)
    visible.value = false;
}

watch(visible, () => {
    if (visible.value) {
        window.value = 'group'
    } else {
        form.value.reset()
    }
})
</script>
<template>
    <v-dialog v-model="visible" max-width="600" class="">
        <template v-slot:activator="{ props }">
            <v-btn color="primary" prepend-icon="mdi-plus" class="rounded" v-bind="props">
                New Group
            </v-btn>
        </template>
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Group</span>
                <span class="float-right">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-window v-model="window">
                    <v-window-item value="group">
                        <v-form ref="form" validate-on="submit" @submit.prevent="window = 'contact'">
                            <text-field label="Name" placeholder="Enter name of your group" v-model="name"
                                is-required></text-field>
                        </v-form>
                    </v-window-item>
                    <v-window-item value="contact">
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            @update:model-value="searchHandler" />
                        <v-card max-height="500" elevation="0">
                            <v-list @update:selected="selectHandler" select-strategy="classic">
                                <template v-for="(contact, index) in Contacts" :key="index">
                                    <v-list-item :title="`${contact.firstName} ${contact.lastName}`"
                                        :subtitle="contact.email" :value="contact.id" class="border rounded-lg mb-3 py-1">
                                        <template v-slot:prepend="{ isSelected }">
                                            <v-checkbox density="compact" hide-details color="primary" class="me-2"
                                                :model-value="isSelected" />
                                            <v-avatar :text="contact.email.charAt(0).toUpperCase()" color="primary" />
                                        </template>
                                    </v-list-item>
                                </template>
                                <v-list-item v-if="Contacts.length == 0" title="No Contact found"
                                    class="bg-grey-lighten-3 text-center text-grey rounded-lg" />
                            </v-list>
                        </v-card>
                    </v-window-item>
                </v-window>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn class="rounded-xl mx-2 mb-2" variant="elevated" color="primary" width="120" v-if="window == 'group'"
                    append-icon="mdi-arrow-right" @click="validateForm">
                    Next
                </v-btn>
                <v-btn class="rounded-xl mx-2 mb-2" variant="elevated" color="primary" width="120" v-else
                    @click="handleSubmit" :loading="Loading" :disabled="Loading">
                    Done
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>