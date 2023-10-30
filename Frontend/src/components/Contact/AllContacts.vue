<script setup lang="ts">
import { ContactStore } from '@/stores'
import { storeToRefs } from 'pinia'

const { Contacts } = storeToRefs(ContactStore())

</script>
<template>
    <v-list>
        <div v-for="(contact, index) in Contacts" :key="index">
            <v-list-item class="px-0">
                <v-row class="align-center">
                    <v-col cols="2" sm="1">
                        <v-avatar class="text-white text-uppercase" color="primary">
                            {{ contact.email.slice(0, 1) }}
                        </v-avatar>
                    </v-col>
                    <v-col cols="3">
                        {{ contact.userName }}
                    </v-col>
                    <v-col>{{ contact.email }}</v-col>
                </v-row>
            </v-list-item>
            <v-divider class="mb-5 mt-1"></v-divider>
        </div>
    </v-list>
</template>
