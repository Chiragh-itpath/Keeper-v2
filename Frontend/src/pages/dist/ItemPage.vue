<script setup lang="ts">
import { computed, ref, onMounted, type Ref } from 'vue'
import { useRoute } from 'vue-router'
import { AddItem, AllItems } from '@/components/Items'
import { DatePicker } from '@/components/Custom'
import { ItemStore, ContactStore } from '@/stores'
import { storeToRefs } from 'pinia'

const route = useRoute()
const { GetAllItems } = ItemStore()
const { Items } = storeToRefs(ItemStore())
const loading: Ref<boolean> = ref(false)
const date = ref(null)

const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const keepId = computed(() => {
    const id = route.params.keepId
    return Array.isArray(id) ? id.join('') : id
})

onMounted(async () => {
    loading.value = true
    await GetAllItems(keepId.value)
    await ContactStore().GetContacts()
    loading.value = false
})
</script>
<template>
    <v-container class="px-10 pt-5" fluid>
        <v-row v-if="loading">
            <v-col v-for=" i  in  4" :key="i" cols="6">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading">
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
            <v-col class="my-auto d-flex justify-end">
                <add-item :keep-id="keepId"></add-item>
            </v-col>
        </v-row>
        <v-row v-if="!loading">
            <all-items :items="Items" :date="date"></all-items>
        </v-row>
    </v-container>
</template>
