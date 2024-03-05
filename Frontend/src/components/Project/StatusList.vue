<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { type IStatus } from '@/Models/ProjectSettings'
import { useTheme } from '@/composable/useTheme'
import { TextField, DeletePrompt } from '@/components/Custom'
import { ProjectSettingsService } from '@/Services/ProjectSettings'
type EditableStatus = IStatus & { isEditing: boolean }

const { projectId, isOwner } = defineProps<{
    projectId: string,
    isOwner: boolean
}>()
const { dark } = useTheme()
const disabled = computed(() => newStaus.value != undefined || statusList.value.some(x => x.isEditing))
const statusList = ref<EditableStatus[]>([])
const newStaus = ref<string>()
const updatingStaus = ref<string>()
const error = ref<string>()
const isEditing = computed(() => statusList.value.some(x => x.isEditing))
const projectSettings = new ProjectSettingsService()
onMounted(async () => {
    const items = (await projectSettings.GetAllStatus(projectId)) ?? []
    statusList.value = items.map(x => {
        return { ...x, isEditing: false }
    })
})

const saveClicked = async () => {
    if (!newStaus.value || newStaus.value.trim() == '') {
        error.value = 'This field is required'
        return
    }
    if (statusList.value.some(x => x.title.toLowerCase() == newStaus.value!.toLowerCase())) {
        error.value = 'Status already exist'
        return
    }
    const response = await projectSettings.AddStatus({
        title: newStaus.value.trim(),
        projectId: projectId
    })
    if (!response) return
    statusList.value.push({ ...response, isEditing: false })
    newStaus.value = error.value = undefined;
}

const cancelClicked = (item?: EditableStatus) => {
    if (item) item.isEditing = false
    error.value = newStaus.value = undefined
}

const editClicked = (item: EditableStatus) => {
    if (isEditing.value) return
    item.isEditing = true
    updatingStaus.value = item.title
}

const updateClicked = async (item: IStatus, index: number) => {
    if (!updatingStaus.value || updatingStaus.value.trim() == '') {
        error.value = 'This field is required'
        return
    }
    const matchingIndex = statusList.value.findIndex(x => x.title.toLowerCase() == updatingStaus.value!.toLowerCase())
    if (matchingIndex != -1 && matchingIndex != index) {
        error.value = 'Status already exist'
        return
    }
    const res = await projectSettings.UpdateStatus({
        id: item.id,
        projectId: item.projectId,
        title: updatingStaus.value.trim() ?? ''
    })
    if (res) {
        statusList.value.splice(index, 1, { ...res, isEditing: false })
        updatingStaus.value = undefined
    }
}

const deleteClicked = async (id: string, index: number) => {
    if (isEditing.value) return
    if (await projectSettings.DeleteStatus(id))
        statusList.value.splice(index, 1)
}
</script>
<template>
    <v-form @submit.prevent>
        <v-container fluid>
            <v-row v-if="isOwner">
                <v-col cols="12" class="d-flex justify-end">
                    <v-btn text="Add Status" prepend-icon="mdi-plus" color="primary" class="rounded" :disabled="disabled"
                        @click="newStaus = ''" />
                </v-col>
            </v-row>
            <v-row class="px-4 bg-primary mt-10">
                <v-col>Title</v-col>
                <v-col cols="2" lg="1" class="text-center" v-if="isOwner">Actions</v-col>
            </v-row>
            <v-row class="border" v-if="newStaus != undefined" :class="[{ 'bg-white': !dark }]">
                <v-col>
                    <text-field v-model="newStaus" density="compact" :error-messages="error"></text-field>
                </v-col>
                <v-col cols="2" lg="1" class="d-flex ga-2 flex-wrap align-center justify-center">
                    <v-btn density="comfortable" icon="mdi-check" color="primary" size="small" @click="saveClicked" />
                    <v-btn density="comfortable" icon="mdi-close" color="red" size="small" @click="cancelClicked" />
                </v-col>
            </v-row>
            <v-row class="border" v-else-if="!statusList.length" :class="[{ 'bg-white': !dark }]">
                <v-col class="text-grey text-center text-capitalize"> no status added</v-col>
            </v-row>
            <template v-for="(item, index) in statusList" :key="index">
                <v-row class="border border-t-0 ps-4" :class="[{ 'bg-white': !dark }]">
                    <v-col>
                        <text-field v-model="updatingStaus" density="compact" v-if="item.isEditing"
                            :error-messages="error" />
                        <template v-else>
                            {{ item.title }}
                        </template>
                    </v-col>
                    <v-col cols="2" lg="1" class="d-flex ga-2 flex-wrap align-center justify-center" v-if="isOwner">
                        <template v-if="!item.isSystem">
                            <template v-if="item.isEditing">
                                <v-btn density="comfortable" icon="mdi-check" color="primary" size="small"
                                    @click="updateClicked(item, index)" />
                                <v-btn density="comfortable" icon="mdi-close" color="red" size="small"
                                    @click="cancelClicked(item)" />
                            </template>
                            <template v-else>
                                <v-btn density="comfortable" icon="mdi-pencil" color="primary" size="small"
                                    @click="editClicked(item)" :disabled="item.isSystem || isEditing" />
                                <delete-prompt v-slot="{ props }" @click:yes="deleteClicked(item.id, index)"
                                    title="Delete Status?">
                                    <v-btn density="comfortable" icon="mdi-delete" color="red" size="small" v-bind="props"
                                        :disabled="item.isSystem || isEditing" />
                                </delete-prompt>
                            </template>
                        </template>
                    </v-col>
                </v-row>
            </template>
        </v-container>
    </v-form>
</template>