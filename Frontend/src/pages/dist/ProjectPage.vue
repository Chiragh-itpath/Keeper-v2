<script setup lang="ts">
import { ref } from 'vue'
import AddProject from '@/components/Project/AddProject.vue'
import AllProjects from '@/components/Project/AllProjects.vue'
import DatePicker from '@/components/Custom/DatePicker.vue'

const date = ref('')

</script>
<template>
    <v-container class="overflow-auto h-screen " fluid>
        <v-row class="mx-5">
            <v-col>
                <date-picker label="Select a Date" v-model="date"></date-picker>
            </v-col>
            <v-col cols="12" lg="2" sm="3" class="my-auto d-flex justify-end">
                <add-project />
            </v-col>
        </v-row>
        <v-row class="mx-5">
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
