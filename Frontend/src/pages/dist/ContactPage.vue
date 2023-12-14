<script setup lang="ts">
import { ref, type Ref } from 'vue';
import AddGroup from '@/components/Contact/AddGroup.vue'
import AddContact from '@/components/Contact/AddContact.vue'
import AllContacts from '@/components/Contact/AllContacts.vue'
import AllGroups from '@/components/Contact/AllGroups.vue'

const tabs = ref()
const searchText: Ref<string> = ref('')
</script>
<template>
    <v-container class="overflow-auto" fluid>
        <v-row class="mt-10 align-center">
            <v-col cols="12" md="8">
                <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable hide-details
                    v-model="searchText"></v-text-field>
            </v-col>
            <v-col cols="6" md="2" class="text-center">
                <add-contact></add-contact>
            </v-col>
            <v-col cols="6" md="2" class="text-center">
                <add-group></add-group>
            </v-col>
        </v-row>
        <v-row class="mt-10">
            <v-col cols="12">
                <v-card elevation="8">
                    <v-tabs color="primary" v-model="tabs">
                        <v-tab value="contact" class="w-50">Contacts</v-tab>
                        <v-tab value="group" class="w-50">Groups</v-tab>
                    </v-tabs>
                </v-card>
            </v-col>
            <v-col cols="12">
                <v-window v-model="tabs">
                    <v-window-item value="contact">
                        <all-contacts :search="searchText"></all-contacts>
                    </v-window-item>
                    <v-window-item value="group">
                        <all-groups :search="searchText"></all-groups>
                    </v-window-item>
                </v-window>
            </v-col>
        </v-row>
    </v-container>
</template>