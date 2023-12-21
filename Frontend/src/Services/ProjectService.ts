import { http } from '@/config/ApiClient'
import type { IAddProject, IEditProject, IProject } from '@/Models/ProjectModels'

export class ProjectService {
    private readonly baseUrl: string = 'Project/'
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
}
