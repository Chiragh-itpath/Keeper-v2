<script setup lang="ts">
import { ref, onMounted, type Ref } from 'vue'
import { AddProject, AllProject } from '@/components/Project'
import { DatePicker, TagSelector } from '@/components/Custom'
import { useRoute } from 'vue-router'
import { RouterEnum } from '@/Models/enum'
import { storeToRefs } from 'pinia'
import { ProjectStore } from '@/stores'

const { Projects, ProjectTags } = storeToRefs(ProjectStore())
const route = useRoute();
const date = ref('');
const loading: Ref<boolean> = ref(false)
const selectedTags: Ref<string[]> = ref([])
const filter = ref(0)

onMounted(async () => {
    loading.value = true
    await ProjectStore().GetProjects()
    loading.value = false
})
</script>
<template>
    <v-container class="px-10" fluid>
        <v-row class="mt-2">
            <v-col class="d-flex">
                <v-btn-toggle color="primary" density="comfortable" mandatory class="rounded-pill" v-model="filter">
                    <v-btn prepend-icon="mdi-account">
                        All
                    </v-btn>
                    <v-btn prepend-icon="mdi-account-multiple">Shared</v-btn>
                </v-btn-toggle>
                <v-divider vertical thickness="1" inset class="border-opacity-50 mx-3"></v-divider>
                <tag-selector :items="ProjectTags" v-model:selected="selectedTags"></tag-selector>
                <span class="mx-2"></span>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" xl="2" class="my-auto d-flex justify-end" v-if="route.name != RouterEnum.SHARED">
                <add-project />
            </v-col>
        </v-row>
        <v-row v-if="loading">
            <v-col v-for=" i  in  10 " :key="i" cols="3">
                <v-skeleton-loader type="text,image,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-else class="mt-10">
            <all-project :date="date" :projects="Projects" :tags="selectedTags" :is-shared="filter == 1"></all-project>
        </v-row>
    </v-container>
</template>

<style scoped>
.v-list-item {
    min-height: 0 !important;
    margin: 5px 0 !important;
}
</style>
