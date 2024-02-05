import type { IComment } from './CommentModel'
import type { ItemStatus, ItemType } from '@/Models/enum'


interface CommonItem {
    title: string
    type: ItemType
    description?: string
    url?: string
    number?: string
    to?: string
    discussedBy?: string
    keepId: string
}
interface IAddItem extends CommonItem {
    files?: File[]
}

interface IEditItem extends CommonItem {
    id: string
    files?: File[]
}

interface IItem extends CommonItem {
    id: string
    type: ItemType
    createdBy: string
    createdOn: string
    updatedBy?: string
    updatedOn?: string
    status: ItemStatus
    files: FileModel[]
    comments: IComment[]
}
interface FileModel {
    fileName: string
    fileUrl: string
    isImage: boolean
}
export type { IItem, IAddItem, IEditItem, ItemType }
