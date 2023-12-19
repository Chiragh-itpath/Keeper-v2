<script setup lang="ts">
import { ref, watch, type Ref } from 'vue';

const props = withDefaults(defineProps<{
    items: string[]

}>(), {

})
const tags = ref(props.items)
const menu: Ref<boolean> = ref(false)
const selected: Ref<string[]> = ref([])
watch(props, () => [
    tags.value = props.items
])
watch(selected, () => {
    emits('update:selected', selected.value)
})
const emits = defineEmits<{
    (e: 'update:selected', value: string[]): void
}>()
</script>
<template>
    <v-menu v-model="menu" :close-on-content-click="false">
        <template v-slot:activator="{ props }">
            <v-btn variant="outlined" color="primary" v-bind="props" class="rounded-lg">
                <template v-slot:append>
                    <v-icon>{{ `mdi-menu-${menu ? 'up' : 'down'}` }}</v-icon>
                </template>
                Tag
                <span v-if="selected.length > 0">
                    ({{ selected.length }})
                </span>
            </v-btn>
        </template>
        <v-list min-width="200" select-strategy="classic" v-model:selected="selected" density="compact">
            <v-list-item v-for="(tag, index) in tags" :key="index" :value="tag">
                <template v-slot:prepend="{ isActive }">
                    <v-checkbox hide-details :model-value="isActive" color="primary" density="comfortable"></v-checkbox>
                </template>
                {{ tag }}
            </v-list-item>
        </v-list>
    </v-menu>
</template>