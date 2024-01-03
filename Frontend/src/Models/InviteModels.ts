import type { IUser } from '@/Models/UserModels'

interface IInvited {
    name: string
    email: string
}
interface IInvitedKeep extends IInvited {
    keepId: string
}
interface IInvitedProject extends IInvited {
    projectId: string
}

interface IProjectInvite {
    projectId: string
    users: IUser[]
}
interface IKeepInvite extends IProjectInvite {
    keepId: string
}

interface InviteResponse {
    inviteId: string
    response: boolean
}
export type { IInvitedKeep, IInvitedProject, IKeepInvite, IProjectInvite, InviteResponse }
