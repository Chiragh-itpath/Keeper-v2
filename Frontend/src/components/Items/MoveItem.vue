<script setup lang="ts">
import type { IMoveItem } from '@/Models/ItemModels';
import type { IKeep } from '@/Models/KeepModels';
import { computed, ref } from 'vue';

const props = defineProps<{
    action: 'copy' | 'move',
    keeps: IKeep[],
    itemId: string,
    keepId: string,
}>()

const emits = defineEmits<{
    (e: 'move', item: IMoveItem): void,
}>()
const items = ref<IKeep[]>(props.keeps)

const icon = computed(() => {
    return props.action === 'copy' ? 'mdi-content-copy' : 'mdi-content-cut'
})
const label = computed(() => {
    return props.action === 'copy' ? 'Copy' : 'Move'
})

const handleClick = (keep: IKeep) => {
    const moveItem: IMoveItem = {
        itemId: props.itemId,
        currentKeepId: props.keepId,
        targetKeepId: keep.id,
        action : props.action === 'move' ? 0 : 1,
    }
    emits('move', moveItem);
}

</script>

<template>
    <v-list-item role="button">
        <v-icon>{{ icon }}</v-icon>
        <span class="mx-3">{{ label }}</span>
        <template v-slot:append>
            <v-icon icon="mdi-menu-right" size="x-small"></v-icon>
        </template>
        <v-menu :open-on-focus="false" activator="parent" open-on-hover submenu location="end" width="250" max-height="250">
            <v-list >
                <template v-for="keep in items" :key="keep.id">
                    <v-list-item @click="handleClick(keep)" v-if="keep.id !== props.keepId">
                        {{ keep.title }}
                    </v-list-item>
                </template>
            </v-list>
        </v-menu>
    </v-list-item>
</template>