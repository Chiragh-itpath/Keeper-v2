<script setup lang="ts">
import { ref, watch } from 'vue'

const props = defineProps<{
    modelValue?: string
    label?: string
    placeholder?: string
    searchItems: { title: string; subtitle?: string; value: string }[]
    multiple?: boolean
}>()

const emit = defineEmits<{
    (e: 'update:modelValue', value: string | undefined): void
}>()

const selected = ref<string | string[] | undefined>(props.multiple ? [] : undefined)
const search = ref('')

watch(
    () => props.modelValue,
    (val) => {
        if (!val) {
            selected.value = props.multiple ? [] : undefined
            return
        }
        selected.value = props.multiple
            ? val.split(',').map(x => x.trim())
            : val.trim()
    },
    { immediate: true }
)

watch(selected, (val) => {
    const value = props.multiple
        ? (val as string[])?.join(', ')
        : val as string
    emit('update:modelValue', value)

    if (props.multiple) {
        search.value = ''
    }
})

const onKeydown = (e: KeyboardEvent) => {
    if (e.key === 'Tab') {
        const match = props.searchItems.find(x =>
            x.title.toLowerCase().includes(search.value.toLowerCase()) ||
            x.subtitle?.toLowerCase().includes(search.value.toLowerCase()) ||
            x.value.toLowerCase().includes(search.value.toLowerCase())
        )
        if (match && search.value) {
            if (props.multiple) {
                e.preventDefault()
                const currentSelected = selected.value as string[]
                if (!currentSelected.includes(match.value)) {
                    selected.value = [...currentSelected, match.value]
                }
            }
        }
    }
}
</script>

<template>
    <v-autocomplete v-model="selected" :items="searchItems" item-title="title" item-value="value" :multiple="multiple"
        :label="label" :placeholder="placeholder" v-model:search="search" :return-object="false" @keydown="onKeydown"
        item-color="primary" color="primary" :clearable="!multiple">
        <template #selection="{ index, item }">
            <template v-if="index < 2">
                <v-chip color="primary" class="me-1">
                    {{ item.title }}
                </v-chip>
            </template>
            <template v-else-if="index === 2">
                <v-chip color="primary">
                    +{{ selected.length - 2 }}
                </v-chip>
            </template>
        </template>
    </v-autocomplete>
</template>
<style>
.mdi-close-circle {
    color: rgb(38, 166, 154);
}
</style>