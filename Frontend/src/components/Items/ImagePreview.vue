<script setup lang="ts">
import { ref, type Ref } from 'vue'

const { imageUrl } = defineProps<{
    imageUrl: string
}>()
const visible: Ref<boolean> = ref(false)
</script>
<template>
    <v-dialog v-model="visible" max-width="800">
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Image Preview
                <v-btn density="compact" icon="mdi-close" variant="text" class="float-end" @click="visible = !visible" />
            </v-card-title>
            <v-card-text>
                <v-col cols="12">
                    <v-img :src="imageUrl" />
                </v-col>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>