<script setup lang="ts">
</script>
<template>
    <v-col cols="12">
        <v-card width="auto" elevation="8">
            <v-sheet class="d-flex justify-center align-center flex-column">
                <div class="bg-blue-grey-lighten-5 rounded-circle pa-10 mt-10">
                    <v-icon color="primary" size="150">mdi-notebook-remove</v-icon>
                </div>
                <p class="text-h5 mt-10 text-primary">
                    <slot></slot>
                </p>
                <p class="text-grey-darken-1 mt-2 text-center mb-10">
                    Please click on add button to insert new record
                </p>
            </v-sheet>
        </v-card>
    </v-col>
</template>
