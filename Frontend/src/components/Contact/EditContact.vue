<script setup lang="ts">
import { type Ref, ref, reactive, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { ContactStore, GlobalStore } from '@/stores'
import { TextField } from '@/components/Custom'
import type { IContact } from '@/Models/ContactModels'

const { contact, contacts } = defineProps<{
    contact: IContact
    contacts: IContact[]
}>()
const { errors, Loading } = storeToRefs(GlobalStore())
const visible: Ref<boolean> = ref(false)
const form = ref()
const oldEmail = contact.email;
const editContact = reactive<IContact>({
    id: contact.id,
    firstName: contact.firstName,
    lastName: contact.lastName,
    email: contact.email
})
const updateHandler = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const index1 = contacts.findIndex(x => x.email == editContact.email)
    const index2 = contacts.findIndex(x => x.email == oldEmail)
    if (index1 != -1) {
        if (index1 != index2) {
            errors.value.email = 'Contact already exist'
            return
        }
    }
    if (await ContactStore().UpdateContact(editContact))
        visible.value = false
}
watch(visible, (newVal: boolean) => {
    if (newVal) {
        errors.value = {}
    }
})
</script>
<template>
    <v-dialog v-model="visible" max-width="700">
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Edit contact
                <v-btn @click="visible = !visible" icon="mdi-close" class="float-end  pa-0" color="primary"
                    density="compact" variant="flat" />
            </v-card-title>
            <v-card-text>
                <v-form ref="form">
                    <v-row>
                        <v-col cols="12" sm="6">
                            <text-field label="First name" v-model="editContact.firstName" is-required />
                        </v-col>
                        <v-col cols="12" sm="6">
                            <text-field label="Last name" v-model="editContact.lastName" is-required />
                        </v-col>
                        <v-col cols="12">
                            <text-field label="Email" v-model="editContact.email" is-required is-email
                                :error-messages="errors.email" />
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn text="Update" color="primary" class="rounded-xl ma-2" variant="elevated" width="120"
                    @click="updateHandler" :loading="Loading" :disabled="Loading" />
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>