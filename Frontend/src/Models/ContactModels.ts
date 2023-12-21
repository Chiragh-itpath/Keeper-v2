import { type IUser } from '@/Models/UserModels'
interface IContact {
    id: string,
    addedBy: IUser
    addedPerson: IUser
}

export type { IContact }
