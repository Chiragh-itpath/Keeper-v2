<script setup lang="ts">
import { reactive, ref, type Ref } from 'vue'
import { GlobalStore, ProjectStore } from '@/stores'
import { TextField } from '@/components/Custom'
import type { IAddProject } from '@/Models/ProjectModels'
import { storeToRefs } from 'pinia';

const visible: Ref<boolean> = ref(false)
const form = ref()
const { AddProject } = ProjectStore()
const { Loading } = storeToRefs(GlobalStore())
const addProject: IAddProject = reactive({
    title: '',
    description: '',
    tag: ''
})

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (valid) {
        await AddProject(addProject)
        close()
    }
}
const close = () => {
    form.value.reset()
    visible.value = false
}
</script>
<template>
    <v-btn color="primary" variant="elevated" prepend-icon="mdi-plus" @click="visible = true" class="w-sm-100"> 
        New Project
    </v-btn>
    <v-dialog transition="scale-transition" v-model="visible" width="700" @update:model-value="close">
        <v-card>
            <v-card-title class="text-center bg-primary">
                New Project
                <v-icon class="float-end" @click="close">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form @submit.prevent="submitHandler" ref="form" validate-on="submit">
                    <v-row>
                        <v-col cols="12" sm="8">
                            <text-field v-model="addProject.title" label="Title*" placeholder="Project Title" is-required
                                :max-limit="100" />
                        </v-col>
                        <v-col cols="12" sm="4">
                            <text-field v-model="addProject.tag" label="Tag" placeholder="Project Tag" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea v-model="addProject.description" label="Description" color="primary"></v-textarea>
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="120" class="mx-2 rounded-xl"
                    :loading="Loading" :disabled="Loading">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
