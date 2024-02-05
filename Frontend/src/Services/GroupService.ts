import { http } from '@/config/ApiClient'
import type { IAddContactToGroup, IAddGroup, IGroup } from '@/Models/GroupModels'

export class GroupService {
    private readonly baseUrl = 'Group'
    public async AddGroup(group: IAddGroup): Promise<IGroup | null> {
        const response: IGroup = await http.post(this.baseUrl, group)
        return response
    }
    public async GetAll(): Promise<IGroup[] | null> {
        const response: IGroup[] = await http.get(this.baseUrl)
        return response
    }
    public async AddContacts(contacts: IAddContactToGroup): Promise<IGroup | null> {
        const response: IGroup = await http.post(`${this.baseUrl}/AddContacts`, contacts)
        return response
    }
    public async RemoveContact(groupId: string, contactId: string): Promise<string | null> {
        return await http.delete(`${this.baseUrl}/RemoveContact/${groupId}/${contactId}`)
    }
}
