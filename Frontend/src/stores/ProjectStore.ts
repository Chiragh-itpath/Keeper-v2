import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IProject, IAddProject, IEditProject } from '@/Models/ProjectModels'
import { Insert, GetAll, Update, Delete } from '@/Services/ProjectService'

const ProjectStore = defineStore('ProjectStore', () => {
    const Projects: Ref<IProject[]> = ref([])

    const ProjectTags = computed(() => {
        const tags = Projects.value.map((x) => x.tag).filter((x) => x != '' && x != null)
        return [...new Set(tags)]
    })

    const GetSingalProject = (id: string): IProject | undefined => {
        const project = Projects.value.find((x) => x.id == id)
        if (project) {
            return project
        }
    }

    const FindIndex = (id: string): number => Projects.value.findIndex((x) => x.id == id)

    const AddProject = async (addProject: IAddProject): Promise<any> => {
        const response = await Insert(addProject)
        if (response) {
            Projects.value.push(response)
        }
    }

    const GetProjects = async (): Promise<void> => {
        Projects.value = []
        const response = await GetAll()
        if (!response) {
            return
        }
        Projects.value = response
    }

    const UpdateProject = async (editProject: IEditProject): Promise<void> => {
        const response = await Update(editProject)
        if (response) {
            const index = FindIndex(editProject.id)
            Projects.value.splice(index, 1, response)
        }
    }

    const DeleteProject = async (id: string): Promise<void> => {
        const response = await Delete(id)
        if (response) {
            const index = FindIndex(id)
            if (index !== -1) Projects.value.splice(index, 1)
        }
    }

    const SingleProject = (id: string) => {
        return Projects.value.find((x) => x.id == id)
    }

    return {
        Projects,
        ProjectTags,
        AddProject,
        GetSingalProject,
        UpdateProject,
        DeleteProject,
        GetProjects,
        SingleProject
    }
})

export { ProjectStore }
