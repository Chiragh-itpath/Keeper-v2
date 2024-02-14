<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import moment from 'moment'

import { AllComments } from '@/components/Comments/'
import { TypeList, ImagePreview } from '@/components/Items'
import type { IItem } from '@/Models/ItemModels'
import { ItemType } from '@/Models/enum'

const tab: Ref<'info' | 'comments' | 'logs'> = ref('info')
const visible: Ref<boolean> = ref(false)
defineProps<{
    item: IItem,
    modelValue?: boolean
}>()
const downloadFile = (path: string) => {
    window.open(path, '_blank')
}
watch(visible, () => {
    if (visible.value) {
        tab.value = 'info'
    }
})
const emit = defineEmits<{
    (e: 'update:modelValue', value: boolean): void
}>()
</script>
<template>
    <v-dialog v-model="visible" max-width="800" @update:model-value="(value) => emit('update:modelValue', value)">
        <template v-slot:activator="{ props }">
            <slot :activator="props" :visible="visible"></slot>
        </template>
        <v-card>
            <v-card-title class="bg-primary d-flex align-center">
                <a :href="item.url" class="text-decoration-none text-white" target="_blank">
                    <v-tooltip location="top">
                        <template v-slot:activator="{ props }">
                            <v-chip class="me-1" v-bind="props"
                                v-if="item.type == ItemType.TICKET || item.type == ItemType.PR">
                                {{ item.type == 0 ? '#' : '!' }} {{ item.number }}
                            </v-chip>
                            <v-icon size="small" v-bind="props"
                                v-if="item.type == ItemType.MAIL || item.type == ItemType.SUMMARY_MAIL"
                                :icon="item.type == ItemType.MAIL ? 'mdi-email-outline' : 'mdi-file'"></v-icon>
                        </template>
                        {{ TypeList[item.type].title }}
                    </v-tooltip>
                </a>
                <v-spacer></v-spacer>
                <span class="text-truncate">{{ item.title }}</span>
                <v-spacer></v-spacer>
                <slot name="edit"></slot>
                <v-tooltip location="top">
                    <template v-slot:activator="{ props: tooltip }">
                        <v-icon color="white" v-bind="tooltip" class="ms-2" @click="(visible = false)">mdi-close</v-icon>
                    </template>
                    Close
                </v-tooltip>
            </v-card-title>
            <v-card-text class="pa-0 pb-5">
                <div class="d-flex flex-row-reverse mx-5">
                    <v-tabs v-model="tab" color="primary" class="">
                        <v-tab value="info">Info</v-tab>
                        <v-tab value="comments">Comments</v-tab>
                        <v-tab value="logs">logs</v-tab>
                    </v-tabs>
                </div>
                <v-card height="450" max-height="450" class="overflow-y-auto mx-2 pa-3 " elevation="0">
                    <v-window v-model="tab" class="mt-5">
                        <v-window-item value="info">
                            <div>
                                <div class="mb-3">Discuss with:
                                    <span v-if="item.to">
                                        <template v-for="(client, index) in item.to.split(/,/)" :key="index">
                                            <v-chip color="primary" class="mx-1" v-if="client.trim()">
                                                {{ client.trim() }}
                                            </v-chip>
                                        </template>
                                    </span>
                                    <span class="text-grey text-h5" v-else>-</span>
                                </div>
                                <div class="mb-3">Discuss by:
                                    <v-chip color="primary" v-if="item.discussedBy">
                                        {{ item.discussedBy }}
                                    </v-chip>
                                    <span v-else class="text-grey text-h5">-</span>
                                </div>
                            </div>
                            <div class="description rounded-lg pa-3 mt-2 ">
                                <div v-if="item.description" v-html="item.description"></div>
                                <div v-else class="text-grey">No description provided</div>
                            </div>
                            <div v-if="item.files && item.files.length > 0" class="mt-3">Files:</div>
                            <v-row class="mt-2">
                                <v-col v-for="(file, index) in item.files" :key="index" cols="auto">
                                    <v-card max-width="200" color="primary" variant="tonal"
                                        class="d-flex justify-center align-center pa-3">
                                        <v-tooltip location="top">
                                            <template v-slot:activator="{ props }">
                                                <span class="text-black text-truncate" v-bind="props">
                                                    {{ file.fileName }}
                                                </span>
                                            </template>
                                            {{ file.fileName }}
                                        </v-tooltip>
                                        <image-preview v-if="file.isImage" v-slot="{ activator }" :image-url="file.fileUrl">
                                            <v-btn icon="mdi-eye" class="text-primary ms-2" density="compact" variant="flat"
                                                v-bind="activator" />
                                        </image-preview>
                                        <v-btn icon="mdi-download" class="text-primary" density="compact" variant="flat"
                                            @click="() => downloadFile(file.fileUrl)" />
                                    </v-card>
                                </v-col>
                            </v-row>
                        </v-window-item>
                        <v-window-item value="comments">
                            <all-comments :item-id="item.id" :comments="item.comments"></all-comments>
                        </v-window-item>
                        <v-window-item value="logs">
                            <v-row>
                                <v-col cols="6">Created By:</v-col>
                                <v-col cols="6">{{ item.createdBy }}</v-col>
                                <v-col cols="6">Created By:</v-col>
                                <v-col cols="6">
                                    {{ moment(item.createdOn).format('DD/MM/YYYY, hh:mm a') }}
                                </v-col>
                                <v-col cols="6">Modified By:</v-col>
                                <v-col cols="6">
                                    {{ item.updatedBy ?? '-' }}
                                </v-col>
                                <v-col cols="6">Modified On:</v-col>
                                <v-col cols="6">
                                    <span v-if="item.updatedOn">
                                        {{ moment(item.updatedOn).format('DD/MM/YYYY, hh:mm a') }}
                                    </span>
                                    <span v-else>-</span>
                                </v-col>
                            </v-row>
                        </v-window-item>
                    </v-window>
                </v-card>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<style>
.description {
    min-height: 150px;
    max-height: 300px;
    background-color: #ECEFF1;
    color: black;
    overflow-x: auto;
}

.description>div>ol,
.description>div>ul {
    margin: 0 15px;
}
</style>