<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { VTextField } from 'vuetify/components'
const { modelValue, placeholder, label, searchItems, multiple } = defineProps<{
    modelValue?: string,
    label?: string,
    placeholder?: string,
    searchItems: { title: string, subtitle?: string, value: string }[],
    multiple?: boolean
}>();
const search: Ref<string> = ref(modelValue ?? '')
const selected = ref<string[]>([])
const menu: Ref<boolean> = ref(false)
const displayList = ref(searchItems)
const inputHandler = () => {
    if (search.value == '') {
        displayList.value = searchItems
    } else {
        menu.value = true
        displayList.value = searchItems.filter(x =>
            x.title.toLowerCase().startsWith(search.value.toLowerCase()) ||
            x.subtitle?.toLowerCase().startsWith(search.value.toLowerCase()) ||
            x.value.toLowerCase().startsWith(search.value.toLowerCase())
        )
    }
}
const itemClickHandler = (item: { title: string, value: string }) => {
    if (multiple) {
        const index = selected.value.findIndex(x => x == item.value)
        if (index > -1) {
            selected.value.splice(index, 1)
        } else {
            selected.value.push(item.value)
        }
        search.value = selected.value.join(', ')
    } else {
        search.value = item.value
        menu.value = false
        displayList.value = searchItems
    }
    emit('update:modelValue', search.value)
}
const emit = defineEmits<{
    (e: 'update:modelValue', value: string | undefined): void
}>()
onMounted(() => {
    if (modelValue)
        selected.value = modelValue
            .split(',')
            .map(x => x.trim())
})
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

            <template v-for="(item, index) in displayList" :key="index">
                <v-list-item density="compact" :active="selected.some(x => x == item.value)" color="primary"
                    :title="item.title" :subtitle="item.subtitle" :value="item.value"
                    @click="() => itemClickHandler(item)">
                </v-list-item>
            </template>
            <v-list-item v-if="displayList.length == 0" title="No data" density="compact" disabled></v-list-item>
        </v-list>
    </v-menu>
</template>