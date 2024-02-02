<script setup lang="ts">
import { useDisplay } from 'vuetify'
import { ContactStore } from '@/stores'
import { EditContact } from '@/components/Contact'
import { DeletePrompt } from '@/components/Custom'
import { type IContact } from '@/Models/ContactModels'
defineProps<{
    contacts: IContact[]
}>()
const { smAndUp } = useDisplay()
const { DeleteContact } = ContactStore()
</script>
<template>
    <template v-if="smAndUp">
        <v-card class="mt-10">
            <v-card-title class="text-body-1 bg-primary">
                <v-row>
                    <v-col cols="1"></v-col>
                    <v-col cols="2">First name</v-col>
                    <v-col cols="2">Last name</v-col>
                    <v-col>Email</v-col>
                    <v-col cols="2" class="text-end">Actions</v-col>
                </v-row>
            </v-card-title>
            <v-card-text class="pt-3">
                <v-col v-if="contacts.length == 0" cols="12" class="text-center text-grey pa-2">
                    No contacts
                </v-col>
                <template v-for="(contact, index) in contacts" :key="index">
                    <v-row :class="index < contacts.length - 1 ? 'border-b' : ''">
                        <v-col cols="1">
                            <v-avatar size="small" color="primary">
                                {{
                                    `${contact.firstName.charAt(0).toUpperCase()}${contact.lastName.charAt(0).toUpperCase()}`
                                }}
                            </v-avatar>
                        </v-col>
                        <v-col cols="12" sm="2">
                            {{ contact.firstName }}
                        </v-col>
                        <v-col cols="12" sm="2">
                            {{ contact.lastName }}
                        </v-col>
                        <v-col>
                            {{ contact.email }}
                        </v-col>
                        <v-col cols="12" sm="2">
                            <div class="d-flex float-end">
                                <edit-contact :contact="contact" :contacts="contacts" v-slot="{ activator }">
                                    <v-icon icon="mdi-pencil" color="primary" size="large" v-bind="activator" />
                                </edit-contact>
                                <delete-prompt width="500" title="Delete Contact"
                                    @click:yes="() => DeleteContact(contact.id)">
                                    <template v-slot="{ props }">
                                        <v-icon icon="mdi-delete" color="danger" size="large" class="mx-2" v-bind="props" />
                                    </template>
                                    <template v-slot:alert>
                                        <v-alert text="This action will also remove contact from all groups."
                                            color="warning" class="mx-0 mt-1">
                                            <template v-slot:prepend>
                                                <v-icon icon="mdi-alert" color="white" />
                                            </template>
                                        </v-alert>
                                    </template>
                                </delete-prompt>
                            </div>
                        </v-col>
                    </v-row>
                </template>
            </v-card-text>
        </v-card>
    </template>
    <template v-else>
        <v-sheet v-if="contacts.length == 0" height="300" class="mt-10 d-flex align-center justify-center flex-column">
            <v-icon color="grey" icon="mdi-account-off" size="50" />
            <span class="text-grey">No contacts</span>
        </v-sheet>
        <v-list lines="two" class="bg-transparent mt-10">
            <template v-for="(contact, index) in contacts" :key="index">
                <v-list-item :title="`${contact.firstName} ${contact.lastName}`" :subtitle="contact.email"
                    class="py-2 pe-1 bg-white mb-3">
                    <template v-slot:prepend>
                        <v-avatar size="small" color="primary">
                            {{
                                `${contact.firstName.charAt(0).toUpperCase()}${contact.lastName.charAt(0).toUpperCase()}`
                            }}
                        </v-avatar>
                    </template>
                    <template v-slot:append>
                        <v-menu location="left">
                            <template v-slot:activator="{ props }">
                                <v-icon icon="mdi-dots-vertical" v-bind="props" />
                            </template>
                            <v-list density="compact" width="120">
                                <edit-contact :contact="contact" :contacts="contacts" v-slot="{ activator }">
                                    <v-list-item density="compact" v-bind="activator">
                                        <v-icon size="small" icon="mdi-pencil" /> Edit
                                    </v-list-item>
                                </edit-contact>
                                <delete-prompt width="500" title="Delete Contact"
                                    @click:yes="() => DeleteContact(contact.id)">
                                    <template v-slot="{ props }">
                                        <v-list-item density="compact" v-bind="props">
                                            <v-icon size="small" icon="mdi-delete" /> Delete
                                        </v-list-item>
                                    </template>
                                    <template v-slot:alert>
                                        <v-alert text="This action will also remove contact from all groups."
                                            color="warning" class="mx-0 mt-1">
                                            <template v-slot:prepend>
                                                <v-icon icon="mdi-alert" color="white" />
                                            </template>
                                        </v-alert>
                                    </template>
                                </delete-prompt>
                            </v-list>
                        </v-menu>
                    </template>
                </v-list-item>
            </template>
        </v-list>
    </template>
</template>
