<script setup lang="ts">
import { computed, ref, watch, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { AddItem, AllItems } from '@/components/Items'
import { DatePicker } from '@/components/Custom'
import { ItemStore, ContactStore } from '@/stores'
import { storeToRefs } from 'pinia'

const route = useRoute()
const { Items } = storeToRefs(ItemStore())
const { FilterByDate } = ItemStore()

const date = ref()

const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const keepId = computed(() => {
    const id = route.params.keepId
    return Array.isArray(id) ? id.join('') : id
})

watch(date, () => {
    FilterByDate(date.value)
})
onMounted(async () => {
    await ContactStore().GetContacts()
})
</script>
<template>
    <v-container class="overflow-auto" fluid>

        <v-row class="mx-5">
            <v-col cols="12">
                <v-breadcrumbs divider="/" :items="[
                    {
                        title: 'Projects',
                        disabled: false,
                        to: '/Project'
                    },
                    {
                        title: 'Keeps',
                        disabled: false,
                        to: `/Project/${projectId}`
                    },
                    {
                        title: 'Item',
                        disabled: true
                    }
                ]"></v-breadcrumbs>
            </v-col>
            <v-col>
                <date-picker label="Select a date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" md="3" sm="3" class="my-auto d-flex justify-end">
                <add-item :keep-id="keepId"></add-item>
            </v-col>
        </v-row>
        <v-row class="mx-5">
            <v-col cols="12" v-if="date">
                Filter By:
                <v-chip color="black" closable v-if="date" @click:close="date = ''" class="mx-3 pa-3">Date</v-chip>
            </v-col>
        </v-row>
        <v-row class="mx-5">
            <all-items></all-items>
        </v-row>
        <no-item v-if="Items.length == 0">
            No record has been added yet
        </no-item>
    </v-container>
</template>
