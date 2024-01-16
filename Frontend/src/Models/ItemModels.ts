import type { IComment } from './CommentModel'
import type { ItemType } from '@/Models/types'
import type { ItemStatus } from '@/Models/enum'
type NullableString = string | undefined

interface CommonItem {
    title: string
    description: NullableString
    url: NullableString
    number: string
    to: NullableString
    discussedBy: NullableString
    keepId: string
}
interface IAddItem extends CommonItem {
    type: ItemType
    files: any
}

interface IEditItem extends CommonItem {
    type: ItemType
    id: string
    files: any
}

interface IItem extends CommonItem {
    id: string
    type: number
    createdBy: string
    createdOn: string
    updatedBy: NullableString
    updatedOn: NullableString
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
