<script setup lang="ts">
import { RouterEnum } from '@/Models/enum'
import { storeToRefs } from 'pinia'
import { useRoute, useRouter } from 'vue-router'
import { GlobalStore, ProjectStore, KeepStore } from '@/stores'
import { onMounted, reactive, watch } from 'vue';
import { useDisplay } from 'vuetify';

const { SideBarIsVisible } = storeToRefs(GlobalStore())
const { ProjectTags } = storeToRefs(ProjectStore())
const { keepTags } = storeToRefs(KeepStore())
const route = useRoute()
const router = useRouter()
const sidebarButtons = reactive<{
    shared: boolean,
    tags: boolean,
    projectTags: boolean,
    keeptags: boolean
}>({
    shared: false,
    tags: false,
    projectTags: false,
    keeptags: false
})

const filterByTag = (name: string, tag: string) => {
    router.push({ name, params: { tag } })
}
const navigate = (name: string) => {
    router.push({ name })
}
const changeSidebar = () => {
    sidebarButtons.shared = (
        route.name == RouterEnum.PROJECT ||
        route.name == RouterEnum.SHARED ||
        route.name == RouterEnum.PROJECT_BY_TAG
    )
    sidebarButtons.projectTags = (
        route.name == RouterEnum.PROJECT ||
        route.name == RouterEnum.SHARED ||
        route.name == RouterEnum.PROJECT_BY_TAG
    )
    sidebarButtons.keeptags = (
        route.name == RouterEnum.KEEP ||
        route.name == RouterEnum.KEEP_BY_TAG
    )
    sidebarButtons.tags = (
        sidebarButtons.projectTags ||
        sidebarButtons.keeptags
    )
}
watch(route, changeSidebar)
onMounted(() => {
    const { mdAndDown } = useDisplay()
    SideBarIsVisible.value = !mdAndDown.value
    changeSidebar()
})
</script>
<template>
    <v-navigation-drawer location="left" color="blur-white" v-model="SideBarIsVisible">
        <v-list>
            <v-list-item role="button" prepend-icon="mdi-folder" class="my-2 sweep-to-right"
                :class="route.name == RouterEnum.PROJECT ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.PROJECT)">
                <span class="mx-2">All Projects</span>
            </v-list-item>

            <v-list-item v-if="sidebarButtons.shared" role="button" prepend-icon="mdi-folder-account"
                class="my-2 sweep-to-right" color="primary"
                :class="route.name == RouterEnum.SHARED ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.SHARED)">
                <span class="mx-2">Shared</span>
            </v-list-item>
            <v-list-item role="button" prepend-icon="mdi-contacts" class="my-2 sweep-to-right" color="primary"
                :class="route.name == RouterEnum.CONTACT ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.CONTACT)">
                <span class="mx-2">Contact</span>
            </v-list-item>
            <v-divider v-if="sidebarButtons.tags"></v-divider>
            <v-list-group value="tags" prepend-icon="mdi-tag" v-if="sidebarButtons.tags">
                <template v-slot:activator="{ props }">
                    <v-list-item v-bind="props" class="sweep-to-right my-2">Tags</v-list-item>
                </template>
                <div v-if="sidebarButtons.projectTags">
                    <v-list-item role="button" v-for="(tag, index) in ProjectTags" :key="index"
                        @click="filterByTag(RouterEnum.PROJECT_BY_TAG, tag)" prepend-icon="mdi-tag"
                        class="sweep-to-right my-2"
                        :class="route.params.tag == tag ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'">
                        <span class="mx-2">{{ tag }}</span>
                    </v-list-item>
                </div>
                <div v-if="sidebarButtons.keeptags">
                    <v-list-item role="button" v-for="(tag, index) in keepTags" :key="index"
                        @click="filterByTag(RouterEnum.KEEP_BY_TAG, tag)" prepend-icon="mdi-tag" class="sweep-to-right my-2"
                        :class="route.params.tag == tag ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'">
                        <span class="mx-2">{{ tag }}</span>
                    </v-list-item>
                </div>
            </v-list-group>
        </v-list>
    </v-navigation-drawer>
</template>
<style >
.effect {
    background-color: #4db6ac;
}

.v-list-item__prepend {
    display: inline !important;
}

.v-list-item__prepend>.v-icon {
    opacity: 1 !important;
}

.v-list-item__prepend i {
    color: #26A69A;
}

.v-list-group {
    --prepend-width: 10px !important;
}
</style>
