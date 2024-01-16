import AddItem from './AddItem.vue'
import AllItems from './AllItems.vue'
import EditItem from './EditItem.vue'
import InfoItem from './InfoItem.vue'
import DeleteItem from './DeleteItem.vue'
import UpdateStatus from './UpdateStatus.vue'
import { ItemStatus } from '@/Models/enum'

export {
    AddItem,
    AllItems,
    EditItem,
    InfoItem,
    DeleteItem,
    UpdateStatus
}

export type StatusListType = {
    title: string,
    value: ItemStatus
}
export const StatusList: StatusListType[] = [
    {
        title: 'New',
        value: ItemStatus.NEW
    },
    {
        title: 'Waiting from client side',
        value: ItemStatus.PENDING
    },
    {
        title: 'Follow up',
        value: ItemStatus.FOLLOW_UP
    },
    {
        title: 'Close',
        value: ItemStatus.COMPLETED
    },
    {
        title: 'Re-Open',
        value: ItemStatus.RE_OPEN
    }
]

