import { http } from '@/config/ApiClient'
import type { IAddComment, IComment } from '@/Models/CommentModel'
import type { IItem } from '@/Models/ItemModels'
import type { IResponse } from '@/Models/ResponseModel'

const GetAll = async (KeepId: string): Promise<IItem[] | null> => {
    const response: IItem[] = await http.get(`Item?KeepId=${KeepId}`)
    return response
}
const Get = async (itemId: string): Promise<IItem | null> => {
    const response: IItem = await http.get(`Item/${itemId}`)
    return response
}
const Insert = async (addItem: FormData): Promise<IItem | null> => {
    const response: IItem = await http.post(`Item/`, addItem)
    return response
}
const Update = async (editItem: FormData): Promise<IItem | null> => {
    const response: IItem = await http.put(`Item/`, editItem)
    return response
}
const Delete = async (itemId: string): Promise<boolean> => {
    const response: IResponse = await http.delete(`Item/${itemId}`)
    return response != null
}
const PostComment = async (comment: IAddComment): Promise<IComment | null> => {
    const response: IComment = await http.post('Item/AddComment/', { ...comment })
    return response
}
export { GetAll, Get, Insert, Update, Delete, PostComment }
