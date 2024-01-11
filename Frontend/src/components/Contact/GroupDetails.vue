<script setup lang="ts">
import { computed, ref, watch, type Ref } from 'vue'
import { ContactStore, GlobalStore, GroupStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IGroup } from '@/Models/GroupModels'
import { AllContacts } from '@/components/Contact/'


const { AddContacts } = GroupStore()
const { Contacts } = storeToRefs(ContactStore())
const { Loading } = storeToRefs(GlobalStore())
const visible: Ref<boolean> = ref(false)
const contactsToAdd: Ref<string[]> = ref([])
const error: Ref<string | undefined> = ref()
const props = defineProps<{
    group: IGroup
}>()

const clickHandler = () => {
    visible.value = true
}

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
            title: c.addedPerson.email,
            value: c.id
        }
    })
})
watch(visible, () => [
    contactsToAdd.value = []
])
</script>
<template>
    <slot :onclick="clickHandler"></slot>
    <v-dialog v-model="visible" max-width="700">
        <v-card>
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
                            v-model:model-value="contactsToAdd" hide-selected>
                            <template v-slot:chip="{ item, props }">
                                <v-chip color="primary" v-bind="props">
                                    {{ item.title }}
                                </v-chip>
                            </template>
                            <template v-slot:no-data>
                                <v-list-item>
                                    No more contacts left to show
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
                        <v-card max-height="500" elevation="0">
                            <all-contacts hide-header :show-border="(group.contacts.length ?? 0) > 0"
                                :contacts="group.contacts" no-contacts></all-contacts>
                        </v-card>
                    </v-col>
                </v-row>

            </v-card-text>
            <v-card-actions></v-card-actions>
        </v-card>
    </v-dialog>
</template>