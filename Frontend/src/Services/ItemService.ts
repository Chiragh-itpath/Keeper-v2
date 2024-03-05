import { http } from '@/config/ApiClient'
import type { IAddComment, IComment } from '@/Models/CommentModel'
import type { IItem } from '@/Models/ItemModels'
import type { IResponse } from '@/Models/ResponseModel'

export class ItemService {
    private readonly baseUrl = 'Item'

    public GetAll = async (KeepId: string): Promise<IItem[] | null> => {
        const response: IItem[] = await http.get(`${this.baseUrl}?KeepId=${KeepId}`)
        return response
    }

    public GetById = async (itemId: string): Promise<IItem | null> => {
        const response: IItem = await http.get(`${this.baseUrl}/${itemId}`)
        return response
    }

    public Add = async (addItem: FormData): Promise<IItem | null> => {
        const response: IItem = await http.post(this.baseUrl, addItem)
        return response
    }

    public Update = async (editItem: FormData): Promise<IItem | null> => {
        const response: IItem = await http.put(this.baseUrl, editItem)
        return response
    }

    public Delete = async (itemId: string): Promise<boolean> => {
        const response: IResponse = await http.delete(`${this.baseUrl}/${itemId}`)
        return response != null
    }

    public PostComment = async (comment: IAddComment): Promise<IComment | null> => {
        const response: IComment = await http.post(`${this.baseUrl}/AddComment/`, comment)
        return response
    }

    public UpdateStatus = async (itemId: string, statusId: string): Promise<IItem | null> => {
        return await http.put(`${this.baseUrl}/UpdateStatus`, {
            id: itemId,
            statusId
        })
    }
    public getAllComments = async (itemid: string): Promise<IComment[] | null> => {
        const response: IComment[] | null = await http.get(`${this.baseUrl}/Comments/${itemid}`)
        return response
    }
}
