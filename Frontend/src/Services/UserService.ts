import { http } from '@/config/ApiClient'
import type { IUser } from '@/Models/UserModels'

const GetByEmail = async (email: string): Promise<IUser | null> => {
    const response: IUser = await http.get(`User/CheckMail?email=${email}`)
    return response
}
const GetMyProfile = async (): Promise<IUser | null> => {
    const response: IUser = await http.get('User/Me')
    return response
}
const EmailSearch = async (email: string): Promise<string[] | null> => {
    const response: string[] = await http.get(`User/EmailSearch?email=${email}`)
    return response
}

export { GetByEmail, GetMyProfile, EmailSearch }
