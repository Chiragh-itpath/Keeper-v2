import type { IAddContact, IContact } from '@/Models/ContactModels'
import { http } from '@/config/ApiClient'
export class ContactService {
    public async AddContact(contact: IAddContact): Promise<IContact | null> {
        const response: IContact = await http.post('Contact', contact)
        return response
    }
    public async GetContacts(): Promise<IContact[] | null> {
        const response: IContact[] = await http.get('Contact')
        return response
    }
}
