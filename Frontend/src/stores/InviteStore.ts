import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IInvitedKeep, IInvitedProject } from '@/Models/InviteModels'
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
    const ProjectInviteResponse = async (inviteId: string, inviteResponse: boolean): Promise<boolean> => {
        return (await inviteService.ResponseToProjectInvite({
            inviteId,
            response: inviteResponse
        })) != null
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
    const keepInviteResponse = async (inviteId: string, inviteResponse: boolean): Promise<boolean> => {
        return (await inviteService.ResponseToKeepInvite({
            inviteId,
            response: inviteResponse
        })) != null
    }
    return {
        InvitedProjectList,
        InvitedKeepList,
        InviteUsersToProject,
        FetchInvitedProjects,
        ProjectInviteResponse,
        FetchInvitedKeeps,
        InviteUsersToKeep,
        keepInviteResponse
    }
})

export { InviteStore }
