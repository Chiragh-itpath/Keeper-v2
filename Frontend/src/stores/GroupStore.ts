import { defineStore } from 'pinia'
import { GroupService } from '@/Services/GroupService'
import type { IAddContact, IAddGroup, IGroup } from '@/Models/GroupModels'
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
    const AddContacts = async (contacts: IAddContact) => {
        const res = await groupService.AddContacts(contacts)
        if (res) {
            const index = Groups.value.findIndex((x) => x.id == res.id)
            Groups.value.splice(index, 1, res)
        }
    }
    return {
        Groups,
        isGroupFetched,
        AddGroup,
        GetGroups,
        AddContacts
    }
})
