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
    <v-navigation-drawer location="left" :model-value="SideBarIsVisible" class="bg-grey-lighten-3">
        <v-list nav density="comfortable">
            <v-list-item role="button" variant="flat" class="my-3" value="project" color="primary"
                :to="{ name: RouterEnum.PROJECT }" :active="route.name == RouterEnum.PROJECT">
                <template v-slot:prepend="{ isActive }">
                    <v-icon :color="isActive ? 'white' : 'primary'">mdi-folder</v-icon>
                </template>
                Projects
            </v-list-item>
            <v-list-item role="button" variant="flat" class="my-3" value="contact" color="primary"
                :to="{ name: RouterEnum.CONTACT }" :active="route.name == RouterEnum.CONTACT">
                <template v-slot:prepend="{ isActive }">
                    <v-icon :color="isActive ? 'white' : 'primary'">mdi-contacts</v-icon>
                </template>
                Contacts
            </v-list-item>
        </v-list>
    </v-navigation-drawer>
</template>