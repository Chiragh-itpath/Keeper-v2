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
    <v-container class="overflow-auto h-screen bg-blue-grey-lighten-5" fluid>
        <v-row class="mx-5">
            <v-col>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="3" sm="4" class="my-auto d-flex justify-end" v-if="route.name != RouterEnum.SHARED">
                <add-project />
            </v-col>
        </v-row>
        <v-row class="mx-5">
            <v-col cols="12" v-if="date || route.name == RouterEnum.PROJECT_BY_TAG">
                Filter By:
                <v-chip color="black" closable v-if="date" @click:close="date = ''" class="mx-3 pa-3">Date</v-chip>
                <v-chip color="black" closable v-if="route.name == RouterEnum.PROJECT_BY_TAG"
                    @click:close="router.push({ name: RouterEnum.PROJECT })">
                    Tag
                </v-chip>
            </v-col>
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
