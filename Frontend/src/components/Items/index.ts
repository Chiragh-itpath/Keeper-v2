import AddItem from './AddItem.vue'
import EditItem from './EditItem.vue'
import InfoItem from './InfoItem.vue'
import DeleteItem from './DeleteItem.vue'
import UpdateStatus from './UpdateStatus.vue'
import ItemFilter from './ItemFilter.vue'
import ItemCard from './ItemCard.vue'
import ItemGrid from './ItemGrid.vue'
import ImagePreview from './ImagePreview.vue'
import { ItemStatus, ItemType } from '@/Models/enum'

export {
    AddItem,
    EditItem,
    InfoItem,
    DeleteItem,
    UpdateStatus,
    ItemFilter,
    ItemCard,
    ItemGrid,
    ImagePreview
}

export type ListItemsType = {
    title: string,
    subtitle?: string,
    value: ItemStatus | ItemType
}

export const StatusList: ListItemsType[] = [
    {
        title: 'New',
        value: ItemStatus.NEW
    },
    {
        title: 'Waiting from client side',
        subtitle: 'Waiting',
        value: ItemStatus.WAITING
    },
    {
        title: 'Pending',
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
    },
    {
        title: 'Mail',
        value: ItemType.MAIL
    },
    {
        title: 'Summary Mail',
        value: ItemType.SUMMARY_MAIL
    },
    {
        title: 'Custom',
        value: ItemType.CUSTOM
    }
]
