<script setup lang="ts">
import { computed, ref, watch, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GlobalStore, GroupStore } from '@/stores'
import type { IGroup } from '@/Models/GroupModels'
import { DeletePrompt } from '@/components/Custom'


const { AddContacts, RemoveContact } = GroupStore()
const { Contacts } = storeToRefs(ContactStore())
const { Loading } = storeToRefs(GlobalStore())
const visible: Ref<boolean> = ref(false)
const contactsToAdd: Ref<string[]> = ref([])
const error: Ref<string | undefined> = ref()
const props = defineProps<{
    group: IGroup
}>()

const submitHandler = async () => {
    error.value = undefined
    if (contactsToAdd.value.length == 0) {
        error.value = 'Please Select Email'
        return;
    }
    await AddContacts({
        groupId: props.group.id,
        contactIds: contactsToAdd.value
    })
    visible.value = false
}
const items = computed(() => {
    return Contacts.value.filter(x => {
        return !props.group.contacts.find(c => c.id == x.id)
    }).map(c => {
        return {
            title: c.email,
            value: c.id
        }
    })
})
watch(visible, () => [
    contactsToAdd.value = []
])
</script>
<template>
    <v-dialog v-model="visible" max-width="700">
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card max-height="500">
            <v-card-title class="bg-primary text-center">
                <span>Group Contacts</span>
                <span class="float-end">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-row justify="end">
                    <v-col cols="12">
                        <v-select :items="items" color="primary" multiple chips closable-chips
                            v-model:model-value="contactsToAdd" hide-selected hide-details>
                            <template v-slot:chip="{ item, props }">
                                <v-chip color="primary" v-bind="props">
                                    {{ item.title }}
                                </v-chip>
                            </template>
                            <template v-slot:no-data>
                                <v-list-item class="text-center text-grey">
                                    No data
                                </v-list-item>
                            </template>
                        </v-select>
                    </v-col>
                    <v-col cols="auto">
                        <v-btn color="primary" class="rounded-xl" @click="submitHandler" :loading="Loading"
                            :disabled="Loading">
                            Add Person
                            <span v-if="contactsToAdd.length > 0" class="ms-2">
                                ({{ contactsToAdd.length }})
                            </span>
                        </v-btn>
                    </v-col>
                    <v-col cols="12">
                        <v-list max-height="300">
                            <v-list-item class="text-grey text-center border rounded-lg" v-if="group.contacts.length == 0">
                                No Contacts
                            </v-list-item>
                            <template v-for="(contact, index) in group.contacts" :key="index">
                                <v-list-item :title="`${contact.firstName} ${contact.lastName}`" :subtitle="contact.email"
                                    class="border rounded-lg py-2 mb-2">
                                    <template v-slot:prepend>
                                        <v-avatar :text="contact.email.charAt(0)" class="bg-primary text-capitalize" />
                                    </template>
                                    <template v-slot:append>
                                        <delete-prompt v-slot="{ props }" width="500" title="Remove Contact"
                                            subtitle="Are you sure you want to remove contact from group?"
                                            @click:yes="RemoveContact(group.id, contact.id)">
                                            <v-icon icon="mdi-delete" color="danger" v-bind="props" />
                                        </delete-prompt>
                                    </template>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions></v-card-actions>
        </v-card>
    </v-dialog>
</template>