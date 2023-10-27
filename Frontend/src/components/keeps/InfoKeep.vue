<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import type { IKeep } from '@/Models/KeepModels'
import { dateHelper } from '@/Services/HelperFunction'
import { UserStore, KeepStore } from '@/stores'

const props = withDefaults(defineProps<{
    modelValue: boolean,
    id: string
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(false)
const Keep: Ref<IKeep | undefined> = ref()
const { User, myProfile } = UserStore()
const { getSingle } = KeepStore()

watch(props, async () => {
    visible.value = props.modelValue
    if (props.modelValue) {
        Keep.value = getSingle(props.id)

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
                        {{ Keep?.createdBy == User.email ? 'me' : Keep?.createdBy }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Created On:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">{{ dateHelper(Keep?.createdOn) }}</v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified By:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ Keep?.createdBy == User.email ? 'me' : Keep?.createdBy }}
                    </v-col>

                    <v-col cols="6" class="text-grey pb-0 pb-sm-3">Last Modified:</v-col>
                    <v-col cols="6" class="pb-0 pb-sm-3">
                        {{ dateHelper(Keep?.updatedOn) }}
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
