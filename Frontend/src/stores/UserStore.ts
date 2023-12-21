import { reactive } from 'vue'
import { defineStore } from 'pinia'
import { useRouter } from 'vue-router'
import type { IUser } from '@/Models/UserModels'
import { UserService } from '@/Services/UserService'
import { removeToken } from '@/Services/TokenService'
import { RouterEnum } from '@/Models/enum'

const UserStore = defineStore('user', () => {
    const router = useRouter()

    const User = reactive<IUser>({
        id: '',
        email: '',
        userName: '',
        contact: ''
    })
    const userService = new UserService()

    const myProfile = async (): Promise<void> => {
        const res = await userService.GetMyProfile()
        if (res) {
            User.id = res.id
            User.email = res.email
            User.contact = res.contact
            User.userName = res.userName
        }
    }
    const CheckEmail = async (email: string): Promise<IUser | undefined> => {
        const response = await userService.GetByEmail(email)
        if (response) {
            return response
        }
        return undefined
    }

    const SearchEmail = async (email: string): Promise<string[]> => {
        const emails = await userService.EmailSearch(email)
        return emails ?? []
    }

    const logout = () => {
        removeToken()
        router.push({ name: RouterEnum.HOME })
    }

    return {
        User,
        logout,
        CheckEmail,
        myProfile,
        SearchEmail
    }
})
export { UserStore }
