<script setup lang="ts">
import { ref, watch, type Ref, reactive } from 'vue'
import { TextField, TextEditor, SearchableList } from '@/components/Custom/'
import { ItemStore } from '@/stores'
import type { IAddItem } from '@/Models/ItemModels'
import type { IKeep } from '@/Models/KeepModels'
import { fileRule } from '@/data/ValidationRules'
import { ItemType } from '@/Models/enum'
import { TypeList } from '@/components/Items'
import type { IStatus } from '@/Models/ProjectSettings'

type ListItem = { title: string, subtitle?: string, value: string }
const { keep, users, statusList } = defineProps<{
    keep: IKeep,
    users: ListItem[]
    clientList: ListItem[],
    statusList: IStatus[]
}>()
const visible: Ref<boolean> = ref(false)
const form = ref()
const validateOn: Ref<'submit' | 'input'> = ref('submit')
const { AddItem } = ItemStore()
const addItem = reactive<IAddItem>({
    title: '',
    description: '',
    type: ItemType.TICKET,
    url: '',
    keepId: keep.id,
    number: '',
    statusId: statusList[0].id
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
        addItem.description = ''
        addItem.keepId = keep.id
        addItem.type = ItemType.TICKET
        addItem.discussedBy = ''
    }
    validateOn.value = 'submit'
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
                                    v-model="addItem.title" :max-limit="100" counter />
                            </v-col>
                            <v-col cols="12" v-if="addItem.type == ItemType.TICKET || addItem.type == ItemType.PR">
                                <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="addItem.url"
                                    :max-limit="200" icon="mdi-link-box-variant-outline" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="clientList" label="Discuss With" v-model="addItem.to"
                                    multiple>   
                                </searchable-list>
                            </v-col>
                            <v-col cols="12" sm="6">
                                <searchable-list :search-items="users" label="Discuss By" v-model="addItem.discussedBy">
                                </searchable-list>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12">
                                <text-editor v-model="addItem.description"></text-editor>
                            </v-col>
                            <v-col cols="12">
                                <v-file-input color="primary" v-model="addItem.files" label="Select Files" multiple
                                    prepend-inner-icon="mdi-paperclip" prepend-icon="" show-size chips
                                    :rules="[fileRule]">
                                </v-file-input>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="130"
                    class="mx-2 rounded-xl">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
