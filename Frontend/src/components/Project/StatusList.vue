<script setup lang="ts">
import { computed, ref } from 'vue'
import { type Status } from '@/Models/ProjectSettings'
import { useTheme } from '@/composable/useTheme'
import { TextField } from '@/components/Custom'

type EditableStatus = Status & { isEditing: boolean }

const { dark } = useTheme()
const disabled = computed(() => newStaus.value != undefined || status.value.some(x => x.isEditing))
const status = ref<EditableStatus[]>([])
const newStaus = ref<string>()
const updatingStaus = ref<string>()
const error = ref<string>()

const newClicked = () => {
    newStaus.value = ''
}
const saveClicked = async () => {
    if (!newStaus.value || newStaus.value.trim() == '') {
        error.value = 'This field is required '
        return
    }
    if (status.value.some(x => x.title == newStaus.value)) {
        error.value = 'Status already exist'
        return
    }
    status.value.unshift({
        title: newStaus.value,
        id: '',
        projectId: '',
        isEditing: false
    })
    newStaus.value = error.value = undefined;
}
const cancelClicked = () => {
    error.value = newStaus.value = undefined
}
const deleteClicked = (index: number) => {
    status.value.splice(index, 1)
}
</script>
<template>
    <v-form @submit.prevent>
        <v-container fluid>
            <v-row>
                <v-col cols="12" class="d-flex justify-end">
                    <v-btn text="Add Status" prepend-icon="mdi-plus" color="primary" class="rounded" :disabled="disabled"
                        @click="newClicked" />
                </v-col>
            </v-row>
            <v-row class="px-4 bg-primary mt-10">
                <v-col>Title</v-col>
                <v-col cols="auto" class="text-end">Actions</v-col>
            </v-row>
            <v-row class="border" v-if="newStaus != undefined" :class="[{ 'bg-white': !dark }]">
                <v-col>
                    <text-field v-model="newStaus" density="compact" :error-messages="error"></text-field>
                </v-col>
                <v-col cols="auto" class="d-flex ga-2 flex-wrap ">
                    <v-btn density="comfortable" icon="mdi-check" color="primary" size="small" @click="saveClicked" />
                    <v-btn density="comfortable" icon="mdi-close" color="red" size="small" @click="cancelClicked" />
                </v-col>
            </v-row>
            <v-row class="border" v-else-if="!status.length" :class="[{ 'bg-white': !dark }]">
                <v-col class="text-grey text-center text-capitalize"> no status added</v-col>
            </v-row>
            <template v-for="(item, index) in status" :key="index">
                <v-row class="border border-t-0 ps-4" :class="[{ 'bg-white': !dark }]">
                    <v-col>
                        <text-field v-model="updatingStaus" v-if="item.isEditing">
                        </text-field>
                        <template v-else>
                            {{ item.title }}
                        </template>
                    </v-col>
                    <v-col cols="auto" class="d-flex ga-2 flex-wrap align-center">
                        <template v-if="!item.isEditing">
                            <v-btn density="comfortable" icon="mdi-pencil" color="primary" size="small"
                                @click="item.isEditing = true; updatingStaus = item.title" />
                            <v-btn density="comfortable" icon="mdi-delete" color="red" size="small"
                                @click="deleteClicked(index)" />
                        </template>
                        <template v-else>
                            <v-btn density="comfortable" icon="mdi-check" color="primary" size="small"
                                @click="item.isEditing = false; item.title = updatingStaus ?? ''" />
                            <v-btn density="comfortable" icon="mdi-close" color="red" size="small"
                                @click="item.isEditing = false" />
                        </template>
                    </v-col>
                </v-row>
            </template>
        </v-container>
    </v-form>
</template>