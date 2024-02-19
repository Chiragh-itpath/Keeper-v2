<script setup lang="ts">
import { type Ref, ref, watch, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { InviteDropDown } from '@/components/Contact/'
import { GlobalStore, InviteStore, KeepStore } from '@/stores'
import type { IKeep, IKeepMembers } from '@/Models/KeepModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'
import { Permission } from '@/Models/enum'
import { permissions } from '@/data/permission'
import type { IKeepInvite } from '@/Models/InviteModels'

const props = defineProps<{
    keep: IKeep
    project: IProject
}>()

const permissionForAll = ref(0)
const visible: Ref<boolean> = ref(false)
const window: Ref<'next' | 'done'> = ref('next')
const inviteUser: Ref<IKeepInvite[]> = ref([])
const selectedUser: Ref<string[]> = ref([])
const keepInvitedUsers: Ref<IKeepMembers[]> = ref([])
const projectInvitedUsers: Ref<IProjectMembers[]> = ref([])
const errorMessage: Ref<string> = ref('')
const { Loading } = storeToRefs(GlobalStore())
const handleInvite = async (): Promise<void> => {
    if (props.keep) {
        await InviteStore().InviteUsersToKeep(inviteUser.value)
        await KeepStore().GetInvitedMembers(props.keep.id)
        visible.value = false
        inviteUser.value = []
    }
}
const handleChanges = () => {
    const firstPermision = inviteUser.value[0].permission
    if (inviteUser.value.every(x => x.permission == firstPermision))
        permissionForAll.value = firstPermision
}
watch(visible, () => {
    if (!visible.value) {
        emits('close')
        window.value = 'next'
        inviteUser.value = []
    }
})
watch(selectedUser, () => {
    inviteUser.value = selectedUser.value.filter(u =>
        !keepInvitedUsers.value.some(s => s.invitedUser.email == u)
    ).filter(u =>
        !projectInvitedUsers.value.some(s => s.invitedUser.email == u)
    ).map(x => {
        return {
            email: x,
            keepId: props.keep.id,
            projectId: props.project.id,
            permission: Permission.VIEW
        }
    })
    errorMessage.value = inviteUser.value.length != selectedUser.value.length ?
        'Users already invited to this project or keep will not be included' : ''
})
watch(permissionForAll, () => {
    inviteUser.value = inviteUser.value.map(x => { return { ...x, permission: permissionForAll.value } })
})
const emits = defineEmits<{
    (e: 'close'): void
}>()
onMounted(() => {
    keepInvitedUsers.value = props.keep.users
    projectInvitedUsers.value = props.project.users
})
</script>
<template>
    <v-dialog v-model="visible" max-width="700" close-on-back>
        <template v-slot:activator="{ props }">
            <slot :activator="props"></slot>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Keep
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-window v-model="window" :touch="false">
                    <v-window-item value="next">
                        <invite-drop-down v-model:users="selectedUser" :error-message="errorMessage"></invite-drop-down>
                        <v-list-item v-if="keepInvitedUsers.length == 0" height="250"
                            class="border rounded-lg text-center text-grey">
                            No Data
                        </v-list-item>
                        <v-list v-else height="250" class="overflow-y-auto">
                            <template v-for="share in keepInvitedUsers" :key="share.shareId">
                                <v-list-item class="py-2 mb-1 border rounded-lg" :title="share.invitedUser.userName"
                                    :subtitle="share.invitedUser.email">
                                    <template v-slot:prepend>
                                        <v-avatar color="primary">
                                            {{ share.invitedUser.email.slice(0, 1).toUpperCase() }}
                                        </v-avatar>
                                    </template>
                                    <template v-slot:append>
                                        <v-chip class="me-4" color="primary" variant="flat">
                                            {{ permissions[share.permission].title }}
                                        </v-chip>
                                        <v-icon v-if="share.isAccepted" color="success">
                                            mdi-account-check
                                        </v-icon>
                                        <v-icon v-else color="danger">
                                            mdi-account-clock
                                        </v-icon>
                                    </template>
                                </v-list-item>
                            </template>
                        </v-list>

                    </v-window-item>
                    <v-window-item value="done">
                        <v-list-item class="py-2 px-1 mb-3 rounded-lg" title="Permission">
                            <template v-slot:append>
                                <v-sheet width="120">
                                    <v-select color="primary" class="w-100" density="compact" hide-details
                                        :items="permissions" v-model="permissionForAll">
                                    </v-select>
                                </v-sheet>
                            </template>
                        </v-list-item>
                        <v-list height="270" class="overflow-auto">
                            <template v-for="(share, index) in inviteUser" :key="index">
                                <v-list-item class="py-2 mb-1 rounded-lg border" :title="share.email"
                                    :subtitle="share.email">
                                    <template v-slot:prepend>
                                        <v-avatar color="primary">
                                            {{ share.email.slice(0, 1) }}
                                        </v-avatar>
                                    </template>
                                    <template v-slot:append>
                                        <v-sheet width="120">
                                            <v-select color="primary" class="w-100" density="compact" hide-details
                                                :items="permissions" v-model="share.permission"
                                                @update:model-value="handleChanges">
                                            </v-select>
                                        </v-sheet>
                                    </template>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-window-item>
                </v-window>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn v-if="window == 'done'" color="primary" variant="elevated" class="mx-4 mb-3 rounded-xl"
                    @click="window = 'next'">
                    <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-spacer></v-spacer>
                <v-btn v-if="window == 'next'" color="primary" variant="elevated" min-width="130"
                    class="mx-4 mb-3 rounded-xl" append-icon="mdi-arrow-right" :disabled="inviteUser.length == 0"
                    @click="window = 'done'; permissionForAll = 0">Next</v-btn>
                <v-btn v-if="window == 'done'" color="primary" variant="elevated" min-width="130"
                    class="mx-4 mb-3 rounded-xl" @click="handleInvite" :disabled="Loading || inviteUser.length == 0"
                    :loading="Loading">
                    invite
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
