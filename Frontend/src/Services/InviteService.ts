import { http } from '@/config/ApiClient'
import type { Collaborators } from '@/Models/Collaborators'
import type {
    IInvitedKeep,
    IInvitedProject,
    IKeepInvite,
    IProjectInvite,
    InviteResponse
} from '@/Models/InviteModels'

export class InviteService {
    private readonly baseUrl = 'Invite'
    public InviteToProject = async (projectInvite: IProjectInvite): Promise<void> => {
        await http.post(`${this.baseUrl}/InviteToProject`, projectInvite)
    }
    public ResponseToProjectInvite = async (inviteResponse: InviteResponse): Promise<void> => {
        await http.post(`${this.baseUrl}/ProjectInviteResponse`, inviteResponse)
    }
    public GetAllInvitedProject = async (): Promise<IInvitedProject[] | null> => {
        const response: IInvitedProject[] = await http.get(`${this.baseUrl}/AllInvitedProjects`)
        return response
    }
    public InviteToKeep = async (keepInvite: IKeepInvite): Promise<void> => {
        await http.post(`${this.baseUrl}/InviteToKeep`, keepInvite)
    }
    public ResponseToKeepInvite = async (inviteResponse: InviteResponse): Promise<void> => {
        await http.post(`${this.baseUrl}/keepInviteResponse`, inviteResponse)
    }
    public GetAllInvitedKeeps = async (): Promise<IInvitedKeep[] | null> => {
        const response: IInvitedKeep[] = await http.get(`${this.baseUrl}/InvitedKeeps`)
        return response
    }
    public ProjectCollaborators = async (projectId: string): Promise<Collaborators[] | null> => {
        const response: Collaborators[] = await http.get(
            `${this.baseUrl}/ProjectCollaborators?ProjectId=${projectId}`
        )
        return response
    }
    public KeepCollaborators = async (keepId: string): Promise<Collaborators[] | null> => {
        const response: Collaborators[] = await http.get(
            `${this.baseUrl}/KeepCollaborators?keepId=${keepId}`
        )
        return response
    }
}
