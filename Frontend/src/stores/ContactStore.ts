import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import { ContactService } from '@/Services/ContactService'
import type { IAddContact, IContact } from '@/Models/ContactModels'

export const ContactStore = defineStore('contact', () => {
    const contactService = new ContactService()
    const Contacts: Ref<IContact[]> = ref([])
    const isContactFetched: Ref<boolean> = ref(false);
    const AddContact = async (contact: IAddContact): Promise<boolean> => {
        const res = await contactService.AddContact(contact)
        if (res) {
            Contacts.value.push(res)
            return true
        }
        return false
    }
    const GetContacts = async () => {
        const res = await contactService.GetContacts()
        if (res) {
            isContactFetched.value = true;
            Contacts.value = res
        }
    }
    const UpdateContact = async (contact: IContact): Promise<boolean> => {
        const res = await contactService.UpdateContact(contact);
        if (res) {
            Contacts.value.splice(Contacts.value.findIndex(x => x.id == contact.id), 1, res)
            return true
        }
        return false
    }
    const DeleteContact = async (id: string) => {
        if (await contactService.DeleteContact(id)) {
            Contacts.value.splice(Contacts.value.findIndex(x => x.id == id), 1)
        }
    }
    return {
        Contacts,
        isContactFetched,
        AddContact,
        GetContacts,
        UpdateContact,
        DeleteContact
    }
})
