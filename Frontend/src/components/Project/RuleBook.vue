<script setup lang="ts">
import { computed, ref } from 'vue'
import { TextEditor } from '@/components/Custom'
import { useDisplay } from 'vuetify'

const { height } = useDisplay()
const newValue = ref<string>()
const oldValue = ref<string>()
const saveDisabled = computed(() => newValue.value == oldValue.value)
</script>
<template>
    <v-row>
        <v-col cols="12" class="d-flex justify-end">
            <v-btn prepend-icon="mdi-file-check" text="save" color="primary" width="120" :disabled="saveDisabled" />
        </v-col>
        <v-col>
            <v-card elevation="0">
                <text-editor :height="height - 300" v-model="newValue"></text-editor>
            </v-card>
        </v-col>
    </v-row>
</template>