import type { IAddContact, IContact } from '@/Models/ContactModels'
import { http } from '@/config/ApiClient'
export class ContactService {
    private readonly baseURl = 'Contact'
    public async AddContact(contact: IAddContact): Promise<IContact | null> {
        const response: IContact = await http.post(this.baseURl, contact)
        return response
    }
    public async UpdateContact(contact: IContact): Promise<IContact | null> {
        return await http.put(this.baseURl, contact)
    }
    public async DeleteContact(id: string): Promise<string | null> {
        return await http.delete(`${this.baseURl}/${id}`)
    }
    public async GetContacts(): Promise<IContact[] | null> {
        const response: IContact[] = await http.get(this.baseURl)
        return response
    }
}
