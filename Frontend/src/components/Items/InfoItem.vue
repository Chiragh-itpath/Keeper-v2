<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { ItemStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import { dateHelper } from '@/Services/HelperFunction'

import AllComments from '@/components/Comments/AllComments.vue'
const { GetById } = ItemStore()

const itemtype = ['Ticket', 'PR']
const visible: Ref<boolean> = ref(false)
const Item: Ref<IItem | null> = ref(null)
const props = withDefaults(defineProps<{
    modelValue: boolean,
    id: string
}>(), {
    modelValue: false
})

watch(props, () => {
    visible.value = props.modelValue
})

watch(visible, async () => {
    Item.value = await GetById(props.id)
    if (!visible.value) {
        emits('update:modelValue', visible.value)
    }
})

const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()
</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible">
        <v-row class="justify-center" v-if="Item">
            <v-col cols="12" lg="8">
                <v-card width="100%" max-height="600" class="overflow-auto">
                    <v-card-title class="bg-primary text-center">
                        <div>
                            Item Details
                            <v-icon class="float-end" color="white" @click="(visible = false)">mdi-close</v-icon>
                        </div>
                    </v-card-title>
                    <v-card-text class="pt-5">
                        <v-row class="ps-3 ">
                            <v-col cols="12" md="7" class="main text-body-2">
                                <div class="mb-3 ">Title: {{ Item.title }} </div>
                                <div v-if="Item.url" class="mb-3">
                                    URL: <a :href="Item.url" target="_blank">{{ Item.url }}</a>
                                </div>
                                <div class="description rounded-lg pa-3 ">
                                    <div v-if="Item.description" v-html="Item.description"></div>
                                    <div v-else class="text-grey">No description provided</div>
                                </div>
                                <div class="mt-4">
                                    <div class="my-1" v-if="Item.files">Files:</div>
                                    <div v-for="(file, index) in Item.files" :key="index" class="my-3">
                                        <a :href="file.fileUrl" target="_blank">
                                            <v-chip color="primary" class="pa-3 cursor-pointer">
                                                <span class="ma-2">{{ file.fileName }}</span>
                                            </v-chip>
                                        </a>
                                    </div>
                                </div>
                            </v-col>
                            <v-divider vertical thickness="2" color="black" class="d-none d-md-flex"></v-divider>
                            <v-col>
                                <v-row>
                                    <v-col cols="4">type:</v-col>
                                    <v-col cols="8">{{ itemtype[Item.type] }} </v-col>
                                    <v-col cols="4">number:</v-col>
                                    <v-col cols="8">
                                        <a :href="Item.url" target="_blank">
                                            {{ Item.number }}
                                        </a>
                                    </v-col>
                                    <v-col cols="4">to:</v-col>
                                    <v-col cols="8">{{ Item.to ?? '-' }}</v-col>
                                    <v-col cols="4">discussed by:</v-col>
                                    <v-col cols="8">{{ Item.discussedBy ?? '-' }}</v-col>
                                    <v-col cols="4">created by:</v-col>
                                    <v-col cols="8">{{ Item.createdBy }}</v-col>
                                    <v-col cols="4">created on:</v-col>
                                    <v-col cols="8">{{ dateHelper(Item.createdOn) }}</v-col>
                                    <v-col cols="4">updated by:</v-col>
                                    <v-col cols="8">{{ Item.updatedBy ?? '-' }}</v-col>
                                    <v-col cols="4">updated on:</v-col>
                                    <v-col cols="8">{{ dateHelper(Item.updatedOn) }}</v-col>
                                </v-row>
                            </v-col>
                            <v-col cols="12" class="mt-3">
                                <all-comments :item-id="Item.id" :comments="Item.comments"></all-comments>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-dialog>
</template>
<style scoped>
.v-col-4,
.v-col-8 {
    padding: 8px 12px;
}

.description {
    height: 150px;
    background-color: #ECEFF1;
    color: black;
    overflow-x: auto;
}

.scroll-y-reverse-transition {
    transition-duration: 1s !important;
}
</style>