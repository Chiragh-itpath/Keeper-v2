<script setup lang="ts">
import { ref, watch, type Ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useDate } from 'vuetify'
import { UserStore } from '@/stores'
import { Permission, RouterEnum } from '@/Models/enum'
import { EditProject, InfoProject, InviteProject, ManageUser, DeleteProject } from '@/components/Project'
import { NoItem } from '@/components/Custom'
import type { IProject } from '@/Models/ProjectModels'

const props = withDefaults(defineProps<{
    date?: string | null | Date,
    projects: IProject[]
    tags: string[],
    isShared: boolean
}>(), {
    date: null
})

const router = useRouter()
const dateHelper = useDate()
const ProjectsToDisplay: Ref<IProject[]> = ref(props.projects)
const { User } = UserStore()
const filterFunction = (project: IProject) => {
    return (
        !props.date ||
        dateHelper.format(project.createdOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate')) &&
        (props.tags.length === 0 || props.tags.includes(project.tag)) &&
        (!props.isShared || project.isShared)
}
const canEdit = (index: number): boolean => {
    if (ProjectsToDisplay.value.length < index) return false
    const project = ProjectsToDisplay.value[index]
    if (project.createdBy == User.email) return true
    const projectUser = project.users.find(u => u.invitedUser.id == User.id)
    if (!projectUser) return false
    return projectUser.permission == Permission.EDIT || projectUser.permission == Permission.ALL
}
watch(props, () => {
    ProjectsToDisplay.value = props.projects.filter(filterFunction)
})
onMounted(() => {
    ProjectsToDisplay.value = props.projects.filter(filterFunction)
})
</script>
<template>
    <no-item v-if="ProjectsToDisplay.length == 0" title="No Project Found"
        :sub-title="date ? 'There is no project on this date' : props.isShared ? 'There is no shared projects' : 'Please click on add button to insert new record'">
    </no-item>
    <v-col cols="12" lg="3" md="4" sm="6" xl="2" v-for="( project, index ) in  ProjectsToDisplay " :key="index">
        <v-hover v-slot="{ isHovering, props }">
            <v-card :elevation="isHovering ? 8 : 3" v-bind="props" class="cursor-pointer"
                @click="router.push({ name: RouterEnum.KEEP, params: { id: project.id } })">
                <v-card-title class="bg-primary d-flex">
                    <span class="text-truncate">{{ project.title }}</span>
                    <v-spacer></v-spacer>
                    <v-menu location="right" width="250">
                        <template v-slot:activator="{ props: menu }">
                            <v-icon v-bind="menu" color="grey-lighten-4">mdi-dots-vertical</v-icon>
                        </template>
                        <template v-slot:default="{ isActive }">
                            <v-list>
                                <info-project :project="project" @close="isActive.value = false" />
                                <invite-project :project="project" v-if="!project.isShared"
                                    @close="isActive.value = false" />
                                <manage-user :project="project" v-if="!project.isShared" @close="isActive.value = false" />
                                <edit-project :project="project" v-if="canEdit(index)" @close="isActive.value = false" />
                                <delete-project :id="project.id" v-if="!project.isShared" @close="isActive.value = false" />
                            </v-list>
                        </template>
                    </v-menu>
                </v-card-title>
                <v-card-text>
                    <v-sheet height="100" class="mt-3 ">
                        <span v-if="project.description">
                            {{ project.description }}
                        </span>
                        <span v-else class="text-grey">
                            No Description
                        </span>
                    </v-sheet>
                </v-card-text>
                <v-card-actions class="border">
                    <v-chip v-if="project.tag" class="bg-primary">#{{ project.tag }}</v-chip>
                    <v-spacer></v-spacer>
                    <v-icon color="primary" v-if="project.isShared">mdi-account-multiple-outline</v-icon>
                </v-card-actions>
            </v-card>
        </v-hover>
    </v-col>
</template>
<style scoped>
.v-chip {
    cursor: pointer;
}

.v-sheet {
    overflow: hidden;
    text-overflow: ellipsis;
}
</style>