import type { IContact } from '@/Models/ContactModels'
import { http } from '@/config/ApiClient'
export class ContactService {
    public async AddContact(userIds: string[]): Promise<IContact[] | null> {
        const response: IContact[] = await http.post('Contact', { userIds })
        return response
    }
    public async GetContacts(): Promise<IContact[] | null> {
        const response: IContact[] = await http.get('Contact')
        return response
    }
}
