import type { IContact } from '@/Models/ContactModels'
import { http } from '@/config/ApiClient'
export class ContactService {
    public async AddContact(email: string): Promise<IContact | null> {
        const response: IContact = await http.post('Contact', { email })
        return response
    }
    public async GetContacts(): Promise<IContact[] | null> {
        const response: IContact[] = await http.get('Contact')
        return response
    }
}
