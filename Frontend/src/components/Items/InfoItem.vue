<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { useDate } from 'vuetify'
import { ItemStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import { AllComments } from '@/components/Comments/'

const { GetById } = ItemStore()
const tab: Ref<'info' | 'comments' | 'logs'> = ref('info')
const visible: Ref<boolean> = ref(false)
const Item: Ref<IItem | null> = ref(null)
const dateHelper = useDate()

const props = withDefaults(defineProps<{
    modelValue: boolean,
    id: string
}>(), {
    modelValue: false
})

const downloadFile = (path: string) => {
    window.open(path, '_blank')
}

watch(props, () => {
    visible.value = props.modelValue
    tab.value = 'info'
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
    <v-dialog transition="scale-transition" v-model="visible" max-width="800">
        <v-card v-if="Item">
            <v-card-title class="bg-primary d-flex">
                <span>
                    <a :href="Item.url" class="text-decoration-none text-white" target="_blank">
                        <v-tooltip>
                            <template v-slot:activator="{ props }">
                                <v-chip class="cursor-pointer" v-bind="props">
                                    {{ Item.type == 0 ? '#' : '!' }}
                                    {{ Item.number }}
                                </v-chip>
                            </template>
                            {{ Item.type == 0 ? 'Ticket' : 'PR' }}
                        </v-tooltip>
                    </a>
                </span>
                <v-spacer></v-spacer>
                <span class="text-truncate">{{ Item.title }}</span>
                <v-spacer></v-spacer>
                <v-icon color="white" @click="(visible = false)">mdi-close</v-icon>
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
                                <div class="mb-3">Discussed with:
                                    <v-chip v-if="Item.to" color="primary">{{ Item.to }}</v-chip>
                                    <span class="text-grey text-h5" v-else>-</span>
                                </div>
                                <div class="mb-3">Discussed by:
                                    <v-chip color="primary" v-if="Item.discussedBy">
                                        {{ Item.discussedBy }}
                                    </v-chip>
                                    <span v-else class="text-grey text-h5">-</span>
                                </div>
                            </div>
                            <div class="description rounded-lg pa-3 mt-2 ">
                                <div v-if="Item.description" v-html="Item.description"></div>
                                <div v-else class="text-grey">No description provided</div>
                            </div>
                            <div v-if="Item.files.length > 0" class="mt-3">Files:</div>
                            <v-row class="mt-2">
                                <v-col v-for="(file, index) in Item.files" :key="index" cols="auto">
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
                                        <v-icon @click="() => downloadFile(file.fileUrl)">mdi-download</v-icon>
                                    </v-card>
                                </v-col>
                            </v-row>
                        </v-window-item>
                        <v-window-item value="comments">
                            <all-comments :item-id="Item.id" :comments="Item.comments"></all-comments>
                        </v-window-item>
                        <v-window-item value="logs">
                            <v-row>
                                <v-col cols="6">created by:</v-col>
                                <v-col cols="6">{{ Item.createdBy }}</v-col>
                                <v-col cols="6">created on:</v-col>
                                <v-col cols="6">
                                    {{ dateHelper.format(Item.createdOn, 'keyboardDate') }}
                                </v-col>
                                <v-col cols="6">modified by:</v-col>
                                <v-col cols="6">
                                    {{ Item.updatedBy ?? '-' }}
                                </v-col>
                                <v-col cols="6">modified on:</v-col>
                                <v-col cols="6">
                                    <span v-if="Item.updatedOn">
                                        {{ dateHelper.format(Item.updatedOn, 'keyboardDate') }}
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