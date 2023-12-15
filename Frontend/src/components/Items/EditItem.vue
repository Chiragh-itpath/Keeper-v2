<script setup lang="ts">
import { reactive, ref, watch, type Ref } from 'vue'
import { ItemType } from '@/Models/enum'
import { ItemStore } from '@/stores'
import TextField from '@/components/Custom/TextField.vue'
import TextEditor from '@/components/Custom/TextEditor.vue'
import ContactDropDown from '../Contact/ContactDropDown.vue'
import type { IEditItem } from '@/Models/ItemModels'

const visible: Ref<boolean> = ref(false)
const form = ref()
const editItem: IEditItem = reactive({
    id: '',
    title: '',
    description: '',
    url: '',
    keepId: '',
    userId: '',
    number: '',
    type: ItemType.TICKET,
    to: '',
    discussedBy: '',
    files: null
})
const { getSingalItem, EditItem } = ItemStore()
const itemType = ref('Ticket')

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (!valid) return
    editItem.type = itemType.value == 'Ticket' ? ItemType.TICKET : ItemType.PR
    await EditItem(editItem)
    visible.value = false
}

const props = withDefaults(defineProps<{
    modelValue: boolean,
    id: string
}>(), {
    modelValue: false
})

watch(props, () => {
    visible.value = props.modelValue
    if (props.modelValue) {
        const item = getSingalItem(props.id)
        editItem.id = item!.id
        editItem.keepId = item!.keepId
        editItem.title = item!.title
        editItem.number = item!.number
        editItem.description = item!.description
        editItem.url = item!.url
        editItem.to = item!.to
        editItem.discussedBy = item!.discussedBy
        editItem.type = item!.type
    }
})
watch(visible, () => {
    if (!visible.value) {
        emits('update:modelValue', visible.value)
    }
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()

</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" close-on-back max-width="900">
        <v-card max-height="600" class="position-relative">
            <v-card-title class="bg-primary text-center position-sticky">
                Update Item
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="showScrollerbar">
                <v-form ref="form" @submit.prevent>
                    <v-row>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <v-select :items="['Ticket', 'PR']" label="Type" color="primary" v-model="itemType" />
                        </v-col>
                        <v-col cols="6" lg="3" md="3" sm="6">
                            <text-field label="Number" placeholder="Ticker | PR number" is-number
                                v-model="editItem.number" />
                        </v-col>
                        <v-col>
                            <text-field label="Item Name*" placeholder="Item title" is-required v-model="editItem.title" />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <text-field label="URL" placeholder="URL for Ticket | PR" is-url v-model="editItem.url"
                                icon="mdi-link-box-variant-outline" />
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
                            <contact-drop-down label="Discussed by" v-model="editItem.discussedBy"
                                v-model:email="editItem.discussedBy" :multiple="false"></contact-drop-down>
                        </v-col>
                    </v-row>
                </v-form>
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
