<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { ItemStore } from '@/stores'
import type { IItem } from '@/Models/ItemModels'
import AllComments from '@/components/Comments/AllComments.vue'
import { useDate } from 'vuetify'

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
    <v-dialog transition="scale-transition" v-model="visible" width="1000">
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
                <v-card height="400" max-height="400" class="overflow-y-auto mx-2 pa-5 " elevation="0">
                    <v-window v-model="tab" class="mt-5">
                        <v-window-item value="info">
                            <div v-if="Item.url" class="mb-3">
                                Ticket | PR: <a :href="Item.url" target="_blank">{{ Item.url }}</a>
                            </div>
                            <div>
                                <div class="mb-3">Discuss with:
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
                            <div v-if="Item.files.length > 0" class="mt-3">Files:</div>
                            <div v-for="(file, index) in Item.files" :key="index" class="my-3">
                                <a :href="file.fileUrl" target="_blank">
                                    <v-chip color="primary" class="pa-3 cursor-pointer">
                                        <span class="ma-2">{{ file.fileName }}</span>
                                    </v-chip>
                                </a>
                            </div>
                            <div class="description rounded-lg pa-3 mt-2 ">
                                <div v-if="Item.description" v-html="Item.description"></div>
                                <div v-else class="text-grey">No description provided</div>
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
                                    <span>-</span>
                                </v-col>
                            </v-row>
                        </v-window-item>
                    </v-window>
                </v-card>
            </v-card-text>
        </v-card>

    </v-dialog>
</template>
<style scoped>
.v-col-4,
.v-col-8 {
    padding: 8px 12px;
}

.description {
    min-height: 150px;
    max-height: 300px;
    background-color: #ECEFF1;
    color: black;
    overflow-x: auto;
}

.scroll-y-reverse-transition {
    transition-duration: 1s !important;
}
</style>