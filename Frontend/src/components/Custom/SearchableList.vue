<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { VTextField } from 'vuetify/components'
const props = defineProps<{
    modelValue?: string,
    label?: string,
    placeholder?: string,
    searchItems: { title: string, subtitle?: string, value: string }[]
}>();
const search: Ref<string> = ref(props.modelValue ?? '')
const menu: Ref<boolean> = ref(false)
const displayList = ref(props.searchItems)
const inputHandler = () => {
    if (search.value == '') {
        displayList.value = props.searchItems
    } else {
        menu.value = true
        displayList.value = props.searchItems.filter(x =>
            x.title.toLowerCase().startsWith(search.value.toLowerCase()) ||
            x.subtitle?.toLowerCase().startsWith(search.value.toLowerCase()) ||
            x.value.toLowerCase().startsWith(search.value.toLowerCase())
        )
    }
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
                :subtitle="item.subtitle" :value="item.value" @click="() => itemClickHandler(item)">
            </v-list-item>
            <v-list-item v-if="displayList.length == 0" title="No data" density="compact" disabled></v-list-item>
        </v-list>
    </v-menu>
</template>