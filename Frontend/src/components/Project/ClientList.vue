<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { TextField, DeletePrompt } from "@/components/Custom"
import { useTheme } from '@/composable/useTheme'
import { type IClient } from "@/Models/ProjectSettings"
import { ProjectSettingsService } from '@/Services/ProjectSettings'

type EditableClient = IClient & { isEditing: boolean }

const { projectId } = defineProps<{
    projectId: string,
    isOwner: boolean
}>()
const { dark } = useTheme()
const disabled = computed(() => newClient.value != undefined || clientList.value.some(x => x.isEditing))
const clientList = ref<EditableClient[]>([])
const newClient = ref<string>()
const updatingClient = ref<string>()
const error = ref<string>()
const projectSettings = new ProjectSettingsService()
const isEditing = computed(() => clientList.value.some(x => x.isEditing))

onMounted(async () => {
    const clients = await projectSettings.GetAllClient(projectId)
    if (clients) {
        clientList.value = clients.map(x => {
            return { ...x, isEditing: false }
        })
    }
})

const cancelClicked = (item?: EditableClient) => {
    if (item) item.isEditing = false
    newClient.value = error.value = undefined
}

const saveClicked = async () => {
    if (!newClient.value || newClient.value.trim() == '') {
        error.value = 'This field is required'
        return
    }
    if (clientList.value.some(x => x.name == newClient.value!.trim())) {
        error.value = 'Client already exist'
        return
    }
    const client = await projectSettings.AddClient({
        name: newClient.value.trim(),
        projectId
    })
    if (client) {
        clientList.value.push({ ...client, isEditing: false })
        newClient.value = error.value = undefined
    }
}

const editClicked = async (item: EditableClient) => {
    if (clientList.value.some(x => x.isEditing)) return
    item.isEditing = true;
    updatingClient.value = item.name
}

const updateClicked = async (item: EditableClient, index: number) => {
    if (!updatingClient.value || updatingClient.value.trim() == '') {
        error.value = 'This field is required'
        return
    }
    const matchingIndex = clientList.value.findIndex(x => x.name.toLowerCase() == updatingClient.value!.toLowerCase())
    if (matchingIndex != index && matchingIndex != -1) {
        error.value = 'Client name already exist'
        return
    }
    const res = await projectSettings.UpdateClient({
        id: item.id,
        projectId,
        name: updatingClient.value.trim()
    })
    if (res) {
        clientList.value.splice(index, 1, { ...res, isEditing: false })
    }
}

const deleteClicked = async (id: string, index: number) => {
    if (isEditing.value) return
    if (await projectSettings.DeleteClient(id)) {
        clientList.value.splice(index, 1)
    }
}
</script>
<template>
    <v-form @submit.prevent>
        <v-container fluid>
            <v-row>
                <v-col cols="12" class="d-flex justify-end">
                    <v-btn text="Add Client" prepend-icon="mdi-plus" color="primary" class="rounded" :disabled="disabled"
                        @click="newClient = ''" />
                </v-col>
            </v-row>
            <v-row class="mt-10 bg-primary px-4">
                <v-col> Client name</v-col>
                <v-col cols="auto">Actions</v-col>
            </v-row>
            <v-row class="border" :class="[{ 'bg-white': !dark }]" v-if="newClient != undefined">
                <v-col>
                    <text-field label="Client name" density="compact" v-model="newClient" :error-messages="error" />
                </v-col>
                <v-col cols="auto" class="d-flex align-center ga-2">
                    <v-btn icon="mdi-check" density="comfortable" color="primary" size="small" @click="saveClicked" />
                    <v-btn icon="mdi-close" density="comfortable" color="red" size="small" @click="cancelClicked" />
                </v-col>
            </v-row>
            <v-row class="border" v-else-if="!clientList.length" :class="[{ 'bg-white': !dark }]">
                <v-col class="text-center text-grey text-capitalize">
                    no client added
                </v-col>
            </v-row>
            <template v-for="(client, index) in clientList" :key="index">
                <v-row class="border border-t-0 px-4" :class="[{ 'bg-white': !dark }]">
                    <v-col>
                        <text-field v-if="client.isEditing" v-model="updatingClient" density="compact" />
                        <template v-else>
                            {{ client.name }}
                        </template>
                    </v-col>
                    <v-col cols="auto" class="d-flex ga-2">
                        <template v-if="client.isEditing">
                            <v-btn density="comfortable" size="small" icon="mdi-check" color="primary"
                                @click="updateClicked(client, index)" />
                            <v-btn density="comfortable" size="small" icon="mdi-close" color="red"
                                @click="updatingClient = undefined; client.isEditing = false" />
                        </template>
                        <template v-else>
                            <v-btn density="comfortable" size="small" icon="mdi-pencil" color="primary"
                                @click="editClicked(client)" :disabled="isEditing" />
                            <delete-prompt v-slot="{ props }" title="Delete Client"
                                @click:yes="deleteClicked(client.id, index)">
                                <v-btn density="comfortable" size="small" icon="mdi-delete" color="red" v-bind="props"
                                    :disabled="isEditing" />
                            </delete-prompt>
                        </template>
                    </v-col>
                </v-row>
            </template>
        </v-container>
    </v-form>
</template>