<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { AddContact, AllGroups, AddGroup, AllContacts } from '@/components/Contact'
import { ContactStore, GroupStore, ProjectStore } from '@/stores'
import type { IContact } from '@/Models/ContactModels'
import type { IGroup } from '@/Models/GroupModels'

const tabs = ref()
const contactsToDisplay: Ref<IContact[]> = ref([])
const groupsToDisplay: Ref<IGroup[]> = ref([])
const { GetContacts } = ContactStore()
const { GetGroups } = GroupStore()
const { Contacts } = storeToRefs(ContactStore())
const { Groups } = storeToRefs(GroupStore())
const { isProjectFetched, Projects } = storeToRefs(ProjectStore())

const loading: Ref<boolean> = ref(false)

const contactSearchHandler = (value: string | null) => {
    contactsToDisplay.value = Contacts.value.filter(contact =>
        !value || contact.firstName.startsWith(value) || contact.email.startsWith(value)
    )
}
const groupSearchHandler = (value: string | null) => {
    groupsToDisplay.value = Groups.value.filter(group => !value || group.name.startsWith(value))
}
onMounted(async () => {
    loading.value = true
    await GetContacts()
    await GetGroups()
    if (!isProjectFetched.value) {
        await ProjectStore().GetProjects()
    }
    contactsToDisplay.value = Contacts.value
    groupsToDisplay.value = Groups.value
    loading.value = false
})
</script>
<template>
    <v-container class="overflow-auto" fluid>
        <v-row v-if="loading">
            <v-col cols="12">
                <v-skeleton-loader type="heading"></v-skeleton-loader>
            </v-col>
            <v-col cols="12">
                <v-skeleton-loader type="list-item,table-tbody"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading">
            <v-col cols="12">
                <v-card elevation="0" class="d-flex flex-row-reverse bg-transparent">
                    <v-tabs color="primary" v-model="tabs">
                        <v-tab value="contact" class="px-10">Contacts</v-tab>
                        <v-tab value="group" class="px-10">Groups</v-tab>
                    </v-tabs>
                </v-card>
            </v-col>
        </v-row>
        <v-window v-model="tabs" v-if="!loading">
            <v-window-item value="contact">
                <v-row class="mt-10 align-center">
                    <v-col>
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            hide-details density="comfortable" @update:model-value="contactSearchHandler"
                            clear-icon="mdi-close-circle-outline">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="12" sm="auto" class="text-end me-2">
                        <add-contact :contacts="Contacts" :projects="Projects" />
                    </v-col>
                </v-row>
                <all-contacts :contacts="contactsToDisplay"></all-contacts>
            </v-window-item>
            <v-window-item value="group">
                <v-row class="mt-10 align-center">
                    <v-col>
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            hide-details density="comfortable" clear-icon="mdi-close-circle-outline"
                            @update:model-value="groupSearchHandler">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="12" sm="auto" class="text-end me-2">
                        <add-group :contacts="Contacts"></add-group>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12">
                        <all-groups :groups="groupsToDisplay"></all-groups>
                    </v-col>
                </v-row>
            </v-window-item>
        </v-window>
    </v-container>
</template>