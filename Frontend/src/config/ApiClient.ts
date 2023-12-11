import axios, { AxiosError } from 'axios'
import { storeToRefs } from 'pinia'
import { Uitlity, UserStore } from '@/stores'
import { getToken } from '@/Services/TokenService'
import { Colors } from '@/Models/enum'

const http = axios.create({
    baseURL: 'https://localhost:7134/api/'
})

const loadingEffect = (arg: boolean): void => {
    const { Loading } = storeToRefs(Uitlity())
    Loading.value = arg
}
const navigateTologin = async () => {
    const { logout } = UserStore()
    logout()
}
const useToster = (message: string, color: Colors) => {
    const { makeToster } = Uitlity()
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
        const { data }: { data: any } = ApiResponse
        if (data.isSuccess) {
            if (data.message) {
                useToster(data.message, Colors.SUCCESS)
            }
        } else {
            if (data.message) {
                useToster(data.message, Colors.DANGER)
            }
        }
        return Promise.resolve(data.data)
    },
    (error: AxiosError<any>): Promise<null> => {
        loadingEffect(false)
        if (error.code == 'ERR_NETWORK') {
            useToster('Server unavailable', Colors.DANGER)
        }
        else if (error.response?.status == 401) {
            navigateTologin()
            useToster('Your session is over Please login again', Colors.WARNING)
        } else {
            useToster(error.response?.data.message, Colors.DANGER)
        }
        return Promise.resolve(null)
    }
)
export { http }
