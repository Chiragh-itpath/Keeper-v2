<script setup lang="ts">
import { ref } from 'vue'
import AddProject from '@/components/Project/AddProject.vue'
import AllProjects from '@/components/Project/AllProjects.vue'
import DatePicker from '@/components/Custom/DatePicker.vue'
import { useRoute, useRouter } from 'vue-router'
import { RouterEnum } from '@/Models/enum'

const route = useRoute();
const router = useRouter();
const date = ref('')
</script>
<template>
    <v-container class="px-10" fluid>
        <v-row>
            <v-col>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" xl="2" class="my-auto d-flex justify-end" v-if="route.name != RouterEnum.SHARED">
                <add-project />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12" v-if="date || route.name == RouterEnum.PROJECT_BY_TAG">
                Filter By:
                <v-chip class="mx-3 pa-3 bg-primary" closable v-if="date" @click:close="date = ''">Date</v-chip>
                <v-chip class="bg-primary" closable v-if="route.name == RouterEnum.PROJECT_BY_TAG"
                    @click:close="router.push({ name: RouterEnum.PROJECT })">
                    Tag
                </v-chip>
            </v-col>
        </v-row>
        <v-row>
            <all-projects :date="date"></all-projects>
        </v-row>
    </v-container>
</template>

<style scoped>
.v-list-item {
    min-height: 0 !important;
    margin: 5px 0 !important;
}
</style>
