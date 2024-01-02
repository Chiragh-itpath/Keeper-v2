<script setup lang="ts">
import { computed, ref, watch, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GroupStore } from '@/stores'
import type { IUser } from '@/Models/UserModels';
defineProps<{
    errorMessage?: string
}>()
const selectedItem: Ref<any[]> = ref([])
const { Contacts, isContactFetched } = storeToRefs(ContactStore())
const { Groups, isGroupFetched } = storeToRefs(GroupStore())
const menu: Ref<boolean> = ref(false)
const items = computed(() => {
    let item: any[] = []
    item = [
        ...(Groups.value.filter(x => x.contacts.length > 0).map(x => {
            return {
                title: x.name,
                subtitle: 'group',
                value: { id: x.id, type: 'group' }
            }
        })),
        ...(Contacts.value.map(x => {
            return {
                title: x.addedPerson.email,
                subtitle: 'contact',
                value: { id: x.id, type: 'contact' }
            }
        }))
    ]
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
    if(!isContactFetched.value) {
        await ContactStore().GetContacts()
    }
    if(!isGroupFetched.value) {
        await GroupStore().GetGroups()
    }
})
</script>

<template>
    <v-autocomplete :items="items" color="primary" v-model="selectedItem" multiple chips label="select contact"
        v-model:menu="menu" :error-messages="errorMessage">
        <template v-slot:chip="{ item }">
            <v-chip color="primary">{{ item.title }}</v-chip>
        </template>
        <template v-slot:item="{ item, props: itemProp }">
            <v-list-item :title="item.title" :subtitle="item.raw.subtitle" v-bind="itemProp" class="px-0 mx-0">
                <template v-slot:prepend="{ isActive }">
                    <v-checkbox :model-value="isActive" hide-details></v-checkbox>
                </template>
                <template v-slot:subtitle="{ subtitle }">
                    {{ subtitle }}
                    <span v-if="subtitle == 'group'">
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