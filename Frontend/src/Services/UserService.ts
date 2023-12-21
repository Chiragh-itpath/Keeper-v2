import { http } from '@/config/ApiClient'
import type { IUser } from '@/Models/UserModels'

export class UserService {
    private readonly baseUrl = 'User'
    public GetByEmail = async (email: string): Promise<IUser | null> => {
        const response: IUser = await http.get(`${this.baseUrl}/CheckMail?email=${email}`)
        return response
    }
    public GetMyProfile = async (): Promise<IUser | null> => {
        const response: IUser = await http.get(`${this.baseUrl}/Me`)
        return response
    }
    public EmailSearch = async (email: string): Promise<string[] | null> => {
        const response: string[] = await http.get(`${this.baseUrl}/EmailSearch?email=${email}`)
        return response
    }
}
