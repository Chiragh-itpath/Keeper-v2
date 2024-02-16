<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useRoute } from 'vue-router'
import { GlobalStore, InviteStore, ProjectStore } from '@/stores'
import type { IInvitedKeep, IInvitedProject } from '@/Models/InviteModels'
import { RouterEnum } from '@/Models/enum'

const menu: Ref<boolean> = ref(false)
const route = useRoute()
const { NotificationCount, Loading } = storeToRefs(GlobalStore())
const { InvitedProjectList, InvitedKeepList } = storeToRefs(InviteStore())
const {
    ProjectInviteResponse,
    keepInviteResponse,
    FetchInvitedProjects,
    FetchInvitedKeeps
} = InviteStore()
const { GetSingalProject } = ProjectStore()
const selectedIndex: Ref<undefined | string> = ref()

const onProjectInviteReponse = async (invite: IInvitedProject, response: boolean): Promise<void> => {
    if (Loading.value) return
    selectedIndex.value = invite.inviteId
    const res = await ProjectInviteResponse(invite.inviteId, response)
    if (res) {
        if (route.name == RouterEnum.PROJECT && response) {
            await GetSingalProject(invite.projectId)
        }
        InvitedProjectList.value = InvitedProjectList.value.filter(x => x.inviteId != invite.inviteId)
        menu.value = false
    }
    selectedIndex.value = undefined
}
const onKeepInviteResponse = async (invite: IInvitedKeep, response: boolean) => {
    if (Loading.value) return
    selectedIndex.value = invite.inviteId
    const res = await keepInviteResponse(invite.inviteId, true)
    if (res) {
        if (route.name == RouterEnum.PROJECT && response) {
            await GetSingalProject(invite.projectId)
        }
        InvitedKeepList.value = InvitedKeepList.value.filter(x => x.inviteId != invite.inviteId)
        menu.value = false
    }
    selectedIndex.value = undefined
}
onMounted(async (): Promise<void> => {
    await FetchInvitedProjects()
    await FetchInvitedKeeps()
})
</script>
<template>
    <v-menu location="bottom" transition="scale-transition" v-model="menu" :close-on-content-click="false">
        <template v-slot:activator="{ props }">
            <v-badge :content="NotificationCount" color="red" v-if="NotificationCount > 0">
                <v-icon size="30" color="primary" v-bind="props">mdi-bell</v-icon>
            </v-badge>
            <v-icon size="30" color="primary" v-bind="props" v-else>mdi-bell</v-icon>
        </template>
        <v-card max-width="450">
            <v-card-text class="ma-0 pa-0 px-3">
                <v-card v-for="(notification, index) in InvitedProjectList" :key="index" class="my-3 custom-card rounded-lg"
                    elevation="0">
                    <v-card-text class="ma-0 pa-0 px-3 pt-2">
                        <div>
                            <span class="font-weight-bold">
                                {{ notification.email }}
                            </span>
                            has invited you to project
                            <span class="font-weight-bold">
                                '{{ notification.name }}'
                            </span>
                        </div>
                    </v-card-text>
                    <v-card-actions class="d-flex justify-end ma-0 pa-0 px-3 ">
                        <v-progress-circular indeterminate color="primary"
                            v-if="Loading && selectedIndex == notification.inviteId"></v-progress-circular>
                        <template v-else>
                            <v-icon color="green" role="button" size="x-large" :class="Loading ? 'v-btn--disabled' : ''"
                                class="mx-4" @click="onProjectInviteReponse(notification, true)">
                                mdi-check-circle-outline
                            </v-icon>
                            <v-icon color="red" role="button" size="x-large" :class="Loading ? 'v-btn--disabled' : ''"
                                @click="onProjectInviteReponse(notification, false)">
                                mdi-close-circle-outline
                            </v-icon>
                        </template>
                    </v-card-actions>
                </v-card>
                <v-card v-for="(notification, index) in InvitedKeepList" :key="index" class="my-3 custom-card rounded-lg"
                    elevation="0">
                    <v-card-text class="ma-0 pa-0 px-3 pt-2">
                        <div>
                            <span class="font-weight-bold">
                                {{ notification.email }}
                            </span>
                            has invited you to keep
                            <span class="font-weight-bold">
                                '{{ notification.name }}'
                            </span>
                        </div>
                    </v-card-text>
                    <v-card-actions class="d-flex justify-end ma-0 pa-0 px-3 ">
                        <v-progress-circular indeterminate color="primary"
                            v-if="Loading && selectedIndex == notification.inviteId"></v-progress-circular>
                        <template v-else>
                            <v-icon color="green" role="button" size="x-large" :class="Loading ? 'v-btn--disabled' : ''"
                                class="mx-4" @click="onKeepInviteResponse(notification, true)">
                                mdi-check-circle-outline
                            </v-icon>
                            <v-icon color="red" role="button" size="x-large" :class="Loading ? 'v-btn--disabled' : ''"
                                @click="onKeepInviteResponse(notification, false)">
                                mdi-close-circle-outline
                            </v-icon>
                        </template>
                    </v-card-actions>
                </v-card>

                <v-card v-if="InvitedProjectList.length == 0 && InvitedKeepList.length == 0" class="text-center text-grey"
                    min-width="300" elevation="0">
                    <v-card-text>
                        No new invitation
                    </v-card-text>
                </v-card>
            </v-card-text>
        </v-card>
    </v-menu>
</template>
<style scoped>
.custom-card {
    border-left: 5px solid #26A69A;
    background: #ECEFF1;
}

.v-icon--size-x-large {
    font-size: calc(var(--v-icon-size-multiplier) * 2rem);
}
</style>