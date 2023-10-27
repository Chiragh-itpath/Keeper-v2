interface IComment {
    id: string
    content: string
    user: string
    date: string
    comments: IComment[]
}
interface IAddComment {
    content: string
    itemId: string
    commentId: string | null
}
export type { IComment, IAddComment }
