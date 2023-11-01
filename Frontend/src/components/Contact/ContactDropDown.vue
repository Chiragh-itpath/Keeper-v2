<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore } from '@/stores/ContactStore'

const props = withDefaults(defineProps<{
    contactId?: string[],
    multiple?: boolean,
    display?: 'email' | 'user',
    errors?: string
}>(), {
    contactId: () => new Array<string>(),
    multiple: true,
    display: 'email',
    errors: undefined
})

const { Contacts } = storeToRefs(ContactStore())
const modelValue = ref()


const items = computed(() => {
    if (props.display == 'email') {
        return Contacts.value.filter(x => !props.contactId.includes(x.id)).map(x => x.email)
    }
    return Contacts.value.filter(x => !props.contactId.includes(x.id)).map(x => x.userName)
})
watch(modelValue, () => {
    if (Array.isArray(modelValue.value)) {
        const emails = Contacts.value.filter(x => modelValue.value.includes(x.email)).map(x => x.email)
        emits('update:emails', emails)
        const ids = Contacts.value.filter(x => modelValue.value.includes(x.email)).map(x => x.id)
        emits('update:ids', ids)
    } else if (typeof modelValue.value == 'string') {
        const email = Contacts.value.filter(x => x.email == modelValue.value)?.map(x => x.email).pop()
        emits('update:email', email)
    } else {
        emits('update:email', undefined)
        emits('update:emails', undefined)
        emits('update:ids', undefined)
    }
})

const emits = defineEmits<{
    (e: 'update:email', modelValue: string | undefined): void,
    (e: 'update:emails', modelValue: string[] | undefined): void
    (e: 'update:ids', modelValue: string[] | undefined): void
}>()

</script>

<template>
    <v-combobox :items="items" :multiple="multiple" chips color="primary" label="Email" placeholder="Select Contacts"
        v-model="modelValue" :error-messages="errors">
        <template v-slot:chip="{ item }">
            <v-chip color="primary">{{ item.value }}</v-chip>
        </template>
    </v-combobox>
</template>