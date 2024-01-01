<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue';
import { AllContacts, AddContact, AllGroups, AddGroup } from '@/components/Contact'
import { ContactStore, GroupStore } from '@/stores'
import { storeToRefs } from 'pinia'

const tabs = ref()
const contactSearch: Ref<string> = ref('')
const groupSearch: Ref<string> = ref('')
const { GetContacts } = ContactStore()
const { GetGroups } = GroupStore()
const { Contacts } = storeToRefs(ContactStore())
const loading: Ref<boolean> = ref(false)
onMounted(async () => {
    loading.value = true
    await GetContacts()
    await GetGroups()
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
                <v-card elevation="0" class="d-flex flex-row-reverse">
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
                            hide-details v-model="contactSearch">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="auto" class="text-end me-2">
                        <add-contact></add-contact>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12">
                        <all-contacts :search="contactSearch" :contacts="Contacts"></all-contacts>
                    </v-col>
                </v-row>
            </v-window-item>
            <v-window-item value="group">
                <v-row class="mt-10 align-center">
                    <v-col>
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            hide-details v-model="groupSearch">
                            <template v-slot:prepend-inner>
                                <v-icon color="primary">mdi-magnify</v-icon>
                            </template>
                        </v-text-field>
                    </v-col>
                    <v-col cols="auto" class="text-center">
                        <add-group></add-group>
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