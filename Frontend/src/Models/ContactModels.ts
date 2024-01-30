import { type IUser } from '@/Models/UserModels'
import type { Permission } from './enum'
interface IContact {
    id: string,
    addedBy: IUser
    addedPerson: IUser
}
interface IAddContact {
    user: IUser,
    projectId?: string,
    permission: Permission
}
export type { IContact, IAddContact }
