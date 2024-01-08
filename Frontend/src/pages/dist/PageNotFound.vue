<script setup lang="ts">
import { useDisplay } from 'vuetify';

const { height } = useDisplay()
</script>

<template>
    <v-card class="bg-white overflow-y-auto v-row align-center justify-center ma-0" :height="height">
        <v-col cols="12" lg="6">
            <!-- <v-img class="img-fluid mx-auto" width="auto"
                src="data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjg1IiBoZWlnaHQ9IjQ0MCIgdmlld0JveD0iMCAwIDY4NSA0NDAiIGZpbGw9Im5vbmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+CjxwYXRoIGZpbGwtcnVsZT0iZXZlbm9kZCIgY2xpcC1ydWxlPSJldmVub2RkIiBkPSJNMC4xNDQ1NTkgMjA5Ljk2MkwxMzcuMzAzIDI0NC42NTFMMTI2LjE1OCAyODguNzE4TDE5OS40MiAzMDcuMjQ2TDIxMC41NjUgMjYzLjE3OUwyMzguNDM4IDI3MC4yMjhDMjc2LjY1NSAzNDguOTc3IDM0MS4yNzUgMzc3LjI5NCA0MTAuNjYyIDM0Ny45NEM0MjEuNTgxIDM0My4zMjEgNDMxLjM4IDMzNy43OTEgNDQwLjAyNiAzMzEuNDI5TDQzOS4wNjcgMzg2Ljc5NUw0NzEuMDg5IDQzOS4xNEw1OTEuNzc1IDM2NS4zMTFMNjE1LjQ5NSA0MDQuMDg2TDY3OS45NTggMzY0LjY1MUw2NTYuMjM4IDMyNS44NzZMNjg0LjgzNCAzMDguMzgyTDY1Mi44MTEgMjU2LjAzN0w2MjQuMjE1IDI3My41M0w1MjguMTQ4IDExNi40OTRMNDY3LjUzNiAxNTMuNTczQzQyOS45MzggNjguMDgxIDM2NC4wMTkgMzguNjk0NyAyOTIuMTcxIDY5LjI0MTlDMjc3LjIyOSA3NS41OTQ1IDI2NC4zNDggODMuNTMyNCAyNTMuNjMgOTIuODk5OUwyNzAuNzQ3IDI1LjIxNjlMMTczLjggMC42OTgyNTFMMTUuMTkwMSAxNTAuNDcyTDAuMTQ0NTU5IDIwOS45NjJaTTQ2Ny41MzYgMTUzLjU3M0w0NDIuODQ0IDE2OC42NzhMNDQwLjAyNiAzMzEuNDI5QzQ4OS4wNzggMjk1LjMzMiA1MDAuOTU3IDIzMi40MzggNDY5LjAzMyAxNTcuMDQxQzQ2OC41MzkgMTU1Ljg3NSA0NjguMDQgMTU0LjcxOSA0NjcuNTM2IDE1My41NzNaTTI1My42MyA5Mi44OTk5TDIyNS42MSAyMDMuNjg5TDI1OC4xMSAyMTEuOTA4TDI0My4wNjQgMjcxLjM5OUwyMzguNDM4IDI3MC4yMjhDMjM2LjQ0OSAyNjYuMTMgMjM0LjUzMiAyNjEuODk2IDIzMi42ODkgMjU3LjUyNkMyMDMuMDIyIDE4Ny4xNzEgMjEyLjAyNCAxMjkuMjYzIDI1My42MyA5Mi44OTk5Wk0xNTQuMDAxIDE4NS41NzhMOTAuNjU1IDE2OS41NThMOTEuMjEyMiAxNjcuMzU0TDE3Ni44NzQgODUuODdMMTc5LjA3NyA4Ni40MjczTDE1NC4wMDEgMTg1LjU3OFpNNTYxLjIwNyAzMTIuMDc2TDUwNS40NjggMzQ2LjE3NEw1MDQuMjgyIDM0NC4yMzVMNTA1Ljg5NyAyMjYuMDE5TDUwNy44MzYgMjI0LjgzM0w1NjEuMjA3IDMxMi4wNzZaTTMwNy45ODQgMjI1LjUxM0MzMzEuNjc3IDI4MS45NjUgMzYwLjQ0MyAyOTkuNTI1IDM4NS41NDEgMjg4Ljg1NEM0MTAuNjQgMjc4LjE4MyA0MTcuODIyIDI0NS4zMzkgMzkzLjczNyAxODkuMDU0QzM3MC4yMDggMTM0LjA3NiAzNDIuMTY4IDExNy4xMzQgMzE3LjA3IDEyNy44MDVDMjkxLjk3MSAxMzguNDc2IDI4NC44NDcgMTcwLjM2OCAzMDcuOTg0IDIyNS41MTNaIiBmaWxsPSJ3aGl0ZSIvPgo8L3N2Zz4K"
                alt="Figure" /> -->
            <v-img height="500" src="https://cdn.dribbble.com/users/285475/screenshots/2083086/dribbble_1.gif"></v-img>
        </v-col>
        <v-col cols="12" lg="6" class="text-center text-lg-start">
            <h1>
                <span class="text-red" style="font-size: 4rem">Opps!</span> Page not
                found.
            </h1>
            <p>The page you’re looking for doesn’t exist.</p>
            <router-link to="/">
                <v-btn color="primary" class="my-2 text-white" variant="outlined" :rounded="true">
                    Home
                </v-btn>
            </router-link>
        </v-col>
    </v-card>
</template>
