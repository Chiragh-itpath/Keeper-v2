<script setup lang="ts">
import { ref, watch, type Ref, onMounted, reactive } from 'vue'
import { ProjectStore } from '@/stores'
import { RouterEnum } from '@/Models/enum'
import { useRouter } from 'vue-router'
import { EditProject, InfoProject, InviteProject } from '@/components/Project'
import { DeletePropmt, HoverEffect, NoItem } from '@/components/Custom'

import type { IProject } from '@/Models/ProjectModels'
import { useDate } from 'vuetify'

const props = withDefaults(defineProps<{
    date?: string | null | Date,
    projects: IProject[]
    tags: string[],
    isShared: boolean
}>(), {
    date: null
})

const { DeleteProject } = ProjectStore()
const router = useRouter()
const id: Ref<string> = ref('')
const dateHelper = useDate()
const ProjectsToDisplay: Ref<IProject[]> = ref(props.projects)
const selectedProject: Ref<IProject | undefined> = ref()
const visible = reactive<{
    edit: boolean,
    delete: boolean,
    info: boolean,
    invite: boolean
}>({
    edit: false,
    delete: false,
    info: false,
    invite: false
})

watch(props, () => {
    ProjectsToDisplay.value = props.projects.filter(filterFunction);
});
onMounted(() => {
    ProjectsToDisplay.value = props.projects.filter(filterFunction);
})
const deleteHandler = async (): Promise<void> => {
    await DeleteProject(id.value)
    visible.delete = false
}
const filterFunction = (project: IProject) => {

    return (
        !props.date ||
        dateHelper.format(project.createdOn, 'keyboardDate') == dateHelper.format(props.date, 'keyboardDate')) &&
        (props.tags.length === 0 || props.tags.includes(project.tag)) &&
        (!props.isShared || project.isShared);
}
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
                        <v-list>
                            <v-list-item role="button" class="ma-0 pa-0"
                                @click="() => { visible.info = true; selectedProject = project }">
                                <hover-effect icon="information-outline" icon-color="info">
                                    Info
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0" v-if="!project.isShared"
                                @click="() => { visible.invite = true; selectedProject = project }">
                                <hover-effect icon="account-plus-outline" icon-color="info">
                                    Invite
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0"
                                @click="() => { visible.edit = true; selectedProject = project }">
                                <hover-effect icon="folder-edit-outline" icon-color="edit">
                                    Edit
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0" v-if="!project.isShared"
                                @click="() => { visible.delete = true; id = project.id }">
                                <hover-effect icon="delete-outline" icon-color="delete">
                                    Delete
                                </hover-effect>
                            </v-list-item>
                        </v-list>
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
    <edit-project v-model="visible.edit" :project="selectedProject"> </edit-project>
    <delete-propmt v-model="visible.delete" @click:yes="deleteHandler" title="Delete Project">Project</delete-propmt>
    <info-project :project="selectedProject" v-model="visible.info"></info-project>
    <invite-project :project="selectedProject" v-model="visible.invite"></invite-project>
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