import type { IUser } from './UserModels'

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
    isShared: boolean
}
interface IProjectMembers {
    shareId: string
    isAccepted: boolean
    invitedUser: IUser
}
export type { IAddProject, IEditProject, IProject, IProjectMembers }
