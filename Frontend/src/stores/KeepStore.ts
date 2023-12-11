import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IKeep, IAddKeep, IEditKeep } from '@/Models/KeepModels'
import { Insert, GetAll, Update, Delete } from '@/Services/KeepService'

const KeepStore = defineStore('KeepStore', () => {
    const Keeps: Ref<IKeep[]> = ref([])

    const keepTags = computed(() => {
        const tags = Keeps.value.map((x) => x.tag).filter(x => x != '' && x != null)
        return [...new Set(tags)]
    })

    const getSingleKeep = (id: string): IKeep | undefined => {
        const keep = Keeps.value.find((x) => x.id == id)
        return keep
    }

    const FindIndex = (id: string): number => Keeps.value.findIndex((x) => x.id == id)

    const AddKeep = async (addKeep: IAddKeep): Promise<any> => {
        const response = await Insert(addKeep)
        if (response) {
            Keeps.value.push(response)
        }
    }
    const getSingle = (id: string) => {
        return Keeps.value.find((x) => x.id == id)
    }
    const GetKeeps = async (projectId: string): Promise<any> => {
        const response = await GetAll(projectId)
        if (response) {
            Keeps.value = response
        }
    }
    const DeleteKeep = async (keepId: string): Promise<any> => {
        const response = await Delete(keepId)
        if (response) {
            const index = Keeps.value.findIndex((x) => x.id == keepId)
            if (index !== -1) Keeps.value.splice(index, 1)
        }
    }
    const Updatekeep = async (editKeep: IEditKeep): Promise<any> => {
        const response = await Update(editKeep)
        if (response) {
            const index = FindIndex(editKeep.id)
            Keeps.value.splice(index, 1, response)
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
        getSingle
    }
})
export { KeepStore }
