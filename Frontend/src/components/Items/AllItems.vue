<script setup lang="ts">
import { ref, watch, type Ref, reactive } from 'vue'
import { ItemStore } from '@/stores'
import { EditItem, InfoItem } from '@/components/Items/'
import { HoverEffect, DeletePropmt, NoItem } from '@/components/Custom/'
import type { IItem } from '@/Models/ItemModels'
import { useDate } from 'vuetify'
const dateHelper = useDate()
const props = defineProps<{
    items: IItem[],
    date: Date | null
}>()
const itemToDisplay = ref(props.items)
const { DeleteItem } = ItemStore()
const id: Ref<string> = ref('')

const visible = reactive<{
    edit: boolean,
    delete: boolean,
    info: boolean
}>({
    edit: false,
    delete: false,
    info: false
})

const deleteHandler = async (): Promise<void> => {
    await DeleteItem(id.value)
    visible.delete = false
}
watch(props, () => {
    itemToDisplay.value = props.items.filter(x => {
        return (
            !props.date ||
            dateHelper.format(x.createdOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate')
        )
    })
})
</script>
<template>
    <edit-item :id="id" v-model="visible.edit"></edit-item>
    <info-item :id="id" v-model="visible.info"></info-item>
    <delete-propmt v-model="visible.delete" @click:yes="deleteHandler">Item</delete-propmt>
    <no-item v-if="itemToDisplay.length == 0" title="No Item Found"
        :sub-title="date ? 'No record found on this date' : 'Please click on add button to insert new record'">
    </no-item>
    <v-col cols="12" lg="4" md="6" v-for="(item, index) of itemToDisplay" :key="index">
        <v-hover v-slot="{ isHovering, props }">
            <v-card v-bind="props" :elevation="isHovering ? 8 : 3" @click="() => { visible.info = true; id = item.id }">
                <v-card-title class="bg-primary d-flex">
                    <a :href="item.url" class="text-white" target="_blank">
                        <v-tooltip color="primary">
                            <template v-slot:activator="{ props }">
                                <v-chip class="me-4 cursor-pointer" v-bind="props">
                                    {{ item.type == 0 ? '#' : '!' }} {{ item.number }}
                                </v-chip>
                            </template>
                            {{ item.type == 0 ? 'Ticket' : 'PR' }}
                        </v-tooltip>
                    </a>
                    <span class="text-white text-truncate">{{ item.title }}</span>
                    <v-spacer></v-spacer>
                    <div>
                        <v-menu location="bottom" width="250">
                            <v-list>
                                <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                    @click="() => { id = item.id; visible.edit = true }">
                                    <hover-effect icon="file-document-edit-outline" icon-color="edit">
                                        Edit
                                    </hover-effect>
                                </v-list-item>
                                <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                    @click="() => { id = item.id; visible.delete = true }">
                                    <hover-effect icon="delete-outline" icon-color="delete">
                                        Delete
                                    </hover-effect>
                                </v-list-item>
                            </v-list>
                            <template v-slot:activator="{ props }">
                                <v-icon v-bind="props" color="white">mdi-dots-vertical</v-icon>
                            </template>
                        </v-menu>
                    </div>
                </v-card-title>
                <v-card-text class="">
                    <v-sheet height="100" class="pa-4 text-truncate">
                        <div v-if="item.description">
                            <div v-html="item.description"></div>
                        </div>
                        <div v-else class="text-grey">No Description</div>
                    </v-sheet>
                </v-card-text>
            </v-card>
        </v-hover>
    </v-col>
</template>
