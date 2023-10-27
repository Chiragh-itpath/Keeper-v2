<script setup lang="ts">
import { ref, type Ref, watch } from 'vue'
import { ProjectStore, UserStore } from '@/stores'
import { dateHelper } from '@/Services/HelperFunction'
import type { IProject } from '@/Models/ProjectModels'

const props = withDefaults(defineProps<{
    id: string,
    modelValue: boolean
}>(), {
    id: '',
    modelValue: false
})

const { SingleProject } = ProjectStore()
const { User, myProfile } = UserStore()
const project: Ref<IProject | undefined> = ref()
const visible: Ref<boolean> = ref(false)

watch(props, async () => {
    visible.value = props.modelValue
    if (visible.value) {
        project.value = SingleProject(props.id)
        if (User.id == '') {
            await myProfile()
        }
    }
})
watch(visible, () => {
    if (!visible.value) {
        emits('update:modelValue', visible.value)
    }
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()
</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="700">
        <v-card class="overflow-auto">
            <v-card-title class="text-center bg-primary">
                Project Details
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="mb-4">
                <v-row>
                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Title:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">{{ project?.title }}</v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Description:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3 description">{{ project?.description }}</v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Tag:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3"><v-chip color="primary">{{ project?.tag }}</v-chip></v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Owner:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ project?.createdBy == User.email ? 'me' : project?.createdBy }}
                    </v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Created On:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">{{ dateHelper(project?.createdOn) }}</v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">
                        Last Modified By:
                    </v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">
                        {{ project?.createdBy == User.email ? 'me' : project?.createdBy }}
                    </v-col>

                    <v-col cols="12" sm="4" class="text-grey pb-0 pb-sm-3">Last Modified:</v-col>
                    <v-col cols="12" sm="8" class="pb-0 pb-sm-3">{{ dateHelper(project?.updatedOn) }}</v-col>
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