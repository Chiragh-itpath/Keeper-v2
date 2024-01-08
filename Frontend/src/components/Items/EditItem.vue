<script setup lang="ts">
import { reactive, ref, watch, type Ref, computed } from 'vue'
import { ItemStore } from '@/stores'
import { TextField, TextEditor } from '@/components/Custom/'
import type { IEditItem, IItem } from '@/Models/ItemModels'
import type { ItemType } from '@/Models/types'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'

const props = withDefaults(defineProps<{
    modelValue: boolean,
    item: IItem
    project: IProject,
    keep: IKeep
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(false)
const form = ref()
const editItem: IEditItem = reactive({
    id: props.item.id,
    title: props.item.title,
    description: props.item.description,
    url: props.item.url,
    keepId: props.keep.id,
    number: props.item.number,
    type: props.item.type == 0 ? 'Ticket' : 'PR',
    to: props.item.to,
    discussedBy: props.item.discussedBy,
    files: null
})
const { EditItem } = ItemStore()
const itemType: Ref<ItemType> = ref('Ticket')

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (!valid) return
    editItem.type = itemType.value
    await EditItem(editItem)
    visible.value = false
}
const users = computed(() => {
    return [
        ...props.project.users.filter(u => u.isAccepted || !u.shareId).map(u => {
            return {
                title: u.invitedUser.userName,
                value: u.invitedUser.email
            }
        }),
        ...props.keep.users.filter(u => u.isAccepted).map(u => {
            return {
                title: u.invitedUser.userName,
                value: u.invitedUser.email
            }
        })
    ]
})
watch(visible, () => {
    if (!visible.value) {
        emits('close')
    }
})
const emits = defineEmits<{
    (e: 'close'): void
}>()
</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" close-on-back max-width="800">
        <template v-slot:activator="{ props }">
            <v-list-item role="button" v-bind="props">
                <v-icon>mdi-folder-edit-outline</v-icon>
                <span class="mx-3">Edit</span>
            </v-list-item>
        </template>
        <v-card class="position-relative">
            <v-card-title class="bg-primary text-center position-sticky">
                Update Item
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="px-0">
                <v-card max-height="450" elevation="0" class="mx-5 px-2 overflow-y-auto">
                    <v-form ref="form" @submit.prevent>
                        <v-row>
                            <v-col cols="6" lg="3" md="3" sm="6">
                                <v-select :items="['Ticket', 'PR']" label="Type" color="primary" v-model="itemType"
                                    density="comfortable" />
                            </v-col>
                            <v-col cols="6" lg="3" md="3" sm="6">
                                <text-field label="Number" placeholder="Ticker | PR number" is-number
                                    v-model="editItem.number" />
                            </v-col>
                            <v-col>
                                <text-field label="Item Name*" placeholder="Item title" is-required
                                    v-model="editItem.title" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12">
                                <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="editItem.url"
                                    :max-limit="200" icon="mdi-link-box-variant-outline" />
                            </v-col>
                            <v-col cols="12">
                                <text-editor v-model="editItem.description" />
                            </v-col>
                            <v-col cols="12">
                                <v-file-input color="primary" v-model="editItem.files" prepend-inner-icon="mdi-paperclip"
                                    prepend-icon="" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" sm="6">
                                <text-field label="Discuss With" placeholder="Client name" v-model="editItem.to" />
                            </v-col>
                            <v-col cols="12" sm="6">
                                <v-select label="Discuss By" color="primary" :items="users" v-model="editItem.discussedBy"
                                    clearable density="comfortable">
                                    <template v-slot:clear>
                                        <v-icon @click="editItem.discussedBy = undefined" color="primary">
                                            mdi-close-circle-outline
                                        </v-icon>
                                    </template>
                                    <template v-slot:item="{ props, item }">
                                        <v-list-item v-bind="props" :title="item.title" :subtitle="item.value"
                                            :value="item.value">
                                        </v-list-item>
                                    </template>
                                    <template v-slot:selection="{ item }">
                                        <v-chip color="primary" v-if="editItem.discussedBy">{{ item.value }}</v-chip>
                                    </template>
                                </v-select>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="130" class="mx-2 rounded-xl">
                    Update
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<style></style>
