<script lang="ts" setup>
import { reactive, ref, type Ref, watch } from 'vue'
import { storeToRefs } from "pinia";
import { GlobalStore, ProjectStore } from '@/stores'
import { TextField } from '@/components/Custom'
import type { IEditProject, IProject } from '@/Models/ProjectModels'

const props = withDefaults(defineProps<{
    modelValue: boolean,
    project?: IProject
}>(), {
    modelValue: false
})

const { UpdateProject } = ProjectStore()
const { Loading } = storeToRefs(GlobalStore())
const editProject: IEditProject = reactive({
    id: '',
    title: '',
    description: '',
    tag: ''
})

const visible: Ref<boolean> = ref(props.modelValue)
const form = ref()

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (valid) {
        await UpdateProject(editProject)
        visible.value = false
    }
}
watch(props, () => {
    visible.value = props.modelValue
    editProject.id = props.project!.id
    editProject.title = props.project!.title
    editProject.description = props.project!.description
    editProject.tag = props.project!.tag
})
watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()

</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="700">
        <v-card>
            <v-card-title class="text-center bg-primary">
                Edit Project
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form @submit.prevent="submitHandler" ref="form" validate-on="submit">
                    <v-row>
                        <v-col cols="12" sm="8">
                            <text-field v-model="editProject.title" placeholder="Project Title" label="Title*" is-required
                                :max-limit="100" />
                        </v-col>
                        <v-col cols="12" sm="4">
                            <text-field v-model="editProject.tag" label="Tag" placeholder="Project Tag" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea v-model="editProject.description" label="Description" color="primary" />
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="120" class="mx-2 rounded-xl"
                    :loading="Loading" :disabled="Loading">
                    Update
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
