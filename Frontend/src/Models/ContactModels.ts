import { type IUser } from '@/Models/UserModels'
interface IContact extends IUser {
    userId: string
}

export type { IContact }
