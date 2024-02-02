import type { Permission } from '@/Models/enum'

interface IInvited {
    inviteId: string
    name: string
    email: string
}
interface IInvitedProject extends IInvited {
    projectId: string
}
interface IInvitedKeep extends IInvitedProject {
    keepId: string
}
interface IProjectInvite {
    projectId: string
    email: string
    permission: Permission
}
interface IKeepInvite extends IProjectInvite {
    keepId: string
}

interface InviteResponse {
    inviteId: string
    response: boolean
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
    IUpdatePermission
}
