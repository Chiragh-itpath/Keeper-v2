<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { VTextField } from 'vuetify/components'
const props = defineProps<{
    modelValue?: string,
    label?: string,
    placeholder?: string,
    searchItems: { title: string, value: string }[]
}>();
const search: Ref<string> = ref(props.modelValue ?? '')
const menu: Ref<boolean> = ref(false)
const displayList = ref(props.searchItems)
const inputHandler = () => {
    if (search.value == '') {
        displayList.value = props.searchItems
    } else {
        displayList.value = displayList.value.filter(x => x.value.startsWith(search.value) || x.title.startsWith(search.value))
    }
    emit('update:modelValue', search.value != '' ? search.value : undefined)
}
const itemClickHandler = (item: { title: string, value: string }) => {
    search.value = item.value
    menu.value = false
    displayList.value = props.searchItems
    emit('update:modelValue', search.value)
}
const emit = defineEmits<{
    (e: 'update:modelValue', value: string | undefined): void
}>()
</script>
<template>
    <v-menu v-model="menu" :close-on-content-click="false" max-height="250">
        <template v-slot:activator="{ props: menu, isActive: activator }">
            <v-text-field color="primary" density="comfortable" hide-details :label="label" :placeholder="placeholder"
                v-bind="menu" v-model="search" @input="inputHandler">
                <template v-slot:append-inner>
                    <v-icon :icon="activator ? 'mdi-menu-up' : 'mdi-menu-down'" class="cursor-pointer"></v-icon>
                </template>
            </v-text-field>
        </template>
        <v-list>
            <v-list-item v-for="(item, index) in displayList" :key="index" density="compact" :title="item.title"
                :subtitle="item.value" :value="item.value" @click="() => itemClickHandler(item)">
            </v-list-item>
            <v-list-item v-if="displayList.length == 0" title="No data" density="compact" disabled></v-list-item>
        </v-list>
    </v-menu>
</template>