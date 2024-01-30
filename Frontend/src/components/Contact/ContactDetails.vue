<script setup lang="ts">
import { ref, type Ref } from 'vue'

const visible: Ref<boolean> = ref(false)
</script>
<template>
    <v-dialog v-model="visible">
        <template v-slot:activator="{ props }">
            <slot :props="props"></slot>
        </template>
    </v-dialog>
</template>