import axios, { AxiosError } from 'axios'
import { storeToRefs } from 'pinia'
import { GlobalStore, UserStore } from '@/stores'
import { getToken } from '@/Services/TokenService'
import { Colors, StatusType } from '@/Models/enum'
import type { IResponse } from '@/Models/ResponseModel'

const http = axios.create({
    baseURL: 'https://localhost:7134/api/'
})

const loadingEffect = (arg: boolean): void => {
    const { Loading } = storeToRefs(GlobalStore())
    Loading.value = arg
}
const navigateTologin = async () => {
    const { logout } = UserStore()
    logout()
}
const useToster = (message: string, color: Colors) => {
    const { makeToster } = GlobalStore()
    makeToster(message, color)
}
http.interceptors.request.use((config) => {
    loadingEffect(true)
    config.headers.Authorization = `Bearer ${getToken()}`
    return config
})
http.interceptors.response.use(
    (ApiResponse): any => {
        loadingEffect(false)
        const { data }: { data: IResponse } = ApiResponse
        if (data.statusName == StatusType.PASSWORD_NOT_MATCHED) {
            GlobalStore().setError('password', data.message)
        } else if (
            data.statusName == StatusType.EMAIL_EXISTS ||
            data.statusName == StatusType.EMAIL_NOT_FOUND
        ) {
            GlobalStore().setError('email', data.message)
        } else if (data.message) {
            useToster(data.message, data.isSuccess ? Colors.SUCCESS : Colors.DANGER)
        }

        return Promise.resolve(data.data)
    },
    (error: AxiosError<any>): Promise<null> => {
        loadingEffect(false)
        if (error.code == 'ERR_NETWORK') {
            useToster('Server unavailable', Colors.DANGER)
        } else if (error.response?.status == 401) {
            navigateTologin()
            useToster('Your session is over Please login again', Colors.WARNING)
        } else {
            useToster(error.response?.data.message, Colors.DANGER)
        }
        return Promise.resolve(null)
    }
)
export { http }
