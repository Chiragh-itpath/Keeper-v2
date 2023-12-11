<script setup lang="ts">
import { ref, watch, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { ProjectStore } from '@/stores'
import { RouterEnum } from '@/Models/enum'
import { useRoute, useRouter } from 'vue-router'
import EditProject from '@/components/Project/EditProject.vue'
import InviteProject from '@/components/Project/InviteProject.vue'
import InfoProject from '@/components/Project/InfoProject.vue'
import Delete from '@/components/Custom/DeletePropmt.vue'
import HoverEffect from '@/components/Custom/HoverEffect.vue'
import NoItem from "@/components/NoItem.vue"
import type { IProject } from '@/Models/ProjectModels'
import { dateHelper } from '@/Services/HelperFunction'

const props = withDefaults(defineProps<{
    date?: string | null
}>(), {
    date: null
})

const { Projects } = storeToRefs(ProjectStore())
const { DeleteProject } = ProjectStore()

const router = useRouter()
const route = useRoute()

const id: Ref<string> = ref('')
const editVisible: Ref<boolean> = ref(false)
const deleteVisible: Ref<boolean> = ref(false)
const infoVisible: Ref<boolean> = ref(false)
const inviteVisible: Ref<boolean> = ref(false)
const ProjectsToDisplay: Ref<IProject[]> = ref([])

watch([route, props], () => {
    const tag = Array.isArray(route.params.tag) ? route.params.tag.join('') : route.params.tag;

    const filterFunction = (project: IProject) => {
        const projectDate = dateHelper(project.createdOn);
        return (!props.date || projectDate === dateHelper(props.date)) &&
            (
                route.name === RouterEnum.PROJECT ||
                (route.name === RouterEnum.SHARED && project.isShared) ||
                (route.name === RouterEnum.PROJECT_BY_TAG && project.tag === tag)
            );
    };

    ProjectsToDisplay.value = Projects.value.filter(filterFunction);
});
onMounted(() => {
    ProjectsToDisplay.value = Projects.value
})
const deleteHandler = async (): Promise<void> => {
    await DeleteProject(id.value)
    deleteVisible.value = false
}
</script>
<template>
    <no-item v-if="ProjectsToDisplay.length == 0">
        No Projects found
    </no-item>
    <v-col cols="12" lg="3" md="4" sm="6" xl="2" v-for="(project, index) in ProjectsToDisplay" :key="index">
        <v-hover v-slot="{ isHovering, props }">
            <v-card elevation="10" v-bind="props" :class="isHovering ? 'fill' : ''" class="cursor-pointer"
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
                                @click="() => { infoVisible = true; id = project.id }">
                                <hover-effect icon="information-outline" icon-color="info">
                                    Info
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0" v-if="!project.isShared"
                                @click="() => { inviteVisible = true; id = project.id }">
                                <hover-effect icon="account-plus-outline" icon-color="info">
                                    Invite
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0"
                                @click="() => { editVisible = true; id = project.id }">
                                <hover-effect icon="folder-edit-outline" icon-color="edit">
                                    Edit
                                </hover-effect>
                            </v-list-item>
                            <v-list-item role="button" class="ma-0 pa-0" v-if="!project.isShared"
                                @click="() => { deleteVisible = true; id = project.id }">
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
    <edit-project v-model="editVisible" :id="id"> </edit-project>
    <delete v-model="deleteVisible" @click:yes="deleteHandler">Project</delete>
    <info-project :id="id" v-model="infoVisible"></info-project>
    <invite-project :id="id" v-model="inviteVisible"></invite-project>
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