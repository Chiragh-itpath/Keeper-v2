<script setup lang="ts">
import { ref, watch } from 'vue'
import { useDate } from 'vuetify'
const props = withDefaults(defineProps<{
    modelValue?: any,
    label?: string
}>(), {
    modelValue: '',
    label: ''
})
const dateHelper = useDate()

const menu = ref()
const date = ref()
const displayDate = ref(props.modelValue)

watch(date, () => {
    if (date.value) {
        emits('update:modelValue', date.value)
        menu.value = false
    }
})
watch(props, () => {
    if (props.modelValue == '') {
        displayDate.value = ''
        date.value = undefined
    }
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: any): void
}>()

const clear = () => {
    emits('update:modelValue', undefined)
}
</script>
<template>
    <v-menu v-model="menu" :close-on-content-click="false" transition="scale-transition">
        <template v-slot:activator="{ isActive, props }">
            <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary" v-if="!date">
                <template v-slot:append>
                    <v-icon>
                        {{ `mdi-menu-${isActive ? 'up' : 'down'}` }}
                    </v-icon>
                </template>
                date
            </v-btn>
            <v-btn class="rounded-lg" variant="flat" color="primary" v-else>
                {{ `${dateHelper.format(date, 'normalDateWithWeekday')}` }}
                <template v-slot:append>
                    <v-icon @click="clear">mdi-close</v-icon>
                </template>
            </v-btn>
        </template>
        <v-date-picker v-model="date" color="primary" :max="new Date()" @click:cancel="menu = false" hide-weekdays>
        </v-date-picker>
    </v-menu>
</template>
<style >
.change-icon>div.v-input__control>.v-field>.v-field__append-inner>.v-icon {
    color: #26A69A !important;
    opacity: 1 !important;
}
</style>