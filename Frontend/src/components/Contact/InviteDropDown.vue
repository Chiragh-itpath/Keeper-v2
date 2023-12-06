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
                    title: x.email,
                    subtitle: 'contact',
                    id: x.id
                }
            }
        }))
    ]
    return item
})

watch(selectedItem, () => {
    let emails: string[] = []
    selectedItem.value.forEach(({ props }: { props: any }) => {
        if (props.subtitle == 'contact') {
            emails.push(props.title)
        } else {
            const id = props.id
            const temp = Groups.value.find(x => x.id == id)?.contacts.map(x => x.email)
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
    <v-autocomplete :items="items" color="primary" v-model="selectedItem" multiple chips>
        <template v-slot:chip="{ item }">
            <v-chip color="primary">{{ item.title }}</v-chip>
        </template>
        <template v-slot:selection="{ item, index }">
            <v-chip color="primary" v-if="index < 2">
                <span>{{ item.title }}</span>
            </v-chip>
            <span v-if="index === 2" class="text-grey text-caption align-self-center">
                (+{{ selectedItem.length - 2 }} others)
            </span>
        </template>
    </v-autocomplete>
</template>