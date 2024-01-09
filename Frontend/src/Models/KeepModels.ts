import type { IUser } from '@/Models/UserModels'
import type { Permission } from '@/Models/enum'

interface IAddKeep {
    title: string
    projectId: string
    tag: string
}
interface IEditKeep extends IAddKeep {
    id: string
}
interface IKeep extends IAddKeep {
    id: string
    createdBy: string
    updatedBy: string
    createdOn: string
    updatedOn: string
    users: IKeepMembers[]
}
interface IKeepMembers {
    shareId: string
    isAccepted: boolean
    permission: Permission
    invitedUser: IUser
}
export type { IAddKeep, IEditKeep, IKeep, IKeepMembers }
