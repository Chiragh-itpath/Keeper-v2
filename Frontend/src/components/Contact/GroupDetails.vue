<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { GroupStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IGroup } from '@/Models/GroupModels'
import type { IContact } from '@/Models/ContactModels'
import AllContact from '@/components/Contact/AllContacts.vue'
import ContactDropDown from '@/components/Contact/ContactDropDown.vue'


const { Groups } = storeToRefs(GroupStore())
const { AddContacts } = GroupStore()
const group: Ref<undefined | IGroup> = ref()
const visible: Ref<boolean> = ref(false)
const Contacts: Ref<IContact[]> = ref([])
const contactsToAdd: Ref = ref([])
const error: Ref<string | undefined> = ref()
const props = defineProps<{
    id: string
}>()

onMounted(() => {
    group.value = Groups.value.find(x => x.id == props.id)
    Contacts.value = group.value?.contacts ?? []
})

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
        groupId: group.value?.id ?? '',
        contactIds: contactsToAdd.value
    })
    group.value = Groups.value.find(x => x.id == props.id)
    visible.value = false
}
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
                        <contact-drop-down :contact-id="group?.contacts.map(x => x.id)" v-model:ids="contactsToAdd"
                            :errors="error">
                        </contact-drop-down>
                    </v-col>
                    <v-col cols="auto">
                        <v-btn color="primary" class="rounded-xl" @click="submitHandler">Add Person</v-btn>
                    </v-col>
                    <v-col cols="12">
                        <v-card max-height="500" elevation="0">
                            <all-contact hide-header :show-border="(group?.contacts.length ?? 0) > 0"
                                :contacts="group?.contacts" no-contacts></all-contact>
                        </v-card>
                    </v-col>
                </v-row>

            </v-card-text>
            <v-card-actions></v-card-actions>
        </v-card>
    </v-dialog>
</template>