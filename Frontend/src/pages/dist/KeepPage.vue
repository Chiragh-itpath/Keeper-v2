<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch, type Ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useDate } from 'vuetify'
import { storeToRefs } from 'pinia'
import { KeepStore, ProjectStore, UserStore } from '@/stores'
import { AddKeep, KeepCard } from '@/components/keeps'
import { DatePicker, TagSelector, NoItem } from '@/components/Custom'
import { Permission, RouterEnum } from '@/Models/enum'
import type { IProject } from '@/Models/ProjectModels'
import type { IKeep } from '@/Models/KeepModels'

const route = useRoute()
const router = useRouter()
const dateHelper = useDate()
const filters = reactive<{
    date?: Date | Date[],
    selectedTags: string[]
}>({
    selectedTags: []
})
const loading: Ref<boolean> = ref(false)
const { User } = UserStore()
const project: Ref<IProject | undefined> = ref()
const { Keeps, keepTags } = storeToRefs(KeepStore())
const KeepsToDisplay: Ref<IKeep[]> = ref([])
const projectId = computed(() => {
    const id = route.params.id
    return Array.isArray(id) ? id.join('') : id
})
const hasAccess = computed((): boolean => {
    return (
        (project.value?.createdBy == User.email) ||
        (project.value?.users.some(u => u.invitedUser.id == UserStore().User.id) ?? false) ||
        (Keeps.value.length > 0)
    )
})
const canCreate = computed((): boolean => {
    if (!project.value) return false
    if (project.value.createdBy == User.email) return true
    const projectUser = project.value.users.find(u => u.invitedUser.id == User.id)
    if (!projectUser) return false
    return projectUser.permission == Permission.CREATE || projectUser.permission == Permission.ALL
})
const isSameDate = (date1: Date, date2: Date | Date[]): boolean => {
    return Array.isArray(date2) ?
        date2.map(d => dateHelper.format(d, 'keyboardDate'))
            .includes(dateHelper.format(date1, 'keyboardDate')) :
        dateHelper.format(date1, 'keyboardDate') === dateHelper.format(date2, 'keyboardDate')
}
const filterFunction = (keep: IKeep) => {
    return (
        !filters.date ||
        (Array.isArray(filters.date) && filters.date.length === 0) || isSameDate(new Date(keep.createdOn), filters.date)) &&
        (filters.selectedTags.length == 0 || filters.selectedTags.includes(keep.tag))
}
watch(filters, () => {
    KeepsToDisplay.value = Keeps.value.filter(filterFunction)
})
onMounted(async () => {
    loading.value = true
    project.value = await ProjectStore().GetSingalProject(projectId.value)
    if (!hasAccess.value) router.push({ name: RouterEnum.PROJECT })
    await KeepStore().GetKeeps(projectId.value)
    KeepsToDisplay.value = Keeps.value
    loading.value = false
})
</script>
<template>
    <v-container class="overflow-auto px-10" fluid>
        <v-row v-if="loading" class="mt-10">
            <v-col v-for="i in 16" :key="i" cols="12" sm="6" md="4" lg="3">
                <v-skeleton-loader type="text,actions"></v-skeleton-loader>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project">
            <v-col cols="12">
                <v-breadcrumbs divider="/" class="px-0" :items="[
                    {
                        title: 'Projects',
                        disabled: false,
                        to: '/Project'
                    },
                    {
                        title: 'Keep',
                        disabled: true
                    }
                ]">
                    <template v-slot:title="{ item }">
                        {{ item.title }}</template>
                </v-breadcrumbs>
            </v-col>
            <v-col class="d-flex">
                <tag-selector :items="keepTags" v-model:selected="filters.selectedTags"></tag-selector>
                <span class="mx-2"></span>
                <date-picker v-model="filters.date"></date-picker>
            </v-col>
            <v-col class="my-auto d-flex justify-end">
                <add-keep :project-id="projectId" v-if="canCreate"></add-keep>
            </v-col>
        </v-row>
        <v-row v-if="!loading && project" class="mt-10">
            <v-col cols="12" lg="3" md="4" sm="6" v-for="(keep, index) in KeepsToDisplay" :key="index">
                <keep-card :keep="keep" :project="project"></keep-card>
            </v-col>
        </v-row>
        <v-row v-if="!loading && KeepsToDisplay.length == 0" class="mt-15">
            <no-item title="No Keep Found"
                :sub-title="filters.date ? 'No keep found on this date' : 'Please click on add button to insert new record'"
                :back-button="!project"></no-item>
        </v-row>
    </v-container>
</template>
