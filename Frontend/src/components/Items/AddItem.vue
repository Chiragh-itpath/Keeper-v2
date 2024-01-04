<script setup lang="ts">
import { ref, watch, type Ref, reactive, computed } from 'vue'
import { TextField, TextEditor } from '@/components/Custom/'
import { ItemStore } from '@/stores'
import type { ItemType } from '@/Models/types'
import type { IAddItem } from '@/Models/ItemModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'

const props = defineProps<{
    keep: IKeep,
    project: IProject
}>()

const visible: Ref<boolean> = ref(false)
const form = ref()

const addItem: IAddItem = reactive({
    title: '',
    description: '',
    url: '',
    keepId: '',
    number: '',
    type: 'Ticket',
    to: undefined,
    discussedBy: undefined,
    files: null
})
const { AddItem } = ItemStore()
const itemType: Ref<ItemType> = ref('Ticket')
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
    addItem.type = itemType.value
    await AddItem(addItem)
    visible.value = false
}

watch(visible, () => {
    if (visible.value) {
        addItem.keepId = props.keep.id
        addItem.title = addItem.description = addItem.url = addItem.number = ''
        addItem.to = addItem.discussedBy = undefined
        addItem.files = null
    }
})
</script>
<template>
    <v-btn color="primary" variant="elevated" prepend-icon="mdi-plus" @click="visible = true">
        New Item
    </v-btn>
    <v-dialog transition="scale-transition" v-model="visible" close-on-back max-width="900">
        <v-card max-height="600">
            <v-card-title class="bg-primary text-center position-sticky">
                New Item
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" @submit.prevent :validate-on="validateOn">
                    <v-row>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <v-select :items="['Ticket', 'PR']" label="Type" color="primary" v-model="itemType"
                                density="comfortable" />
                        </v-col>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <text-field label="Number*" placeholder="Ticker | PR number" is-required is-number
                                v-model="addItem.number" :max-limit="10" />
                        </v-col>
                        <v-col>
                            <text-field label="Item Name*" placeholder="Item title" is-required v-model="addItem.title" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="addItem.url"
                                :max-limit="200" icon="mdi-link-box-variant-outline" />
                        </v-col>
                        <v-col cols="12">
                            <text-editor v-model="addItem.description"></text-editor>
                        </v-col>
                        <v-col cols="12">
                            <v-file-input color="primary" v-model="addItem.files" prepend-inner-icon="mdi-paperclip"
                                prepend-icon="" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="6">
                            <text-field label="Discuss With" placeholder="Client name" v-model="addItem.to" />
                        </v-col>
                        <v-col cols="12" sm="6">
                            <v-select label="Discuss By" color="primary" :items="users" v-model="addItem.discussedBy"
                                clearable density="comfortable">
                                <template v-slot:clear>
                                    <v-icon @click="addItem.discussedBy = undefined" color="primary">
                                        mdi-close-circle-outline
                                    </v-icon>
                                </template>
                                <template v-slot:item="{ props, item }">
                                    <v-list-item v-bind="props" :title="item.title" :subtitle="item.value"
                                        :value="item.value">
                                    </v-list-item>
                                </template>
                                <template v-slot:selection="{ item }">
                                    <v-chip color="primary" v-if="addItem.discussedBy">{{ item.value }}</v-chip>
                                </template>
                            </v-select>
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="130" class="mx-2 rounded-xl">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
