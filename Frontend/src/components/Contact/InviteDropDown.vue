<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GroupStore } from '@/stores'

const selectedItem = ref()
const { Contacts } = storeToRefs(ContactStore())
const { Groups } = storeToRefs(GroupStore())

const items = computed(() => {
    let item: any[] = []
    item = [
        ...(Groups.value.filter(x => x.contacts.length > 0).map(x => {
            return {
                props: {
                    title: x.name,
                    subtitle: 'group',
                    id: x.id
                }
            }
        })),
        ...(Contacts.value.map(x => {
            return {
                props: {
                    title: x.addedPerson.email,
                    subtitle: 'contact',
                    id: x.id
                }
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
    let emails: string[] = []
    selectedItem.value.forEach(({ props }: { props: any }) => {
        if (props.subtitle == 'contact') {
            emails.push(props.title)
        } else {
            const id = props.id
            const temp = Groups.value.find(x => x.id == id)?.contacts.map(x => x.addedPerson.email)
            emails = [...emails, ...temp!]
        }
    })
    emails = [...new Set(emails)]
    emits('update:emails', emails)
})

const emits = defineEmits<{
    (e: 'update:emails', emails: string[]): void
}>()
</script>

<template>
    <v-autocomplete :items="items" color="primary" v-model="selectedItem" multiple chips label="select contact">
        <template v-slot:chip="{ item }">
            <v-chip color="primary">{{ item.title }}</v-chip>
        </template>
        <template v-slot:item="{ item, props: itemProp }">
            <v-list-item :title="item.title" v-bind="itemProp" class="px-0 mx-0">
                <template v-slot:prepend="{ isActive }">
                    <v-checkbox :model-value="isActive" hide-details></v-checkbox>
                </template>
                <template v-slot:subtitle="{ subtitle }">
                    {{ subtitle }}
                    <span v-if="subtitle == 'group'">
                        ({{ getContactCount(item.props.id) }})
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