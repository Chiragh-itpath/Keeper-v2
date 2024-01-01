import { http } from '@/config/ApiClient'
import type { ILogin, IPasswordReset, IRegister } from '@/Models/UserModels'
import type { IToken } from '@/Models/TokenModel'

export class AccountService {
    private readonly baseUrl = 'Account'
    public Signup = async (user: IRegister): Promise<string> => {
        const response: string = await http.post(`${this.baseUrl}/Register`, user)
        return response
    }

    public SignIn = async (user: ILogin): Promise<IToken | null> => {
        const response: IToken = await http.post(`${this.baseUrl}/Login`, user)
        return response
    }
    public GetOtp = async (email: string): Promise<string | null> => {
        const response: string = await http.get(`${this.baseUrl}/otp?email=${email}`)
        return response
    }
    public ResetPassword = async (passwordReset: IPasswordReset): Promise<void> => {
        await http.put(`${this.baseUrl}/ResetPassword`, passwordReset)
    }
}
