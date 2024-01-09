import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IKeep, IAddKeep, IEditKeep } from '@/Models/KeepModels'
import { KeepService } from '@/Services/KeepService'
import { useToster } from '@/composable/useToaster'
import type { IUpdatePermission } from '@/Models/InviteModels'

const KeepStore = defineStore('KeepStore', () => {
    const keepService = new KeepService()
    const isKeepFetched: Ref<boolean> = ref(false)
    const Keeps: Ref<IKeep[]> = ref([])

    const keepTags = computed(() => {
        const tags = Keeps.value.map((x) => x.tag).filter((x) => x != '' && x != null)
        return [...new Set(tags)]
    })

    const getSingleKeep = async (id: string): Promise<IKeep | undefined> => {
        if (isKeepFetched.value) {
            return Keeps.value.find((x) => x.id == id)
        } else {
            return (await keepService.GetById(id)) ?? undefined
        }
    }

    const FindIndex = (id: string): number => Keeps.value.findIndex((x) => x.id == id)

    const AddKeep = async (addKeep: IAddKeep): Promise<any> => {
        const response = await keepService.Add(addKeep)
        if (response) {
            Keeps.value.push(response)
        }
    }

    const GetKeeps = async (projectId: string): Promise<any> => {
        const response = await keepService.GetAll(projectId)
        Keeps.value = response ?? []
        isKeepFetched.value = true
    }

    const DeleteKeep = async (keepId: string): Promise<any> => {
        const response = await keepService.Delete(keepId)
        if (response) {
            useToster({ message: 'Keep Deleted' })
            const index = Keeps.value.findIndex((x) => x.id == keepId)
            if (index !== -1) Keeps.value.splice(index, 1)
        }
    }

    const Updatekeep = async (editKeep: IEditKeep): Promise<any> => {
        const response = await keepService.Update(editKeep)
        if (response) {
            const index = FindIndex(editKeep.id)
            Keeps.value.splice(index, 1, response)
        }
    }
    const GetInvitedMembers = async (id: string): Promise<void> => {
        const keep = await getSingleKeep(id)
        if (keep) {
            keep.users = (await keepService.InvitedUser(id)) ?? []
            Keeps.value.splice(FindIndex(id), 1, keep)
        }
    }

    const RemoveUser = async (id: string, keepId: string): Promise<boolean> => {
        const res = await keepService.RemoveFromKeep(id)
        if (res) {
            const keep = Keeps.value.find(x => x.id == keepId)
            if (keep) {
                keep.users.splice(keep.users.findIndex(x => x.shareId == id), 1)
            }
            return true
        }
        return false
    }
    const UpdatePermissions = async (updatePermissions: IUpdatePermission[], keepId: string) => {
        const res = await keepService.UpdatePermission(updatePermissions);
        if (res) {
            const index = Keeps.value.findIndex(x => x.id == keepId)
            if (index != -1) {
                const users = Keeps.value[index].users
                Keeps.value[index].users = users.map(x => {
                    const update = updatePermissions.find(up => up.shareId == x.shareId)
                    if (update)
                        return { ...x, permission: update.permission }
                    return x
                })
            }
        }
    }
    return {
        Keeps,
        keepTags,
        AddKeep,
        GetKeeps,
        DeleteKeep,
        Updatekeep,
        getSingleKeep,
        GetInvitedMembers,
        RemoveUser,
        UpdatePermissions
    }
})
export { KeepStore }
