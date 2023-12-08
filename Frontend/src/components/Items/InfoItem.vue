<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { ItemStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import { dateHelper } from '@/Services/HelperFunction'
import AllComments from '@/components/Comments/AllComments.vue'

const { GetById } = ItemStore()
const tab: Ref<'info' | 'comments' | 'logs'> = ref('info')
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
    <v-dialog transition="scale-transition" v-model="visible">
        <v-row class="justify-center" v-if="Item">
            <v-col cols="12" lg="8">
                <v-card width="100%" max-height="600" class="overflow-auto">
                    <v-card-title class="bg-primary">
                        <v-row>
                            <v-col cols="2" class="text-start">
                                <v-chip>
                                    <a :href="Item.url" class="text-decoration-none text-white">
                                        {{ Item.type == 0 ? '#' : '!' }}
                                        {{ Item.number }}
                                    </a>
                                </v-chip>
                            </v-col>
                            <v-col cols="8" class="text-center mt-1">{{ Item.title }}</v-col>
                            <v-col cols="2" class="text-end">
                                <v-icon color="white" @click="(visible = false)">mdi-close</v-icon>
                            </v-col>
                        </v-row>

                    </v-card-title>
                    <v-card-text class="">
                        <div class="d-flex flex-row-reverse">

                            <v-tabs v-model="tab" color="primary" class="">
                                <v-tab value="info">Info</v-tab>
                                <v-tab value="comments">Comments</v-tab>
                                <v-tab value="logs">logs</v-tab>
                            </v-tabs>
                        </div>
                        <v-window v-model="tab" class="mt-5">
                            <v-window-item value="info">
                                <div v-if="Item.url" class="mb-3">
                                    URL: <a :href="Item.url" target="_blank">{{ Item.url }}</a>
                                </div>
                                <div>
                                    <div class="mb-3">To: {{ Item.to ?? '' }}</div>
                                    <div class="mb-3">Discussed by:{{ Item.discussedBy ?? '' }}</div>
                                </div>
                                <div class="description rounded-lg pa-3 mt-2 ">
                                    <div v-if="Item.description" v-html="Item.description"></div>
                                    <div v-else class="text-grey">No description provided</div>
                                </div>
                                <div v-if="Item.files.length > 0" class="mt-3">Files:</div>
                                <div v-for="(file, index) in Item.files" :key="index" class="my-3">
                                    <a :href="file.fileUrl" target="_blank">
                                        <v-chip color="primary" class="pa-3 cursor-pointer">
                                            <span class="ma-2">{{ file.fileName }}</span>
                                        </v-chip>
                                    </a>
                                </div>
                            </v-window-item>
                            <v-window-item value="comments">
                                <all-comments :item-id="Item.id" :comments="Item.comments"></all-comments>
                            </v-window-item>
                            <v-window-item value="logs">
                                <v-row>
                                    <v-col cols="6">created by:</v-col>
                                    <v-col cols="6">{{ Item.createdBy }}</v-col>
                                    <v-col cols="6">created on:</v-col>
                                    <v-col cols="6">{{ dateHelper(Item.createdOn) }}</v-col>
                                    <v-col cols="6">updated by:</v-col>
                                    <v-col cols="6">{{ Item.updatedBy ?? '-' }}</v-col>
                                    <v-col cols="6">updated on:</v-col>
                                    <v-col cols="6">{{ dateHelper(Item.updatedOn) }}</v-col>
                                </v-row>
                            </v-window-item>
                        </v-window>
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