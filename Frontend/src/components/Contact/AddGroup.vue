<script setup lang="ts">
import { ref, type Ref } from 'vue'
const visible: Ref<boolean> = ref(false)

</script>
<template>
    <div>
        <v-btn color="primary" append-icon="mdi-plus" class="rounded" @click="visible = !visible" width="100%" disabled>
            New Group
        </v-btn>
    </div>
</template>