import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IInvitedKeep, IInvitedProject } from '@/Models/InviteModels'
import type { Collaborators } from '@/Models/Collaborators'
import { InviteService } from '@/Services/InviteService'
import type { IUser } from '@/Models/UserModels'

const InviteStore = defineStore('inviteStore', () => {
    const inviteService = new InviteService()
    const InvitedProjectList: Ref<IInvitedProject[]> = ref([])
    const InvitedKeepList: Ref<IInvitedKeep[]> = ref([])
    const InviteUsersToProject = async (projectId: string, users: IUser[]) => {
        await inviteService.InviteToProject({
            projectId,
            users
        })
    }
    const FetchInvitedProjects = async () => {
        const response = await inviteService.GetAllInvitedProject()
        if (response) {
            InvitedProjectList.value = response
        }
    }
    const ProjectInviteResponse = async (inviteId: string, inviteResponse: boolean) => {
        await inviteService.ResponseToProjectInvite({
            inviteId,
            response: inviteResponse
        })
    }
    const FetchInvitedKeeps = async () => {
        const response = await inviteService.GetAllInvitedKeeps()
        if (response) {
            InvitedKeepList.value = response
        }
    }
    const InviteUsersToKeep = async (keepId: string, projectId: string, users: IUser[]) => {
        await inviteService.InviteToKeep({
            keepId,
            projectId,
            users
        })
    }
    const keepInviteResponse = async (inviteId: string, inviteResponse: boolean) => {
        await inviteService.ResponseToKeepInvite({
            inviteId,
            response: inviteResponse
        })
    }
    const GetProjectCollaborators = async (projectId: string): Promise<Collaborators[]> => {
        const response = await inviteService.ProjectCollaborators(projectId)
        if (response) {
            return response
        }
        return []
    }
    const GetKeepCollaborators = async (keepId: string): Promise<Collaborators[]> => {
        const response = await inviteService.KeepCollaborators(keepId)
        if (response) {
            return response
        }
        return []
    }
    return {
        InvitedProjectList,
        InvitedKeepList,
        InviteUsersToProject,
        FetchInvitedProjects,
        ProjectInviteResponse,
        FetchInvitedKeeps,
        InviteUsersToKeep,
        keepInviteResponse,
        GetProjectCollaborators,
        GetKeepCollaborators
    }
})

export { InviteStore }
