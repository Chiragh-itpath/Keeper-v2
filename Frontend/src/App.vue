<script setup lang="ts">
import Toster from '@/components/VToster.vue'
</script>

<template>
    <Toster />
    <v-layout>
        <router-view name="NavBar" />
        <router-view name="SideBar" />
        <v-main>
            <router-view>
            </router-view>
        </v-main>
    </v-layout>
</template>
<style>
a {
    text-decoration: none;
}

.fill {
    transform: scale(1.05);
    transition: transform 600ms ease-in;
    z-index: 9;
}
</style>