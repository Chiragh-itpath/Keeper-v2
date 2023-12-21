import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IKeep, IAddKeep, IEditKeep } from '@/Models/KeepModels'
import { KeepService } from '@/Services/KeepService'
import type { IProject } from '@/Models/ProjectModels'
import { useToster } from '@/composable/useToaster'
import { ProjectStore } from '.'

const KeepStore = defineStore('KeepStore', () => {
    const keepService = new KeepService()
    const { GetSingalProject } = ProjectStore()
    
    const Keeps: Ref<IKeep[]> = ref([])
    const Project: Ref<IProject | undefined> = ref()

    const keepTags = computed(() => {
        const tags = Keeps.value.map((x) => x.tag).filter((x) => x != '' && x != null)
        return [...new Set(tags)]
    })

    const FetchProject = (id: string) => {
        Project.value = GetSingalProject(id)
    }

    const getSingleKeep = (id: string): IKeep | undefined => {
        const keep = Keeps.value.find((x) => x.id == id)
        return keep
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
        if (response) {
            Keeps.value = response
        }
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

    return {
        Keeps,
        keepTags,
        AddKeep,
        GetKeeps,
        DeleteKeep,
        Updatekeep,
        getSingleKeep,
        FetchProject
    }
})
export { KeepStore }
