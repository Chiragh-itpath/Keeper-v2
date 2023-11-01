import { RouterEnum } from '@/Models/enum'
import type { RouteLocationNormalized } from 'vue-router'
import { ContactStore, GroupStore, ItemStore, KeepStore, ProjectStore } from '@/stores'

const beforeResolve = async (route: RouteLocationNormalized) => {
    const id = Array.isArray(route.params.id) ? route.params.id.join('') : route.params.id
    const tag = Array.isArray(route.params.tag) ? route.params.tag.join('') : route.params.tag
    const keepId = Array.isArray(route.params.keepId)
        ? route.params.keepId.join('')
        : route.params.keepId
    const projectStore = ProjectStore()
    const keepStore = KeepStore()
    const itemStore = ItemStore()
    const contactStore = ContactStore()
    const groupStore = GroupStore()
    switch (route.name) {
        case RouterEnum.PROJECT:
            await projectStore.getProjects()
            projectStore.FetchProjects()
            break
        case RouterEnum.PROJECT_BY_TAG:
            await projectStore.getProjects()
            projectStore.FilterByTag(tag)
            break
        case RouterEnum.SHARED:
            await projectStore.FetchSharedProjects()
            break
        case RouterEnum.KEEP:
            await keepStore.GetKeeps(id)
            break
        case RouterEnum.KEEP_BY_TAG:
            keepStore.FilterByTag(tag)
            break
        case RouterEnum.ITEM:
            await itemStore.GetAllItems(keepId)
            break
        case RouterEnum.CONTACT:
            await contactStore.GetContacts()
            await groupStore.GetGroups()
            break
        case RouterEnum.HOME:
        case RouterEnum.LOGIN:
        case RouterEnum.SIGNUP:
            break
    }
}

export { beforeResolve }
