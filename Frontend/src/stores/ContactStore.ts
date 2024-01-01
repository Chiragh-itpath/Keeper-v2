import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import { ContactService } from '@/Services/ContactService'
import type { IContact } from '@/Models/ContactModels'

export const ContactStore = defineStore('contact', () => {
    const contactService = new ContactService()
    const Contacts: Ref<IContact[]> = ref([])
    const isContactFetched:Ref<boolean> = ref(false);
    const AddContact = async (ids: string[]) => {
        const res = await contactService.AddContact(ids)
        if (res) {
            Contacts.value = [...Contacts.value, ...res]
        }
    }
    const GetContacts = async () => {
        const res = await contactService.GetContacts()
        if (res) {
            isContactFetched.value = true;
            Contacts.value = res
        }
    }
    return {
        Contacts,
        isContactFetched,
        AddContact,
        GetContacts
    }
})
