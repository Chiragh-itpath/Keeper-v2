import { defineStore } from 'pinia'
import { GroupService } from '@/Services/GroupService'
import type { IAddContactToGroup, IAddGroup, IGroup } from '@/Models/GroupModels'
import { ref, type Ref } from 'vue'

export const GroupStore = defineStore('group', () => {
    const groupService = new GroupService()
    const Groups: Ref<IGroup[]> = ref([])
    const isGroupFetched: Ref<boolean> = ref(false)
    const AddGroup = async (group: IAddGroup): Promise<void> => {
        const res = await groupService.AddGroup(group)
        if (res) {
            Groups.value.push(res)
        }
    }
    const GetGroups = async (): Promise<void> => {
        const res = await groupService.GetAll()
        if (res) {
            isGroupFetched.value = true
            Groups.value = res
        }
    }
    const AddContacts = async (contacts: IAddContactToGroup) => {
        const res = await groupService.AddContacts(contacts)
        if (res) {
            const index = Groups.value.findIndex((x) => x.id == res.id)
            Groups.value.splice(index, 1, res)
        }
    }
    const RemoveContact = async (groupId: string, contactId: string) => {
        const res = await groupService.RemoveContact(groupId, contactId)
        if (res) {
            const groupIndex = Groups.value.findIndex(g => g.id == groupId)
            const contactIndex = Groups.value[groupIndex].contacts.findIndex(c => c.id == contactId)
            Groups.value[groupIndex].contacts.splice(contactIndex, 1)
        }
    }
    return {
        Groups,
        isGroupFetched,
        AddGroup,
        GetGroups,
        AddContacts,
        RemoveContact
    }
})
