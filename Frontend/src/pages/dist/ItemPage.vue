<script setup lang="ts">
import { computed, ref, onMounted, type Ref } from 'vue'
import { useRoute } from 'vue-router'
import { AddItem, AllItems } from '@/components/Items'
import { DatePicker, NoItem } from '@/components/Custom'
import { ItemStore, ProjectStore, KeepStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IKeep } from '@/Models/KeepModels'
import type { IProject } from '@/Models/ProjectModels'

const route = useRoute()
const { GetAllItems } = ItemStore()
const { Items } = storeToRefs(ItemStore())
const loading: Ref<boolean> = ref(false)
const date = ref(null)
const project: Ref<IProject | undefined> = ref()
const keep: Ref<IKeep | undefined> = ref()

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
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    keep.value = await KeepStore().getSingleKeep(keepId.value)
    loading.value = false
})
</script>
<template>
    <v-container class="px-10 pt-5" fluid>
        <v-row v-if="loading">
            <v-col v-for=" i  in  4" :key="i" cols="12" md="6">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project && keep">
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
        <v-row v-if="!loading && project && keep">
            <all-items :items="Items" :date="date"></all-items>
        </v-row>
        <v-row v-if="!loading && (!project || !keep)">
            <no-item :title="!project ? 'No Project found with this id' : 'No Keep found with this id'"></no-item>
        </v-row>
    </v-container>
</template>
