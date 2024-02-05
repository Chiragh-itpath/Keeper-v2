import { computed, ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IProject, IAddProject, IEditProject } from '@/Models/ProjectModels'
import { ProjectService } from '@/Services/ProjectService'
import { useToster } from '@/composable/useToaster'
import type { IUpdatePermission } from '@/Models/InviteModels'

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
        const projectOnLocal = Projects.value.find(x => x.id == id)
        if (projectOnLocal) {
            return projectOnLocal
        } else {
            const projectOnServer = await projectService.GetById(id)
            if (projectOnServer) {
                Projects.value.push(projectOnServer)
                return projectOnServer
            }
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

    const GetInvitedMembers = async (id: string): Promise<void> => {
        const project = await GetSingalProject(id)
        if (project) {
            project.users = (await projectService.InvitedUser(id)) ?? []
            Projects.value.splice(FindIndex(id), 1, project)
        }
    }

    const RemoveUser = async (id: string, projectId: string): Promise<boolean> => {
        const res = await projectService.RemoveFromProject(id)
        if (res) {
            const project = Projects.value.find(x => x.id == projectId)
            if (project) {
                project.users.splice(project.users.findIndex(x => x.shareId == id), 1)
            }
            return true
        }
        return false
    }

    const UpdatePermissions = async (updatePermissions: IUpdatePermission[], projectId: string) => {
        const res = await projectService.UpdatePermission(updatePermissions)
        if (res) {
            const index = Projects.value.findIndex(x => x.id == projectId)
            if (index != -1) {
                const users = Projects.value[index].users
                Projects.value[index].users = users.map(x => {
                    const update = updatePermissions.find(up => up.shareId == x.shareId)
                    if (update) {
                        return { ...x, permission: update.permission }
                    }
                    return x
                })
            }
        }
    }
    return {
        Projects,
        ProjectTags,
        isProjectFetched,
        AddProject,
        GetSingalProject,
        UpdateProject,
        DeleteProject,
        GetProjects,
        GetInvitedMembers,
        RemoveUser,
        UpdatePermissions
    }
})

export { ProjectStore }
