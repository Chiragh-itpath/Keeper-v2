import { http } from '@/config/ApiClient'
import type { IAddContactToGroup, IAddGroup, IGroup } from '@/Models/GroupModels'

export class GroupService {
    public async AddGroup(group: IAddGroup): Promise<IGroup | null> {
        const response: IGroup = await http.post('Group', group)
        return response
    }
    public async GetAll(): Promise<IGroup[] | null> {
        const response: IGroup[] = await http.get('Group')
        return response
    }
    public async AddContacts(contacts: IAddContactToGroup): Promise<IGroup | null> {
        const response: IGroup = await http.post('Group/AddContacts',contacts)
        return response
    }
}
