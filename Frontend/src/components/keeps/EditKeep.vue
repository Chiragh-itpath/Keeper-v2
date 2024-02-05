<script setup lang="ts">
import { ref, watch, type Ref, reactive } from 'vue'
import TextField from '@/components/Custom/TextField.vue'
import { GlobalStore, KeepStore } from '@/stores'
import type { IEditKeep, IKeep } from '@/Models/KeepModels'
import { storeToRefs } from 'pinia'

const props = defineProps<{
    keep: IKeep
}>()

const { Updatekeep } = KeepStore()
const { Loading } = storeToRefs(GlobalStore())
const visible: Ref<boolean> = ref(false)
const form = ref()
const editKeep: IEditKeep = reactive({
    id: props.keep.id,
    title: props.keep.title,
    tag: props.keep.tag,
    projectId: props.keep.projectId
})

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (valid) {
        await Updatekeep(editKeep)
        visible.value = false
    }
}

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
    <v-dialog v-model="visible" max-width="700" close-on-back>
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Edit keep
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form @submit.prevent="submitHandler" ref="form" validate-on="submit">
                    <v-row>
                        <v-col cols="12" sm="8">
                            <text-field v-model="editKeep.title" label="Title*" placeholder="Keep Title" is-required
                                :max-limit="100" />
                        </v-col>
                        <v-col cols="12" sm="4">
                            <text-field v-model="editKeep.tag" label="Tag" placeholder="Keep Tag" />
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="120"
                    class="px-5 mx-2 rounded-xl" :loading="Loading" :disabled="Loading">
                    Update
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
