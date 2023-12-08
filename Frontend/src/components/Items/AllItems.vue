<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { ItemStore } from '@/stores'

import EditItem from '@/components/Items/EditItem.vue'
import InfoItem from '@/components/Items/InfoItem.vue'
import HoverEffect from '@/components/Custom/HoverEffect.vue'
import Delete from '@/components/Custom/DeletePropmt.vue'
import { ItemType } from '@/Models/enum'

const { Items } = storeToRefs(ItemStore())
const { DeleteItem } = ItemStore()
const id: Ref<string> = ref('')
const editVisible: Ref<boolean> = ref(false)
const deleteVisible: Ref<boolean> = ref(false)
const infoVisible: Ref<boolean> = ref(false)

const deleteHandler = async (): Promise<void> => {
    await DeleteItem(id.value)
    deleteVisible.value = false
}
</script>
<template>
    <edit-item :id="id" v-model="editVisible"></edit-item>
    <info-item :id="id" v-model="infoVisible"></info-item>
    <delete v-model="deleteVisible" @click:yes="deleteHandler">Item</delete>
    <v-col cols="12" lg="4" md="6" v-for="(item, index) of Items" :key="index">
        <v-card @click="() => { infoVisible = true; id = item.id }">
            <v-card-title class="bg-primary d-flex">
                <a :href="item.url" class="text-white" target="_blank">
                    <v-chip class="me-4 cursor-pointer">
                        {{ item.type == ItemType.TICKET ? '#' : '!' }} {{ item.number }}
                    </v-chip>
                </a>
                <span class="text-white">{{ item.title }}</span>
                <v-spacer></v-spacer>
                <div>
                    <v-menu location="bottom" width="250">
                        <v-list>
                            <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                @click="() => { id = item.id; editVisible = true }">
                                <hover-effect icon="file-document-edit-outline" icon-color="edit">
                                    Edit
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0 mt-2"
                                @click="() => { id = item.id; deleteVisible = true }">
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
    </v-col>
</template>
