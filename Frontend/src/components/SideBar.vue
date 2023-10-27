<script setup lang="ts">
import { RouterEnum } from '@/Models/enum'
import { storeToRefs } from 'pinia'
import { useRoute, useRouter } from 'vue-router'
import { Uitlity, ProjectStore, KeepStore } from '@/stores'

const { SideBarIsVisible } = storeToRefs(Uitlity())
const { ProjectTags } = storeToRefs(ProjectStore())
const { keepTags } = storeToRefs(KeepStore())
const route = useRoute()
const router = useRouter()

const filterByTag = (name: string, tag: string) => {
    router.push({ name, params: { tag } })
}
const navigate = (name: string) => {
    router.push({ name })
}
</script>
<template>
    <v-navigation-drawer location="left" color="blur-white" v-model="SideBarIsVisible">
        <v-list>
            <v-list-item role="button" prepend-icon="mdi-folder" class="my-2 sweep-to-right"
                :class="route.name == RouterEnum.PROJECT ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.PROJECT)">
                <span class="mx-2">All Projects</span>
            </v-list-item>

            <v-list-item
                v-if="route.name == RouterEnum.PROJECT || route.name == RouterEnum.SHARED || route.name == RouterEnum.PROJECT_BY_TAG"
                role="button" prepend-icon="mdi-folder-account" class="my-2 sweep-to-right" color="primary"
                :class="route.name == RouterEnum.SHARED ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.SHARED)">
                <span class="mx-2">Shared</span>
            </v-list-item>
            <v-list-item role="button" prepend-icon="mdi-contacts" class="my-2 sweep-to-right" color="primary"
                :class="route.name == RouterEnum.CONTACT ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'"
                @click="navigate(RouterEnum.CONTACT)">
                <span class="mx-2">Contact</span>
            </v-list-item>
            <v-divider></v-divider>
            <v-list-group value="tags" prepend-icon="mdi-tag">
                <template v-slot:activator="{ props }">
                    <v-list-item v-bind="props" class="sweep-to-right my-2">Tags</v-list-item>
                </template>
                <div
                    v-if="route.name == RouterEnum.PROJECT || route.name == RouterEnum.SHARED || route.name == RouterEnum.PROJECT_BY_TAG">
                    <v-list-item role="button" v-for="(tag, index) in ProjectTags" :key="index"
                        @click="filterByTag(RouterEnum.PROJECT_BY_TAG, tag)" prepend-icon="mdi-tag"
                        class="sweep-to-right my-2"
                        :class="route.params.tag == tag ? 'bg-blue-grey-lighten-3 text-white' : 'text-black'">
                        <span class="mx-2">{{ tag }}</span>
                    </v-list-item>
                </div>
                <div v-else-if="route.name == RouterEnum.KEEP || route.name == RouterEnum.KEEP_BY_TAG">
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
