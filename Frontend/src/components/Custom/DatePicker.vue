<script setup lang="ts">
import { ref, watch, type Ref, onMounted } from 'vue'
import { useDate } from 'vuetify'
const props = withDefaults(defineProps<{
    modelValue?: Date | Date[],
    label?: string
}>(), {
    label: 'date'
})
const dateHelper = useDate()
const listMenu: Ref<boolean> = ref(false)
const dateMenu: Ref<boolean> = ref(false)
const date: Ref<undefined | Date | Date[]> = ref()
const displayDate = ref(props.modelValue)

const chips = {
    single: 'Single',
    multiple: 'Multiple',
    rangle: 'Range'
} as const
type Chip = typeof chips[keyof typeof chips]
const Selected = ref<Chip>('Single')

watch(date, () => {
    if (date.value) {
        if (Array.isArray(date.value) && date.value.length == 0) {
            displayDate.value = undefined
            emits('update:modelValue')
            return
        }
        displayDate.value = date.value
        emits('update:modelValue', displayDate.value)
        if (Selected.value == 'Single') {
            listMenu.value = false
        }
        return
    }
    emits('update:modelValue')
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue?: Date | Date[]): void
}>()
const clear = () => {
    displayDate.value = undefined
    date.value = undefined
    listMenu.value = false
    emits('update:modelValue')
}
const getDates = (arg: 'week' | 'month'): Date[] => {
    const today = new Date()
    let startOfPeriod = new Date(today)
    if (arg === 'week') {
        const dayOfWeek = today.getDay()
        startOfPeriod.setDate(today.getDate() - dayOfWeek)
    } else if (arg === 'month') {
        startOfPeriod.setDate(1)
    }
    const dates: Date[] = [];
    for (let i = 0; i <= (arg === 'week' ? today.getDay() : today.getDate() - 1); i++) {
        const newDate = new Date(startOfPeriod)
        newDate.setDate(startOfPeriod.getDate() + i)
        if (newDate <= today)
            dates.push(newDate)

    }
    return dates
}
const todaySelected = () => {
    displayDate.value = new Date();
    emits('update:modelValue', displayDate.value)
    listMenu.value = false
}
const thisWeekSelected = () => {
    displayDate.value = getDates('week')
    emits('update:modelValue', displayDate.value)
    listMenu.value = false
}
const thisMonthSelected = () => {
    displayDate.value = getDates('month')
    emits('update:modelValue', displayDate.value)
    listMenu.value = false
}
onMounted(() => Selected.value = 'Single')
</script>
<template>
    <v-menu v-model="listMenu" :close-on-content-click="false" :transition="false">
        <template v-slot:activator="{ isActive, props }">
            <v-btn v-bind="props" class="rounded-lg" variant="outlined" color="primary">
                <span v-if="!displayDate">
                    {{ label }}
                </span>
                <span v-else>
                    {{
                        Array.isArray(displayDate) ?
                        `Selected (${displayDate.length})` :
                        `${dateHelper.format(displayDate, 'normalDateWithWeekday')}`
                    }}
                </span>
                <template v-slot:append>
                    <v-icon v-if="displayDate" icon="mdi-close" @click.stop="clear" />
                    <v-icon :icon="`mdi-menu-${isActive ? 'up' : 'down'}`" />
                </template>
            </v-btn>
        </template>
        <v-list width="200" density="compact">
            <v-list-item @click="todaySelected">
                Today
            </v-list-item>
            <v-list-item @click="thisWeekSelected">
                This Week
            </v-list-item>
            <v-list-item @click="thisMonthSelected">
                This Month
            </v-list-item>
            <v-list-item class="cursor-pointer">
                Custom
                <template v-slot:append>
                    <v-icon :icon="dateMenu ? 'mdi-menu-left' : 'mdi-menu-right'" />
                </template>
                <v-menu activator="parent" v-model="dateMenu" open-on-hover location="right center"
                    :close-on-content-click="false">
                    <v-card elevation="0">
                        <v-card-text class="pa-0">
                            <v-date-picker v-model="date" color="primary" :max="new Date()" class="rounded-0"
                                :multiple="Selected == 'Single' ? false : Selected == 'Multiple' ? true : 'range'">
                            </v-date-picker>
                        </v-card-text>
                        <v-card-actions class="ga-3 justify-end">
                            <template v-for="(chip, key) in chips" :key="key">
                                <v-chip color="primary" :variant="chip == Selected ? 'elevated' : 'tonal'"
                                    @click="Selected = chip; date = displayDate = undefined;">
                                    {{ chip }}
                                    <template v-slot:append v-if="chip == Selected">
                                        <v-icon icon="mdi-check" />
                                    </template>
                                </v-chip>
                            </template>
                        </v-card-actions>
                    </v-card>
                </v-menu>
            </v-list-item>
        </v-list>
    </v-menu>
</template>