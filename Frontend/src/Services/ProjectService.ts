import { http } from '@/config/ApiClient'
import type { IAddProject, IEditProject, IProject, IProjectMembers } from '@/Models/ProjectModels'
import type { IUpdatePermission } from '@/Models/InviteModels'

export class ProjectService {
    private readonly baseUrl: string = 'Project'
    public Add = async (Project: IAddProject): Promise<IProject | null> => {
        const response: IProject = await http.post(this.baseUrl, Project)
        return response
    }
    public GetById = async (ProjectId: string): Promise<IProject | null> => {
        const response: IProject = await http.get(`${this.baseUrl}/${ProjectId}`)
        return response
    }
    public GetAll = async (): Promise<IProject[] | null> => {
        const response: IProject[] = await http.get(this.baseUrl)
        return response
    }
    public Update = async (Project: IEditProject): Promise<IProject | null> => {
        const response: IProject = await http.put(this.baseUrl, Project)
        return response
    }
    public Delete = async (ProjectId: string): Promise<boolean> => {
        const res = await http.delete(`${this.baseUrl}/${ProjectId}`)
        return res != null
    }
    public InvitedUser = async (projectId: string): Promise<IProjectMembers[] | null> => {
        const res: IProjectMembers[] = await http.get(`${this.baseUrl}/Users/${projectId}`)
        return res
    }
    public RemoveFromProject = async (id: string) => {
        return await http.delete(`${this.baseUrl}/Remove/${id}`)
    }
    public UpdatePermission = async (updatePermissions: IUpdatePermission[]) => {
        return await http.put(`${this.baseUrl}/UpdatePermission`, updatePermissions)
    }
}
