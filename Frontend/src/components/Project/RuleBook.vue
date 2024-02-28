<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { TextEditor } from '@/components/Custom'
import { useDisplay } from 'vuetify'
import { ProjectSettingsService } from '@/Services/ProjectSettings'

const { projectId } = defineProps<{
    projectId: string
}>()

const projectSettings = new ProjectSettingsService()
const { height } = useDisplay()
const newValue = ref<string>()
const oldValue = ref<string>()
const saveDisabled = computed(() => newValue.value == oldValue.value)
onMounted(async () => {
    const rules = await projectSettings.GetRuleBook(projectId)
    if (rules) {
        newValue.value = oldValue.value = rules.text
    }
})

const onSave = async () => {
    const rules = await projectSettings.UpdateRuleBook({
        projectId,
        text: newValue.value ?? ''
    })
    if (rules) {
        oldValue.value = newValue.value = rules.text
    }
}
</script>
<template>
    <v-row>
        <v-col cols="12" class="d-flex justify-end">
            <v-btn prepend-icon="mdi-file-check" text="save" color="primary" width="120" :disabled="saveDisabled"
                @click="onSave" />
        </v-col>
        <v-col>
            <v-card elevation="0">
                <text-editor :height="height - 300" v-model="newValue"></text-editor>
            </v-card>
        </v-col>
    </v-row>
</template>