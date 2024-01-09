import type { IUser } from '@/Models/UserModels'
import type { Permission } from '@/Models/enum'

interface IAddProject {
    title: string
    description: string
    tag: string
}

interface IEditProject extends IAddProject {
    id: string
}
interface IProject extends IAddProject {
    id: string
    createdBy: string
    updatedBy: string
    createdOn: string
    updatedOn: string
    isShared: boolean,
    users: IProjectMembers[]
}
interface IProjectMembers {
    shareId: string
    isAccepted: boolean
    permission: Permission
    invitedUser: IUser
}

export type {
    IAddProject,
    IEditProject,
    IProject,
    IProjectMembers
}
