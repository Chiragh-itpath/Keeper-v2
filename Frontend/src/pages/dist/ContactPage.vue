<script setup lang="ts">
import AddGroup from '@/components/Contact/AddGroup.vue'
import AddContact from '@/components/Contact/AddContact.vue'
import AllContacts from '@/components/Contact/AllContacts.vue';
</script>
<template>
    <v-container class="overflow-auto h-screen">
        <v-row class="mt-10 align-center">
            <v-col cols="12" md="8">
                <v-text-field color="primary" model-value="will added later" disabled hide-details></v-text-field>
            </v-col>
            <v-col cols="6" md="2" class="text-center">
                <add-contact></add-contact>
            </v-col>
            <v-col cols="6" md="2" class="text-center">
                <add-group></add-group>
            </v-col>
        </v-row>
        <v-row class="mt-10">
            <v-col>
                <all-contacts></all-contacts>
            </v-col>
        </v-row>
    </v-container>
</template>