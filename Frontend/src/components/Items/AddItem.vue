<script setup lang="ts">
import { ref, watch, type Ref, reactive, computed } from 'vue'
import { TextField, TextEditor, SearchableList } from '@/components/Custom/'
import { ItemStore } from '@/stores'
import type { IAddItem } from '@/Models/ItemModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'
import { fileRule } from '@/data/ValidationRules'
import { ItemType } from '@/Models/enum'
import { TypeList } from '@/components/Items'

const props = defineProps<{
    keep: IKeep,
    project: IProject
}>()

const visible: Ref<boolean> = ref(false)
const form = ref()

const addItem = reactive<IAddItem>({
    title: '',
    description: '',
    type: ItemType.TICKET,
    url: '',
    keepId: props.keep.id,
    number: ''
})
const { AddItem } = ItemStore()
const validateOn: Ref<'submit' | 'input'> = ref('submit')
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
const submitHandler = async (): Promise<void> => {
    validateOn.value = 'input'
    const { valid } = await form.value.validate()
    if (!valid) return
    await AddItem(addItem)
    visible.value = false
}

watch(visible, () => {
    if (!visible.value) {
        form.value.reset()
        addItem.keepId = props.keep.id
        addItem.type = ItemType.TICKET
    }
})
</script>
<template>
    <v-dialog v-model="visible" close-on-back max-width="850">
        <template v-slot:activator="{ props }">
            <v-btn color="primary" variant="elevated" prepend-icon="mdi-plus" v-bind="props" class="float-end">
                New Item
            </v-btn>
        </template>
        <v-card>
            <v-card-title class="bg-primary text-center position-sticky">
                New Item
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="px-0 ">
                <v-card max-height="500" class="mx-5 px-3 overflow-y-auto" elevation="0">
                    <v-form ref="form" @submit.prevent :validate-on="validateOn">
                        <v-row>
                            <v-col>
                                <v-select :items="TypeList" label="Type" color="primary" v-model="addItem.type"
                                    density="comfortable" hide-details>
                                    <template v-slot:item="{ item, props }">
                                        <v-list-item :title="item.title" :value="item.value" density="compact"
                                            v-bind="props"></v-list-item>
                                    </template>
                                </v-select>
                            </v-col>
                            <v-col v-if="addItem.type == ItemType.TICKET || addItem.type == ItemType.PR">
                                <text-field label="Number*" placeholder="Ticker | PR number" is-required is-number
                                    v-model="addItem.number" :max-limit="10" />
                            </v-col>
                            <v-col cols="12" md="6">
                                <text-field label="Item Name*" placeholder="Item title" is-required
                                    v-model="addItem.title" />
                            </v-col>
                            <v-col cols="12" v-if="addItem.type == ItemType.TICKET || addItem.type == ItemType.PR">
                                <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="addItem.url"
                                    :max-limit="200" icon="mdi-link-box-variant-outline" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" sm="6">
                                <text-field label="Discuss With" placeholder="Client name" v-model="addItem.to" />
                            </v-col>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="users" label="Discuss By"
                                    v-model="addItem.discussedBy"></searchable-list>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12">
                                <text-editor v-model="addItem.description"></text-editor>
                            </v-col>
                            <v-col cols="12">
                                <v-file-input color="primary" v-model="addItem.files" label="Select Files"
                                    prepend-inner-icon="mdi-paperclip" prepend-icon="" show-size chips :rules="[fileRule]">
                                </v-file-input>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="130" class="mx-2 rounded-xl">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
