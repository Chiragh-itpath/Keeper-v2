<script setup lang="ts">
import { computed, ref, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GroupStore } from '@/stores'
import type { IGroup } from '@/Models/GroupModels'
import type { IContact } from '@/Models/ContactModels'

defineProps<{
    errorMessage?: string
}>()
const { Contacts, isContactFetched } = storeToRefs(ContactStore())
const { Groups, isGroupFetched } = storeToRefs(GroupStore())
const menu: Ref<boolean> = ref(false)
type itemType = { title: string, subtitle?: string, value: string | string[], isDisabled?: boolean }
const items = computed((): itemType[] => {
    const mapGroup = (group: IGroup) => ({
        title: group.name,
        value: group.contacts.map(contact => contact.email),
    })
    const mapContact = (contact: IContact) => ({
        title: `${contact.firstName} ${contact.lastName}`,
        subtitle: contact.email,
        value: contact.email,
    })

    return [
        ...(Groups.value.filter(x => x.contacts.length).map(mapGroup).length
            ? [{ title: 'Groups', value: '', isDisabled: true }, ...Groups.value.filter(x => x.contacts.length).map(mapGroup)]
            : []
        ),
        ...(Contacts.value.map(mapContact).length
            ? [{ title: 'Contacts', value: '', isDisabled: true }, ...Contacts.value.map(mapContact)]
            : []
        ),
    ]
})
const selectHandler = (selected: readonly any[]) => {
    emits('update:emails', [...new Set(selected.flat(1))])
}
const emits = defineEmits<{
    (e: 'update:emails', emails: string[]): void,
}>()
onMounted(async () => {
    if (!isContactFetched.value) {
        await ContactStore().GetContacts()
    }
    if (!isGroupFetched.value) {
        await GroupStore().GetGroups()
    }
})
</script>

<template>
    <v-autocomplete :items="items" color="primary" multiple chips label="select contact or group" v-model:menu="menu"
        @update:model-value="selectHandler" :error-messages="errorMessage">
        <template v-slot:chip="{ item }">
            <v-chip color="primary">
                {{ item.raw.subtitle ?? item.title }}
            </v-chip>
        </template>
        <template v-slot:item="{ item, props: itemProp }">
            <v-list-item lines="one" class="py-0 ma-0" :title="item.title" :subtitle="item.raw.subtitle" :value="item.value"
                :disabled="item.raw.isDisabled" v-bind="itemProp">
                <template v-slot:prepend="{ isActive }" v-if="!(typeof item.value === 'string' && item.value == '')">
                    <v-checkbox :model-value="isActive" hide-details></v-checkbox>
                </template>
                <template v-slot:append>
                    <span v-if="typeof item.value === 'object'">
                        ({{ item.value.length }})
                    </span>
                </template>
            </v-list-item>
        </template>
        <template v-slot:no-data>
            <v-list-item class="text-grey">
                No contacts or group added
            </v-list-item>
        </template>
    </v-autocomplete>
</template>