import type { IUser } from "./UserModels"

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
}
interface IKeepMembers {
    shareId: string
    isAccepted: boolean
    invitedUser: IUser
}
export type { IAddKeep, IEditKeep, IKeep, IKeepMembers }
