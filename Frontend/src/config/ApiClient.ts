import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios'
import { storeToRefs } from 'pinia'
import { GlobalStore, UserStore } from '@/stores'
import { useToster } from '@/composable/useToaster'
import { getToken } from '@/Services/TokenService'
import { StatusType } from '@/Models/enum'
import type { IResponse } from '@/Models/ResponseModel'
const baseURL = import.meta.env.DEV ? 'https://localhost:7134/api/' : 'http://ipskeeper.project-demo.info:8073/api'
const http = axios.create({
    baseURL
})

const loadingEffect = (arg: boolean): void => {
    const { Loading } = storeToRefs(GlobalStore())
    Loading.value = arg
}
const navigateTologin = async () => {
    const { logout } = UserStore()
    logout()
}

http.interceptors.request.use((config: InternalAxiosRequestConfig<any>) => {
    loadingEffect(true)
    const token = getToken()
    if (
        !config.url?.toLowerCase().startsWith('account') &&
        !config.url?.toLowerCase().startsWith('user/checkmail')
    ) {
        if (!token) {
            navigateTologin()
        }
        config.headers.Authorization = `Bearer ${getToken()}`
    }
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
            useToster({ message: data.message, color: data.isSuccess ? 'success' : 'danger' })
        }

        return Promise.resolve(data.data)
    },
    (error: AxiosError<any>): Promise<null> => {
        loadingEffect(false)
        if (error.code == 'ERR_NETWORK') {
            useToster({ message: 'Server unavailable', color: 'danger' })
        } else if (error.response?.status == 401) {
            navigateTologin()
        } else {
            useToster({ message: error.response?.data.message, color: 'danger' })
        }
        return Promise.resolve(null)
    }
)
export { http }
