<script setup lang="ts">
import Loader from '@/components/Custom/LoaderComponent.vue'
import Toster from '@/components/VToster.vue'
</script>

<template>
    <Toster />
    <loader />
    <v-layout>
        <router-view name="NavBar" />
        <router-view name="SideBar" />
        <v-main class="overflow-hidden">
            <router-view>
            </router-view>
        </v-main>
    </v-layout>
</template>
<style>
a {
    text-decoration: none;
}
.h-screen {
    height: calc(100vh - 80px) !important;
}
.fill {
    transform: scale(1.05);
    transition: transform 600ms ease-in;
    z-index: 9;
}
</style>