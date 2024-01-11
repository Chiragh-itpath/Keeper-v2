<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import type { IKeep } from '@/Models/KeepModels'
import { useDate } from 'vuetify'

const dateHelper = useDate()
withDefaults(defineProps<{
    keep: IKeep
}>(), {

})

const visible: Ref<boolean> = ref(false)

watch(visible, () => {
    if (!visible.value) {
        emits('close')
    }
})
const emits = defineEmits<{
    (e: 'close'): void
}>()

</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="500" v-if="keep">
        <template v-slot:activator="{ props }">
            <v-list-item role="button" v-bind="props">
                <v-icon>mdi-information-outline</v-icon>
                <span class="mx-3">Info</span>
            </v-list-item>
        </template>
        <v-card class="overflow-auto">
            <v-card-title class="text-center bg-primary">
                Keep Details
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="mb-4">
                <v-row>
                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Title:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">{{ keep.title }}</v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Tag:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        <v-chip color="primary" v-if="keep.tag">
                            {{ keep.tag }}
                        </v-chip>
                        <span class="text-grey" v-else>-</span>
                    </v-col>
                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Members:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ keep.users.length }}
                    </v-col>
                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Owner:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ keep.createdBy }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Created On:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ dateHelper.format(keep.createdOn, 'keyboardDate') }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified By:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        <span v-if="keep.updatedBy">{{ keep.updatedBy }}</span>
                        <span v-else class="text-grey">-</span>
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        <span v-if="keep.updatedOn">
                            {{ dateHelper.format(keep.updatedOn, 'keyboardDate') }}
                        </span>
                        <span v-else class="text-grey">-</span>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
