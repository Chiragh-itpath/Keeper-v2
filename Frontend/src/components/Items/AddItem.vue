<script setup lang="ts">
import { ref, watch, type Ref, reactive } from 'vue'
import TextField from '@/components/Custom/TextField.vue'
import TextEditor from '@/components/Custom/TextEditor.vue'
import ContactDropDown from '../Contact/ContactDropDown.vue'
import type { ItemType } from '@/Models/types'
import { ItemStore } from '@/stores'
import type { IAddItem } from '@/Models/ItemModels'

const visible: Ref<boolean> = ref(false)
const form = ref()

const addItem: IAddItem = reactive({
    title: '',
    description: '',
    url: '',
    keepId: '',
    userId: '',
    number: '',
    type: 'Ticket',
    to: '',
    discussedBy: '',
    files: null
})
const { AddItem } = ItemStore()
const itemType: Ref<ItemType> = ref('Ticket')

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (!valid) return
    addItem.type = itemType.value
    await AddItem(addItem)
    visible.value = false
}

const props = withDefaults(defineProps<{
    keepId: string
}>(), {
})

watch(visible, () => {
    if (visible.value) {
        addItem.keepId = props.keepId
        addItem.title = addItem.description = addItem.url = addItem.discussedBy = addItem.to = ''
        addItem.number = ''
        addItem.files = null
    }
})

</script>
<template>
    <v-btn class="w-100" color="primary" variant="elevated" prepend-icon="mdi-plus" @click="visible = true">
        New Item
    </v-btn>
    <v-dialog transition="scale-transition" v-model="visible" close-on-back max-width="900">
        <v-card max-height="600">
            <v-card-title class="bg-primary text-center position-sticky">
                New Item
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" @submit.prevent validate-on="submit">
                    <v-row>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <v-select :items="['Ticket', 'PR']" label="Type" color="primary" v-model="itemType" />
                        </v-col>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <text-field label="Number" placeholder="Ticker | PR number" is-number v-model="addItem.number"
                                :max-limit="10" />
                        </v-col>
                        <v-col>
                            <text-field label="Item Name*" placeholder="Item title" is-required v-model="addItem.title" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="addItem.url"
                                icon="mdi-link-box-variant-outline" />
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
                            <contact-drop-down label="Discussed by" v-model:email="addItem.discussedBy"
                                :multiple="false"></contact-drop-down>
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
