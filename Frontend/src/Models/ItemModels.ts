import type { IComment } from './CommentModel'
import type { ItemType } from '@/Models/types'
type NullableString = string | undefined

interface CommonItem {
    title: string
    description: NullableString
    url: NullableString
    type: ItemType
    number: string
    to: NullableString
    discussedBy: NullableString
    keepId: string
}
interface IAddItem extends CommonItem {
    files: any
}

interface IEditItem extends CommonItem {
    id: string
    files: any
}

interface IItem extends CommonItem {
    id: string
    createdBy: string
    createdOn: string
    updatedBy: NullableString
    updatedOn: NullableString
    files: FileModel[]
    comments: IComment[]
}
interface FileModel {
    fileName: string
    fileUrl: string
    isImage: boolean
}
export type { IItem, IAddItem, IEditItem, ItemType }
