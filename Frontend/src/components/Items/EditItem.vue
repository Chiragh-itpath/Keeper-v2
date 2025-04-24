<script setup lang="ts">
import { reactive, ref, watch, type Ref, computed } from 'vue'
import { ItemStore } from '@/stores'
import { TextField, TextEditor, SearchableList, ConfirmDialog } from '@/components/Custom/' // Import ConfirmDialog
import type { IEditItem, IItem } from '@/Models/ItemModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'
import type { IKeep, IKeepMembers } from '@/Models/KeepModels'
import { fileRule } from '@/data/ValidationRules'
import { ItemType } from '@/Models/enum'
import { TypeList, ImagePreview } from '@/components/Items'
import { useDisplay } from 'vuetify'
import { useTheme } from '@/composable/useTheme'

type ListItem = { title: string, subtitle?: string, value: string }
const { item, keep, project, clientList } = defineProps<{
    item: IItem
    project: IProject,
    keep: IKeep,
    clientList: ListItem[]
}>()
const visible: Ref<boolean> = ref(false)
const confirmDialogVisible = ref(false)
const { dark } = useTheme()
const hasUnsavedChanges = computed(() => {
    const normalizeValue = (value: any) => value?.trim() || ''

    return normalizeValue(editItem.title) !== normalizeValue(item.title) ||
        normalizeValue(editItem.description) !== normalizeValue(item.description) ||
        normalizeValue(editItem.number?.toString()) !== normalizeValue(item.number?.toString()) ||
        normalizeValue(editItem.url) !== normalizeValue(item.url) ||
        normalizeValue(editItem.to) !== normalizeValue(item.to) ||
        normalizeValue(editItem.discussedBy) !== normalizeValue(item.discussedBy)
})

const resetForm = () => {
    const { files, ...rest } = item
    Object.assign(editItem, {
        id: rest.id,
        title: rest.title,
        description: rest.description,
        url: rest.url,
        keepId: keep.id,
        number: rest.number,
        type: rest.type,
        to: rest.to,
        discussedBy: rest.discussedBy,
    })
}

const closeHandler = () => {
    if (hasUnsavedChanges.value) {
        confirmDialogVisible.value = true
    } else {
        resetForm()
        visible.value = false
        emits('update:modelValue', false)
        emits('close')
    }
}

const confirmClose = () => {
    resetForm()
    confirmDialogVisible.value = false
    visible.value = false
    emits('update:modelValue', false)
    emits('close')
}

// Add a watch to reset form when dialog opens
watch(() => visible.value, (newVal) => {
    if (newVal) {
        resetForm()
    } else {
        closeHandler()
    }
})
const fullScreen = ref(false)
const display = useDisplay()
const maxWidth = computed(() => {
    if (fullScreen.value || display.smAndDown.value) {
        return '100%';
    }
    if (display.mdAndDown.value) {
        return '700px';
    }
    return '1000px';
})
const cardMaxHeight = computed(() => fullScreen.value ? 'auto' : '550px')
const editorHeight = computed(() => fullScreen.value ? 400 : 150)

const DeletedClients = computed(() => {
    return item.to != null ? item.to
        .split(',')
        .filter(x => !clientList.map(c => c.value).includes(x))
        .map((x): ListItem => {
            return {
                title: x,
                value: x
            }
        }) : []
})
const form = ref()
const editItem = reactive<IEditItem>({
    id: item.id,
    title: item.title,
    description: item.description,
    url: item.url,
    keepId: keep.id,
    number: item.number,
    type: item.type,
    to: item.to,
    discussedBy: item.discussedBy,
})
const { EditItem } = ItemStore()
const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const savedItem = await EditItem(editItem)
    if (savedItem) {
        editItem.files = []
        const { files, ...savedRest } = savedItem
        emits('update:item', savedItem)
        Object.assign(item, { files, ...savedRest })
        Object.assign(editItem, savedRest)
        visible.value = false
        emits('update:modelValue', false)
        emits('close')
    }
}
const users = computed(() => {
    const mapUser = (u: IProjectMembers | IKeepMembers) => ({
        title: u.invitedUser.userName,
        subtitle: u.invitedUser.email,
        value: u.invitedUser.userName
    });
    return [
        ...project.users.filter(u => u.isAccepted || !u.shareId),
        ...keep.users.filter(u => u.isAccepted)
    ].map(mapUser);
})
const editItemUsers = computed(() => users.value.map(x => ({ ...x, value: x.title })))

