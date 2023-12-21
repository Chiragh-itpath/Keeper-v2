<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import type { IKeep } from '@/Models/KeepModels'
import { UserStore } from '@/stores'
import { useDate } from 'vuetify'

const dateHelper = useDate()
const props = withDefaults(defineProps<{
    modelValue: boolean,
    id: string,
    keep?: IKeep
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(false)
const Keep: Ref<IKeep | undefined> = ref()
const { User, myProfile } = UserStore()

watch(props, async () => {
    visible.value = props.modelValue
    if (props.modelValue) {

        if (User.id == '') {
            await myProfile()
        }
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
    <v-dialog transition="scale-transition" v-model="visible" max-width="500">
        <v-card max-height="400" class="overflow-auto">
            <v-card-title class="text-center bg-primary">
                Keep Details
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text class="mb-4">
                <v-row>
                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Title:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">{{ Keep?.title }}</v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Tag:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        <v-chip color="primary">
                            {{ Keep?.tag }}
                        </v-chip>
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Owner:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ Keep?.createdBy }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Created On:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ dateHelper.format(Keep?.createdOn, 'keyboardDate') }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified By:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ Keep?.updatedBy }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        <span v-if="dateHelper.isValid(Keep?.updatedOn)">
                            {{ dateHelper.format(Keep?.updatedOn, 'keyboardDate') }}
                        </span>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
