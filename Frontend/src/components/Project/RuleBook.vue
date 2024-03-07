<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { TextEditor } from '@/components/Custom'
import { useDisplay } from 'vuetify'
import { ProjectSettingsService } from '@/Services/ProjectSettings'

const { projectId } = defineProps<{
    projectId: string,
    isOwner: boolean
}>()

const projectSettings = new ProjectSettingsService()
const { height } = useDisplay()
const newValue = ref<string>()
const oldValue = ref<string>()
const editing = ref<boolean>(false)
const saveDisabled = computed(() => !!newValue.value == !!oldValue.value)
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
    editing.value = false
}
</script>

<template>
    <v-row class="justify-end " v-if="isOwner">
        <template v-if="editing">
            <v-col cols="auto">
                <v-btn prepend-icon="mdi-close" text="cancel" color="red" width="120"
                    @click="editing = false; newValue = oldValue" />
            </v-col>
            <v-col cols="auto">
                <v-btn prepend-icon="mdi-file-check" text="save" color="primary" width="120" :disabled="saveDisabled"
                    @click="onSave" />
            </v-col>
        </template>
        <v-col cols="auto" class="" v-else>
            <v-btn prepend-icon="mdi-pencil" text="edit" color="primary" width="120" @click="editing = true;" />
        </v-col>
    </v-row>
    <v-row>
        <v-col cols="12" v-if="editing">
            <v-card elevation="0">
                <text-editor :height="height - 300" v-model="newValue"></text-editor>
            </v-card>
        </v-col>
        <v-col cols="12" v-else>
            <v-sheet elevation="0" :height="height - (isOwner ? 300 : 220)" class="ma-2 pa-2 px-5 border overflow-auto">
                <span v-html="oldValue" v-if="oldValue"></span>
                <div class="text-grey h-100 d-flex justify-center align-center" v-else>
                    {{ isOwner ? 'Click on edit button to start typing' : 'No rules added yet' }}
                </div>
            </v-sheet>
        </v-col>
    </v-row>
</template>

<style>
.v-sheet>span>ol,
.v-sheet>span>ul {
    margin: 0 25px;
}
</style>