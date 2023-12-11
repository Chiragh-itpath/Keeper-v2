<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

import AddKeep from '@/components/keeps/AddKeep.vue'
import AllKeeps from '@/components/keeps/AllKeeps.vue'
import DatePicker from '@/components/Custom/DatePicker.vue'
import { RouterEnum } from '@/Models/enum'

const date = ref()
const route = useRoute()
const router = useRouter()

const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
</script>
<template>
    <v-container class="overflow-auto h-screen bg-blue-grey-lighten-5" fluid>
        <v-row class="mx-5">
            <v-col cols="12">
                <v-btn color="primary" prepend-icon="mdi-arrow-left" @click="router.push({ name: RouterEnum.PROJECT })">
                    Back
                </v-btn>
            </v-col>
            <v-col>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" xl="2" class="my-auto d-flex justify-end">
                <add-keep :project-id="projectId"></add-keep>
            </v-col>
        </v-row>
        <v-row class="mx-5">
            <v-col cols="12" v-if="date || route.name == RouterEnum.KEEP_BY_TAG">
                Filter By:
                <v-chip color="black" closable v-if="date" @click:close="date = ''" class="mx-3 pa-3">Date</v-chip>
                <v-chip color="black" closable v-if="route.name == RouterEnum.KEEP_BY_TAG"
                    @click:close="router.push({ name: RouterEnum.KEEP })">
                    Tag
                </v-chip>
            </v-col>
            <all-keeps :date="date"></all-keeps>
        </v-row>
    </v-container>
</template>
