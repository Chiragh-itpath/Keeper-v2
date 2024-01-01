import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IProject, IAddProject, IEditProject, IProjectMembers } from '@/Models/ProjectModels'
import { ProjectService } from '@/Services/ProjectService'
import { useToster } from '@/composable/useToaster'

const ProjectStore = defineStore('ProjectStore', () => {
    const projectService = new ProjectService()
    const Projects: Ref<IProject[]> = ref([])
    const isProjectFetched: Ref<boolean> = ref(false)
    const ProjectTags = computed(() => {
        const tags = Projects.value.map((x) => x.tag).filter((x) => x != '' && x != null)
        return [...new Set(tags)]
    })

    const FindIndex = (id: string): number => Projects.value.findIndex((x) => x.id == id)

    const GetSingalProject = async (id: string): Promise<IProject | undefined> => {
        if (isProjectFetched.value) return Projects.value.find((x) => x.id == id)
        else {
            return (await projectService.GetById(id)) ?? undefined
        }
    }

    const AddProject = async (addProject: IAddProject): Promise<any> => {
        const response = await projectService.Add(addProject)
        if (response) {
            Projects.value.push(response)
        }
    }

    const GetProjects = async (): Promise<void> => {
        Projects.value = (await projectService.GetAll()) ?? []
        isProjectFetched.value = true
    }
    const UpdateProject = async (editProject: IEditProject): Promise<void> => {
        const response = await projectService.Update(editProject)
        if (response) {
            const index = FindIndex(editProject.id)
            Projects.value.splice(index, 1, response)
        }
    }

    const DeleteProject = async (id: string): Promise<void> => {
        const response = await projectService.Delete(id)
        if (response) {
            useToster({ message: 'Project Deleted' })
            const index = FindIndex(id)
            if (index !== -1) Projects.value.splice(index, 1)
        }
    }

    const GetInvitedMembers = async (id: string): Promise<IProjectMembers[]> =>
        (await projectService.InvitedUser(id)) ?? []

    const RemoveUser = async (id: string): Promise<boolean> =>
        (await projectService.RemoveFromProject(id)) != null

    return {
        Projects,
        ProjectTags,
        AddProject,
        GetSingalProject,
        UpdateProject,
        DeleteProject,
        GetProjects,
        GetInvitedMembers,
        RemoveUser
    }
})

export { ProjectStore }
