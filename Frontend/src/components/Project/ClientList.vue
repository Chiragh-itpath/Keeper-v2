<script setup lang="ts">
import { computed, ref } from 'vue'
import { TextField } from "@/components/Custom"
import { useTheme } from '@/composable/useTheme'
import { type Client } from "@/Models/ProjectSettings"

type EditableClient = Client & { isEditing: boolean }

const { dark } = useTheme()
const disabled = computed(() => newClient.value != undefined || clients.value.some(x => x.isEditing))
const clients = ref<EditableClient[]>([])
const newClient = ref<string>()
const updatingClient = ref<string>()
const error = ref<string>()

const newClicked = () => {
    newClient.value = ''
}
const cancelClicked = () => {
    newClient.value = error.value = undefined
}
const saveClicked = () => {
    if (!newClient.value || newClient.value.trim() == '') {
        error.value = 'This field is required'
        return
    }
    if (clients.value.some(x => x.fullname == newClient.value?.trim())) {
        error.value = 'Client already exist'
        return
    }
    clients.value.unshift({
        id: '',
        projectId: '',
        fullname: newClient.value,
        isEditing: false
    })
    newClient.value = error.value = undefined
}

</script>
<template>
    <v-form @submit.prevent>
        <v-container fluid>
            <v-row>
                <v-col cols="12" class="d-flex justify-end">
                    <v-btn text="Add Client" prepend-icon="mdi-plus" color="primary" class="rounded" :disabled="disabled"
                        @click="newClicked" />
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
            <v-row class="border" v-else-if="!clients.length" :class="[{ 'bg-white': !dark }]">
                <v-col class="text-center text-grey text-capitalize">
                    no client added
                </v-col>
            </v-row>
            <template v-for="(client, index) in clients" :key="index">
                <v-row class="border border-t-0 px-4">
                    <v-col>
                        <text-field v-if="client.isEditing" v-model="updatingClient" density="compact" />
                        <template v-else>
                            {{ client.fullname }}
                        </template>
                    </v-col>
                    <v-col cols="auto" class="d-flex ga-2">
                        <template v-if="client.isEditing">
                            <v-btn density="comfortable" size="small" icon="mdi-check" color="primary"
                                @click="client.isEditing = false; client.fullname = updatingClient!" />
                            <v-btn density="comfortable" size="small" icon="mdi-close" color="red"
                                @click="updatingClient = undefined; client.isEditing = false" />
                        </template>
                        <template v-else>
                            <v-btn density="comfortable" size="small" icon="mdi-pencil" color="primary"
                                @click="updatingClient = client.fullname; client.isEditing = true" />
                            <v-btn density="comfortable" size="small" icon="mdi-delete" color="red"
                                @click="clients.splice(index, 1)" />
                        </template>
                    </v-col>
                </v-row>
            </template>
        </v-container>
    </v-form>
</template>