import type { IUser } from '@/Models/UserModels'
import type { Permission } from '@/Models/enum'

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
interface IInvitingUser extends IUser {
    permission: Permission
}
interface IUpdatePermission {
    shareId: string
    permission: Permission
}
export type {
    IInvitedKeep,
    IInvitedProject,
    IKeepInvite,
    IProjectInvite,
    InviteResponse,
    IInvitingUser,
    IUpdatePermission
}