const downloadFile = (path: string) => {
    window.open(path, '_blank')
}
const emits = defineEmits<{
    (e: 'close'): void,
    (e: 'update:modelValue', value: boolean): void,
    (e: 'update:item', item: IItem): void
}>()
</script>

<template>
    <v-dialog v-model="visible" persistent :max-width="maxWidth" :fullscreen="fullScreen"
        @update:model-value="() => emits('update:modelValue', visible)">
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card class="position-relative">
            <v-card-title class="bg-primary text-center position-sticky">
                Update Item
                <div class="float-end d-flex align-center gap-2">
                    <v-icon color="white" :icon="fullScreen ? 'mdi-fullscreen-exit' : 'mdi-fullscreen'"
                        class="cursor-pointer" @click="() => (fullScreen = !fullScreen)">
                    </v-icon>
                    <v-icon @click="closeHandler">mdi-close</v-icon>
                </div>
            </v-card-title>
            <v-card-text class="px-0">
                <v-card elevation="0" class="mx-5 px-2" :class="{ 'overflow-y-auto': !fullScreen }"
                    :style="{ 'max-height': cardMaxHeight }">
                    <v-form ref="form" @submit.prevent>
                        <v-row>
                            <v-col>
                                <v-select :items="TypeList" label="Type" color="primary" v-model="editItem.type"
                                    density="comfortable">

                                    <template v-slot:item="{ item, props }">
                                        <v-list-item v-bind="props" :title="item.title" density="compact"></v-list-item>
                                    </template>
                                </v-select>
                            </v-col>
                            <v-col v-if="editItem.type == ItemType.TICKET || editItem.type == ItemType.PR">
                                <text-field label="Number*" placeholder="Ticker | PR number" is-number
                                    v-model="editItem.number" />
                            </v-col>
                            <v-col cols="12" md="6">
                                <text-field label="Item Name*" placeholder="Item title" is-required
                                    v-model="editItem.title" :max-limit="50" counter />
                            </v-col>
                            <v-col cols="12" v-if="editItem.type == ItemType.TICKET || editItem.type == ItemType.PR">
                                <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="editItem.url"
                                    :max-limit="200" icon="mdi-link-box-variant-outline" />
                            </v-col>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="[...clientList, ...DeletedClients]" label="Discuss With"
                                    v-model="editItem.to" multiple>
                                </searchable-list>
                            </v-col>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="editItemUsers" label="Discuss By"
                                    v-model="editItem.discussedBy">
                                </searchable-list>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12">
                                <text-editor v-model="editItem.description" :height="editorHeight" />
                            </v-col>
                            <v-col cols="12">
                                <v-file-input color="primary" v-model="editItem.files" label="Select Files"
                                    prepend-inner-icon="mdi-paperclip" prepend-icon="" show-size chips multiple
                                    :rules="[fileRule]" />
                            </v-col>
                            <v-col cols="12">
                                <template v-if="item.files && item.files.length > 0">
                                    <div class="mt-3">Files:</div>
                                    <v-row class="mt-2">
                                        <v-col v-for="(file, index) in item.files" :key="index" cols="auto">
                                            <v-card max-width="200" color="primary" variant="tonal"
                                                class="d-flex justify-center align-center pa-3">
                                                <v-tooltip location="top">
                                                    <template v-slot:activator="{ props }">
                                                        <span class="text-truncate"
                                                            :class="dark ? 'text-white' : 'text-black'" v-bind="props">
                                                            {{ file.fileName }}
                                                        </span>
                                                    </template>
                                                    {{ file.fileName }}
                                                </v-tooltip>
                                                <image-preview v-if="file.isImage" v-slot="{ activator }"
                                                    :image-url="file.fileUrl">
                                                    <v-btn icon="mdi-eye" class="text-primary ms-2" density="compact"
                                                        variant="flat" v-bind="activator" />
                                                </image-preview>
                                                <v-btn icon="mdi-download" class="text-primary" density="compact"
                                                    variant="flat" @click="() => downloadFile(file.fileUrl)" />
                                            </v-card>
                                        </v-col>
                                    </v-row>
                                </template>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="130"
                    class="mx-2 rounded-xl">
                    Update
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

    <!-- ConfirmDialog for unsaved changes -->
    <confirm-dialog v-model="confirmDialogVisible" text="Confirm Close"
        description="You have unsaved changes. Are you sure you want to close?" @yes="confirmClose"
        @cancel="confirmDialogVisible = false" />
</template>
