<script setup lang="ts">
import { ref, watch, type Ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { InviteStore, GlobalStore, ProjectStore, UserStore } from '@/stores'
import { InviteDropDown } from '@/components/Contact'
import type { IUser } from '@/Models/UserModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'
import type { IInvitingUser } from '@/Models/InviteModels'
import { Permission } from '@/Models/enum'
import { permissions } from '@/data/permission'

const props = defineProps<{
    project: IProject,
}>()

const permissionForAll = ref(0)
const visible: Ref<boolean> = ref(false)
const window: Ref<'next' | 'done'> = ref('next')
const inviteUser: Ref<IInvitingUser[]> = ref([])
const selectedUsers: Ref<IUser[]> = ref([])
const InvitedUsers: Ref<IProjectMembers[]> = ref([])
const errorMessage: Ref<string> = ref('')
const { InviteUsersToProject } = InviteStore()
const { Loading } = storeToRefs(GlobalStore())
const projectStore = ProjectStore()

const handleInvite = async (): Promise<void> => {
    if (props.project) {
        await InviteUsersToProject(props.project.id, inviteUser.value)
        await projectStore.GetInvitedMembers(props.project.id)
        inviteUser.value = []
        visible.value = false
    }
}
const handleChanges = () => {
    const firstPermision = inviteUser.value[0].permission
    if (inviteUser.value.every(x => x.permission == firstPermision))
        permissionForAll.value = firstPermision
}
watch(visible, () => {
    if (!visible.value) {
        window.value = 'next'
        inviteUser.value = []
    }
    errorMessage.value = ''
})
watch(selectedUsers, () => {
    inviteUser.value = selectedUsers.value.filter(x =>
        !InvitedUsers.value.some(i => i.invitedUser.id == x.id)
    ).map(x => {
        return { ...x, permission: Permission.VIEW }
    });
    errorMessage.value = selectedUsers.value.length != inviteUser.value.length ?
        'Users already invited to this project will not be included' : '';
})
watch(permissionForAll, () => {
    inviteUser.value = inviteUser.value.map(u => { return { ...u, permission: permissionForAll.value } })
})
watch(visible, () => {
    if (!visible.value) emits('close')
})
onMounted(() => {
    InvitedUsers.value = props.project.users.filter(u => u.invitedUser.id != UserStore().User.id)
})
const emits = defineEmits<{
    (e: 'close'): void
}>()
</script>
<template>
    <v-dialog v-model="visible" max-width="700" close-on-back>
        <template v-slot:activator="{ props }">
            <v-list-item v-bind="props">
                <v-icon>mdi-account-plus-outline</v-icon>
                <span class="mx-3">Invite</span>
            </v-list-item>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Project
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-window v-model="window" :touch="false">
                    <v-window-item value="next">
                        <invite-drop-down v-model:users="selectedUsers" :error-message="errorMessage"></invite-drop-down>
                        <v-list-item v-if="InvitedUsers.length == 0" height="250"
                            class="border rounded-lg bg-grey-lighten-3 text-center text-grey">
                            No Data
                        </v-list-item>
                        <v-list v-else height="250" class="overflow-y-auto">
                            <template v-for="share in  InvitedUsers " :key="share.shareId">
                                <v-list-item class="py-2 mb-1 border rounded-lg" :title="share.invitedUser.userName"
                                    :subtitle="share.invitedUser.email">
                                    <template v-slot:prepend>
                                        <v-avatar color="primary">
                                            {{ share.invitedUser.email.slice(0, 1).toUpperCase() }}
                                        </v-avatar>
                                    </template>
                                    <template v-slot:append>
                                        <v-chip color="primary" variant="flat" class="mx-2">
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
                            <template v-for="( share, index ) in  inviteUser " :key="index">
                                <v-list-item class="py-2 mb-1 rounded-lg border" :title="share.userName"
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
            <v-card-actions>
                <v-btn v-if="window == 'done'" color="primary" variant="elevated" class="mx-4 mb-3 rounded-xl"
                    @click="window = 'next'">
                    <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-spacer></v-spacer>
                <v-btn v-if="window == 'next'" color="primary" variant="elevated" min-width="130"
                    class="mx-4 mb-3 rounded-xl" append-icon="mdi-arrow-right" :disabled="inviteUser.length == 0"
                    @click="window = 'done'; permissionForAll = 0">Next</v-btn>
                <v-btn v-if="window == 'done'" color="primary" variant="elevated" min-width="130"
                    class="mx-4 mb-3 rounded-xl" @click="handleInvite" :disabled="Loading" :loading="Loading">
                    invite
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>