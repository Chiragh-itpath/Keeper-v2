import { ref, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import { defineStore } from 'pinia'
import { RouterEnum } from '@/Models/enum'
import type { IRegister, ILogin, IPasswordReset } from '@/Models/UserModels'
import { AccountService } from '@/Services/AccountService'
import { setToken } from '@/Services/TokenService'

const AccountStore = defineStore('AccountStore', () => {
    const router = useRouter()
    const email: Ref<string> = ref('')
    const serverOtp: Ref<string> = ref('')
    const accountService = new AccountService()
    const registerUser = async (user: IRegister): Promise<void> => {
        const response = await accountService.Signup(user)
        if (response) {
            router.push({ name: RouterEnum.LOGIN })
        }
    }
    const loginUser = async (user: ILogin): Promise<void> => {
        const response = await accountService.SignIn(user)
        if (response) {
            setToken(response.token)
            router.push({ name: RouterEnum.PROJECT })
        }
    }
    const fetchOtp = async (email: string): Promise<string> => {
        const response = await accountService.GetOtp(email)
        if (response) {
            return response
        }
        return ''
    }
    const PasswordReset = async (passwordReset: IPasswordReset): Promise<void> => {
        await accountService.ResetPassword(passwordReset)
    }
    return {
        email,
        serverOtp,
        registerUser,
        loginUser,
        fetchOtp,
        PasswordReset
    }
})

export { AccountStore }
