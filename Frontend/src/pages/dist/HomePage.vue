<script setup lang="ts">
import homeimg from '@/assests/images/Home.jpg'
import SiteIcon from '@/assests/images/favicon.svg'
import { RouterEnum } from '@/Models/enum'
import { useDisplay } from 'vuetify'
const { smAndDown, xxl } = useDisplay()
</script>
<template>
    <v-row class="flex-wrap-reverse ma-0" :class="!smAndDown ? 'h-screen' : ''">
        <v-col cols="12" md="6" class="pa-0">
            <v-sheet :height="smAndDown ? '400' : '100%'" class="w-100 d-flex align-center">
                <v-row justify="center">
                    <v-col cols="12" class="d-flex justify-center">
                        <v-img :src="SiteIcon" width="100" height="100"></v-img>
                    </v-col>
                    <v-col cols="12" class="text-center text-xl-h1 text-sm-h3 text-h4" :class="xxl ? 'my-2' : ''">
                        <span class="text-primary">Create. </span>
                        <span class="">Organize.</span>
                    </v-col>
                    <v-col cols="12" class="text-center text-xl-h1 text-sm-h3 text-h4" :class="xxl ? 'my-2' : ''">
                        <span>Share. </span>
                        <span class="text-primary">Easy.</span>
                    </v-col>
                    <v-col cols="10" class="text-center text-xl-h4 text-body-2" :class="xxl ? 'my-2' : ''">
                        Keeper is the best place to note down quick thoughts or to save longer
                        notes filled with checklist, images ,web links, docs.
                    </v-col>
                    <v-col cols="8" xl="4" lg="6" md="9" sm="5" :class="xxl ? 'my-2 mt-5' : ''">
                        <div class="d-flex justify-space-around">
                            <router-link :to="{ name: RouterEnum.LOGIN }">
                                <v-btn variant="outlined" color="primary" class="rounded-xl text-xl-h6 ma-0"
                                    :width="xxl ? 150 : 110" :height="xxl ? 50 : 40">
                                    Login
                                </v-btn>
                            </router-link>
                            <router-link :to="{ name: RouterEnum.SIGNUP }">
                                <v-btn variant="elevated" color="primary" class="rounded-xl text-xl-h6 ma-0"
                                    :width="xxl ? 150 : 110" :height="xxl ? 50 : 40">
                                    Signup
                                </v-btn>
                            </router-link>
                        </div>
                    </v-col>
                </v-row>
            </v-sheet>
        </v-col>
        <v-col cols="12" md="6" class="pa-0">
            <v-img :src="homeimg" class="w-100 h-100" :aspect-ratio="0" cover></v-img>
        </v-col>
    </v-row>
</template>
<style scoped>
.h-screen {
    height: 100vh !important;
}
</style>