import AddItem from './AddItem.vue'
import AllItems from './AllItems.vue'
import EditItem from './EditItem.vue'
import InfoItem from './InfoItem.vue'
import DeleteItem from './DeleteItem.vue'
import UpdateStatus from './UpdateStatus.vue'
import ItemFilter from './ItemFilter.vue'
import { ItemStatus, ItemType } from '@/Models/enum'

export {
    AddItem,
    AllItems,
    EditItem,
    InfoItem,
    DeleteItem,
    UpdateStatus,
    ItemFilter
}

export type ListItemsType = {
    title: string,
    value: ItemStatus | ItemType
}

export const StatusList: ListItemsType[] = [
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

export const TypeList: ListItemsType[] = [
    {
        title: 'Ticket',
        value: ItemType.TICKET
    },
    {
        title: 'PR',
        value: ItemType.PR
    }
]
