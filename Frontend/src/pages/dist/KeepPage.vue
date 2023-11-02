<script setup lang="ts">
import { computed, ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useRoute, useRouter } from 'vue-router'
import { KeepStore } from '@/stores'

import AddKeep from '@/components/keeps/AddKeep.vue'
import AllKeeps from '@/components/keeps/AllKeeps.vue'
import NoItem from '@/components/NoItem.vue'
import DatePicker from '@/components/Custom/DatePicker.vue'
import { RouterEnum } from '@/Models/enum'

const { Keeps } = storeToRefs(KeepStore())
const date = ref()
const route = useRoute()
const router = useRouter()

const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
</script>
<template>
    <v-container class="overflow-auto h-screen">
        <v-row>
            <v-col cols="12">
                <v-btn color="primary" prepend-icon="mdi-arrow-left" @click="router.push({ name: RouterEnum.PROJECT })">
                    Back
                </v-btn>
            </v-col>
            <v-col>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" class="my-auto d-flex justify-end">
                <add-keep :project-id="projectId"></add-keep>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12" v-if="date">
                Filter By:
                <v-chip color="black" closable v-if="date" @click:close="date = ''" class="mx-3 pa-3">Date</v-chip>
            </v-col>
            <no-item v-if="Keeps.length == 0">
                No record has been added yet
            </no-item>
            <all-keeps :date="date"></all-keeps>
        </v-row>
    </v-container>
</template>
