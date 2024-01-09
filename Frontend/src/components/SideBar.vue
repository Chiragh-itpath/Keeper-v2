<script setup lang="ts">
import { onMounted } from 'vue'
import { useDisplay } from 'vuetify'
import { storeToRefs } from 'pinia'
import { RouterEnum } from '@/Models/enum'
import { GlobalStore } from '@/stores'
import { useRoute } from 'vue-router'

const { SideBarIsVisible } = storeToRefs(GlobalStore())
const route = useRoute()
const { mdAndDown } = useDisplay()
onMounted(() => {
    SideBarIsVisible.value = !mdAndDown.value
})
</script>
<template>
    <v-navigation-drawer location="left" v-model="SideBarIsVisible">
        <v-list nav density="comfortable" variant="flat" class="px-0" lines="one" color="primary">
            <v-list-item role="button" value="project" :to="{ name: RouterEnum.PROJECT }"
                :active="route.name == RouterEnum.PROJECT">
                <template v-slot:prepend="{ isActive }">
                    <v-icon :color="isActive ? 'white' : 'primary'">mdi-folder</v-icon>
                </template>
                Projects
            </v-list-item>
            <v-list-item role="button" value="contact" :to="{ name: RouterEnum.CONTACT }"
                :active="route.name == RouterEnum.CONTACT">
                <template v-slot:prepend="{ isActive }">
                    <v-icon :color="isActive ? 'white' : 'primary'">mdi-contacts</v-icon>
                </template>
                Contacts
            </v-list-item>
        </v-list>
    </v-navigation-drawer>
</template>