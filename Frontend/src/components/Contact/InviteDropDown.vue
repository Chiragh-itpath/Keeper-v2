<script setup lang="ts">
import { computed, ref, watch, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GroupStore } from '@/stores'
import type { IUser } from '@/Models/UserModels'

defineProps<{
    errorMessage?: string
}>()
const selectedItem: Ref<any[]> = ref([])
const { Contacts, isContactFetched } = storeToRefs(ContactStore())
const { Groups, isGroupFetched } = storeToRefs(GroupStore())
const menu: Ref<boolean> = ref(false)
const items = computed(() => {
    let groups = Groups.value
        .filter(x => x.contacts.length > 0)
        .map(x => {
            return {
                title: x.name,
                value: { id: x.id, type: 'group' }
            }
        })
    let contacts = Contacts.value
        .map(x => {
            return {
                title: x.addedPerson.email,
                value: { id: x.id, type: 'contact' }
            }
        })

    let item: any[] = [];

    if (groups.length > 0)
        item.push({ title: 'Groups', type: 'header' }, ...groups);

    if (contacts.length > 0)
        item.push({ title: 'Contacts', type: 'header' }, ...contacts);

    return item
})
const getContactCount = (id: string): number => {
    const group = Groups.value.find(x => x.id == id)
    if (group) {
        return group.contacts.length
    }
    return 0
}

watch(selectedItem, () => {
    menu.value = false
    const user = selectedItem.value.flatMap((x: { id: string, type: 'group' | 'contact' }) => {
        if (x.type === 'contact') {
            const contact = Contacts.value.find(c => c.id === x.id);
            return contact ? [contact.addedPerson] : [];
        } else {
            const group = Groups.value.find(g => g.id === x.id);
            return group ? group.contacts.map(c => c.addedPerson) : [];
        }
    }).filter((user, index, array) =>
        array.findIndex(u => u.id == user.id) === index
    );
    emits('update:users', user)
    menu.value = true
})

const emits = defineEmits<{
    (e: 'update:emails', emails: string[]): void,
    (e: 'update:users', users: IUser[]): void
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
    <v-autocomplete :items="items" color="primary" v-model="selectedItem" multiple chips label="select contact or group"
        v-model:menu="menu" :error-messages="errorMessage" hide-details="auto">
        <template v-slot:chip="{ item }">
            <v-chip color="primary">{{ item.title }}</v-chip>
        </template>
        <template v-slot:item="{ item, props: itemProp }">
            <v-list-item :title="item.title" v-bind="itemProp" :disabled="!!item.raw.type" class="py-0 ma-0" lines="one">
                <template v-slot:prepend="{ isActive }">
                    <v-checkbox :model-value="isActive" hide-details v-if="!item.raw.type"></v-checkbox>
                </template>
                <template v-slot:append>
                    <span v-if="item.props.value.type == 'group'">
                        ({{ getContactCount(item.value.id) }})
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