<script setup lang="ts">
import { ref, watch, type Ref } from 'vue';
import type { IContact } from '@/Models/ContactModels'

const props = withDefaults(defineProps<{
    checkbox?: boolean,
    search?: string,
    hideHeader?: boolean,
    showBorder?: boolean,
    contacts: IContact[],
    noContacts?: boolean
}>(), {
    checkbox: false,
    search: '',
    hideHeader: false,
    showBorder: false,
    noContacts: false
})


const checkedContacts: Ref<string[]> = ref([])
const ContactsToDisplay: Ref<IContact[]> = ref(props.contacts)
const chcekHandler = (checked: boolean | null, value: string) => {
    if (checked) {
        checkedContacts.value.push(value)
    } else {
        checkedContacts.value = checkedContacts.value.filter(x => x != value)
    }
    emits('update:selected', checkedContacts.value)
}
watch(props, () => {
    if (props.search != '' && props.search != null) {
        ContactsToDisplay.value = props.contacts.filter(x =>
            x.addedPerson.userName.startsWith(props.search) ||
            x.addedPerson.email.startsWith(props.search)
        )
    } else {
        ContactsToDisplay.value = props.contacts
    }
})
const HighLightText = (text: string) => {
    const searchText = props.search;
    if (searchText == '') {
        return text;
    }
    const regex = new RegExp(searchText, 'gi')
    return text.replace(regex, match => `<span class="high-light">${match}</span>`)
}
const emits = defineEmits<{
    (e: 'update:selected', selected: string[]): void
}>()
</script>
<template>
    <v-table :class="showBorder ? 'border' : ''">
        <thead v-if="!hideHeader">
            <tr>
                <th></th>
                <th>Name</th>
                <th>Email</th>
            </tr>
        </thead>
        <tbody>
            <tr v-if="ContactsToDisplay.length == 0">
                <td class="text-center text-grey" colspan="3">No Contacts</td>
            </tr>
            <tr v-for="(contact, index) in ContactsToDisplay" :key="index">
                <td v-if="checkbox" class="v-col-1">
                    <v-checkbox color="primary" hide-details :model-value="checkedContacts.includes(contact.id)"
                        @update:model-value="(x) => chcekHandler(x, contact.id)"></v-checkbox>
                </td>
                <td class="v-col-2 v-col-md-1">
                    <v-avatar class="text-white text-uppercase" color="primary">
                        {{ contact.addedPerson.email.slice(0, 1) }}
                    </v-avatar>
                </td>
                <td>
                    <span v-html="HighLightText(contact.addedPerson.userName)"></span>
                </td>
                <td>
                    <span v-html="HighLightText(contact.addedPerson.email)"></span>
                </td>
            </tr>
        </tbody>
    </v-table>
</template>
<style>
.high-light {
    background: #BDBDBD;
}

tr>th {
    color: black !important;
}
</style>