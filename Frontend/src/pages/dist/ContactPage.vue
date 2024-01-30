<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { AddContact, AllGroups, AddGroup } from '@/components/Contact'
import { ContactStore, GroupStore, ProjectStore } from '@/stores'
import type { IContact } from '@/Models/ContactModels'

const tabs = ref()
const contactSearch: Ref<string> = ref('')
const groupSearch: Ref<string> = ref('')
const contactsToDisplay: Ref<IContact[]> = ref([])
const { GetContacts } = ContactStore()
const { GetGroups } = GroupStore()
const { Contacts } = storeToRefs(ContactStore())
const { isProjectFetched, Projects } = storeToRefs(ProjectStore())

const loading: Ref<boolean> = ref(false)
onMounted(async () => {
    loading.value = true
    await GetContacts()
    await GetGroups()
    if (!isProjectFetched.value) {
        await ProjectStore().GetProjects()
    }
    contactsToDisplay.value = Contacts.value
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
                            hide-details v-model="contactSearch" density="comfortable">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                            <template v-slot:clear>
                                <v-icon @click="contactSearch = ''">mdi-close-circle-outline</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="auto" class="text-end me-2">
                        <add-contact :contacts="Contacts" :project="Projects" />
                    </v-col>
                </v-row>
                <v-card class="mt-10">
                    <v-card-text>
                        <v-row>
                            <v-col cols="1">#</v-col>
                            <v-col>name</v-col>
                            <v-col>Email</v-col>
                            <v-col>Projects</v-col>
                        </v-row>
                        <template v-for="(contact, index) in contactsToDisplay" :key="index">
                            <v-row class="border-t mt-3">
                                <v-col cols="1">
                                    <v-avatar :text="contact.addedPerson.email[0].toUpperCase()" color="primary"></v-avatar>
                                </v-col>
                                <v-col>{{ contact.addedPerson.userName }}</v-col>
                                <v-col>{{ contact.addedPerson.email }}</v-col>
                                <v-col>
                                    {{
                                        Projects.filter(p => {
                                            return p.users.some(x => x.invitedUser.id == contact.addedPerson.id)
                                        }).length
                                    }}
                                </v-col>
                            </v-row>
                        </template>
                    </v-card-text>
                </v-card>
            </v-window-item>
            <v-window-item value="group">
                <v-row class="mt-10 align-center">
                    <v-col>
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            hide-details v-model="groupSearch" density="comfortable">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                            <template v-slot:clear>
                                <v-icon @click="groupSearch = ''">mdi-close-circle-outline</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="auto" class="text-center">
                        <add-group :contacts="Contacts"></add-group>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12">
                        <all-groups :search="groupSearch"></all-groups>
                    </v-col>
                </v-row>
            </v-window-item>
        </v-window>
    </v-container>
</template>