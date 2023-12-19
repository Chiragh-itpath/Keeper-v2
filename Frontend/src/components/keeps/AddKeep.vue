<script setup lang="ts">
import { reactive, ref, type Ref, watch } from 'vue'
import { TextField } from '@/components/Custom'
import { KeepStore, GlobalStore } from '@/stores'
import type { IAddKeep } from '@/Models/KeepModels'
import { storeToRefs } from 'pinia';

const props = withDefaults(defineProps<{
    projectId: string,
}>(), {
})

const { AddKeep } = KeepStore()
const visible: Ref<boolean> = ref(false)
const { Loading } = storeToRefs(GlobalStore())
const form = ref()
const addKeep: IAddKeep = reactive({
    title: '',
    tag: '',
    projectId: ''
})

const submitHandler = async (): Promise<void> => {
    const { valid } = await form.value.validate()
    if (valid) {
        await AddKeep(addKeep)
        close()
    }
}
watch(visible, () => {
    if (visible.value) {
        addKeep.projectId = props.projectId
    }
})
const close = () => {
    visible.value = false
    form.value.reset()
}
</script>
<template>
    <v-btn @click="visible = true" color="primary" variant="elevated" prepend-icon="mdi-plus" width="100%">
        New Keep
    </v-btn>
    <v-dialog transition="scale-transition" v-model="visible" max-width="700" @update:model-value="close">
        <v-card>
            <v-card-title class="text-center bg-primary">
                New keep
                <v-icon class="float-end" @click="close">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-form @submit.prevent="submitHandler" ref="form" validate-on="submit">
                    <v-row>
                        <v-col cols="12" sm="8">
                            <text-field v-model="addKeep.title" placeholder="Keep Title" label="Title*" is-required
                                :max-limit="100" />
                        </v-col>
                        <v-col cols="12" sm="4">
                            <text-field v-model="addKeep.tag" label="Tag" placeholder="Keep Tag" />
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end ma-3">
                <v-btn @click="submitHandler" color="primary" variant="elevated" min-width="120"
                    class="px-5 mx-2 rounded-xl" :loading="Loading" :disabled="Loading">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
