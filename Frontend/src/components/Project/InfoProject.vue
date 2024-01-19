<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import type { IProject } from '@/Models/ProjectModels'
import moment from 'moment'

defineProps<{
    project: IProject
}>()

const visible: Ref<boolean> = ref(false)
const emit = defineEmits<{
    (e: 'close'): void
}>()
watch(visible, () => {
    if (!visible.value) emit('close')
})
</script>
<template>
    <v-dialog v-model="visible" max-width="700" v-if="project">
        <template v-slot:activator="{ props }">
            <v-list-item v-bind="props">
                <v-icon>mdi-information-outline</v-icon>
                <span class="mx-3">Info</span>
            </v-list-item>
        </template>
        <v-card class="overflow-auto">
            <v-card-title class="text-center bg-primary">
                Project Details
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="mb-4">
                <v-row>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Title:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">{{ project.title }}</v-col>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Description:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3 description">
                        {{ project.description }}
                        <span v-if="!project.description" class="text-grey">No Description provided</span>
                    </v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Tag:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        <v-chip color="primary" v-if="project.tag">{{ project.tag }}</v-chip>
                        <span v-else>-</span>
                    </v-col>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Members:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ project.users.length }}
                    </v-col>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Owner:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ project.createdBy }}
                    </v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Created On:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ moment(project.createdOn).format("DD/MM/YYYY, hh:mm a") }}
                    </v-col>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">
                        Last Modified By:
                    </v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ project.updatedBy ?? '-' }}
                    </v-col>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Last Modified:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        <span v-if="project.updatedOn">
                            {{ moment(project.updatedOn).format("DD/MM/YYYY, hh:mm a") }}
                        </span>
                        <span v-else>-</span>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<style scoped>
.description {
    background-color: #ECEFF1;
    height: 150px;
    overflow: auto;
    text-overflow: ellipsis;
}
</style>