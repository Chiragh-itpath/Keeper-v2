<script setup lang="ts">
import { reactive, ref, watch, type Ref, computed } from 'vue'
import { ItemStore } from '@/stores'
import { TextField, TextEditor, SearchableList } from '@/components/Custom/'
import type { IEditItem, IItem } from '@/Models/ItemModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'
import { fileRule } from '@/data/ValidationRules'
import { ItemType } from '@/Models/enum'
import { TypeList } from '@/components/Items'

const props = withDefaults(defineProps<{
    modelValue?: boolean,
    item: IItem
    project: IProject,
    keep: IKeep
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(false)
const form = ref()
const editItem = reactive<IEditItem>({
    id: props.item.id,
    title: props.item.title,
    description: props.item.description,
    url: props.item.url,
    keepId: props.keep.id,
    number: props.item.number,
    type: props.item.type,
    to: props.item.to,
    discussedBy: props.item.discussedBy
})
const { EditItem } = ItemStore()
const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const item = await EditItem(editItem)
    if (item) emits('update:item', item)
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
    (e: 'close'): void,
    (e: 'update:modelValue', value: boolean): void,
    (e: 'update:item', item: IItem): void
}>()
</script>
<template>
    <v-dialog v-model="visible" close-on-back max-width="850"
        @update:model-value="() => emits('update:modelValue', visible)">
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
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
                                    v-model="editItem.title" />
                            </v-col>
                            <v-col cols="12" v-if="editItem.type == ItemType.TICKET || editItem.type == ItemType.PR">
                                <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="editItem.url"
                                    :max-limit="200" icon="mdi-link-box-variant-outline" />
                            </v-col>
                            <v-col cols="12" sm="6">
                                <text-field label="Discuss With" placeholder="Client name" v-model="editItem.to" />
                            </v-col>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="users" label="Discuss By" v-model="editItem.discussedBy">
                                </searchable-list>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12">
                                <text-editor v-model="editItem.description" />
                            </v-col>
                            <v-col cols="12">
                                <v-file-input color="primary" v-model="editItem.files" label="Select Files"
                                    prepend-inner-icon="mdi-paperclip" prepend-icon="" show-size chips
                                    :rules="[fileRule]" />
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
