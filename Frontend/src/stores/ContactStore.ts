import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import { ContactService } from '@/Services/ContactService'
import type { IAddContact, IContact } from '@/Models/ContactModels'

export const ContactStore = defineStore('contact', () => {
    const contactService = new ContactService()
    const Contacts: Ref<IContact[]> = ref([])
    const isContactFetched: Ref<boolean> = ref(false);
    const AddContact = async (contacts: IAddContact[]) => {
        contacts.forEach(async (contact) => {
            const res = await contactService.AddContact(contact)
            if (res) {
                Contacts.value.push(res)
            }
        })
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
