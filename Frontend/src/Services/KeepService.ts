import { http } from '@/config/ApiClient'
import type { IAddKeep, IEditKeep, IKeep, IKeepMembers } from '@/Models/KeepModels'

export class KeepService {
    private readonly baseUrl = 'keep'

    public GetAll = async (projectId: string): Promise<IKeep[] | null> => {
        const response: IKeep[] = await http.get(`${this.baseUrl}?ProjectId=${projectId}`)
        return response
    }

    public GetById = async (keepId: string): Promise<IKeep | null> => {
        const response: IKeep = await http.get(`${this.baseUrl}/${keepId}`)
        return response
    }

    public Add = async (addKeep: IAddKeep): Promise<IKeep | null> => {
        const response: IKeep = await http.post(`${this.baseUrl}`, addKeep)
        return response
    }

    public Update = async (editKeep: IEditKeep): Promise<IKeep | null> => {
        const response: IKeep = await http.put('${this.baseUrl}', editKeep)
        return response
    }
    public Delete = async (keepId: string): Promise<boolean> =>
        (await http.delete(`${this.baseUrl}/${keepId}`)) != null

    public InvitedUser = async (id: string): Promise<IKeepMembers[] | null> => {
        const response: IKeepMembers[] = await http.get(`${this.baseUrl}/Users/${id}`)
        return response
    }
    public RemoveFromKeep = async (id: string): Promise<void> =>
        await http.delete(`${this.baseUrl}/Remove/${id}`)
}
