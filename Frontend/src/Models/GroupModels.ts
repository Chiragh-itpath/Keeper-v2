import type { IContact } from './ContactModels'

interface IGroup {
    id: string
    name: string
    userEmail: string
    contacts: IContact[]
}
interface IAddGroup {
    name: string
    contactId: string[]
}

interface IAddContactToGroup {
    groupId: string,
    contactIds: string[]
}

export type { IAddGroup, IGroup, IAddContactToGroup }
