<script setup lang="ts">

import type { IGroup } from '@/Models/GroupModels'
import GroupDetails from '@/components/Contact/GroupDetails.vue'

const { groups } = defineProps<{
    groups: IGroup[]
}>()

</script>
<template>
    <v-card>
        <v-card-title class="bg-primary text-body-1">
            <v-row>
                <v-col cols="1"></v-col>
                <v-col>Name</v-col>
                <v-col>Contacts</v-col>
            </v-row>
        </v-card-title>
        <v-card-text class="pt-3">
            <template v-for="(group, index) in groups" :key="index">
                <group-details :group="group" v-slot="{ activator }">
                    <v-row v-bind="activator" class="cursor-pointer">
                        <v-col cols="1">
                            <v-icon color="primary" icon="mdi-account-group-outline" />
                        </v-col>
                        <v-col>{{ group.name }}</v-col>
                        <v-col>{{ group.contacts.length }}</v-col>
                    </v-row>
                </group-details>
            </template>
        </v-card-text>
    </v-card>
</template>