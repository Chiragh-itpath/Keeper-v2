<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { InviteDropDown } from '@/components/Contact/'
import { DeletePropmt } from '@/components/Custom/'
import { GlobalStore, InviteStore, KeepStore } from '@/stores'
import type { IUser } from '@/Models/UserModels'
import type { IKeep, IKeepMembers } from '@/Models/KeepModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'
import type { IInvitingUser } from '@/Models/InviteModels'
import { Permission } from '@/Models/enum'
import { onMounted } from 'vue'

const props = defineProps<{
    keep: IKeep
    project: IProject
}>()
const permissions = [
    {
        title: 'View',
        value: 0
    }, {
        title: 'Edit',
        value: 1
    },
    {
        title: 'Create',
        value: 2
    },
    {
        title: 'All',
        value: 3
    }
]
const permissionForAll = ref(0)
const visible: Ref<boolean> = ref(false)
const window: Ref<'next' | 'done'> = ref('next')
const inviteUser: Ref<IInvitingUser[]> = ref([])
const selectedUser: Ref<IUser[]> = ref([])
const keepInvitedUsers: Ref<IKeepMembers[]> = ref([])
const projectInvitedUsers: Ref<IProjectMembers[]> = ref([])
const errorMessage: Ref<string> = ref('')
const shareId: Ref<string> = ref('')
const deleteVisible: Ref<boolean> = ref(false)
const { Loading } = storeToRefs(GlobalStore())

const handleInvite = async (): Promise<void> => {
    if (props.keep) {
        await InviteStore().InviteUsersToKeep(props.keep.id, props.project.id, inviteUser.value)
        await KeepStore().GetInvitedMembers(props.keep.id)
        visible.value = false
        inviteUser.value = []
    }
}
const handleRemove = async (shareId: string): Promise<void> => {
    const res: boolean = await KeepStore().RemoveUser(shareId)
    if (res) {
        keepInvitedUsers.value.splice(keepInvitedUsers.value.findIndex(x => x.shareId == shareId), 1)
    }
    deleteVisible.value = false
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
        !keepInvitedUsers.value.some(s => s.invitedUser.id == u.id)
    ).filter(u =>
        !projectInvitedUsers.value.some(s => s.invitedUser.id == u.id)
    ).map(x => {
        return { ...x, permission: Permission.VIEW }
    })
    errorMessage.value = inviteUser.value.length != selectedUser.value.length ?
        'Users already invited in project or keep will be skipped' : ''
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
    <v-dialog transition="scale-transition" v-model="visible" max-width="700" close-on-back>
        <template v-slot:activator="{ props }">
            <v-list-item role="button" v-bind="props">
                <v-icon>mdi-account-plus-outline</v-icon>
                <span class="mx-3">Invite</span>
            </v-list-item>
        </template>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Keep
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-window v-model="window" :touch="false" style="height: 350px;">
                    <v-window-item value="next">
                        <invite-drop-down v-model:users="selectedUser" :error-message="errorMessage"></invite-drop-down>
                        <div class="mt-3">Members with access</div>
                        <v-card elevation="0" max-height="300">
                            <v-list>
                                <v-list-item v-if="keepInvitedUsers.filter(x => x.isAccepted).length == 0" height="100"
                                    class="border rounded-lg bg-grey-lighten-3 text-center text-grey">
                                    No Data
                                </v-list-item>
                                <template v-for="share in keepInvitedUsers.filter(x => x.isAccepted)" :key="share.shareId">
                                    <v-list-item class="py-2 mb-1 border rounded-lg" :title="share.invitedUser.userName"
                                        :subtitle="share.invitedUser.email">
                                        <template v-slot:prepend>
                                            <v-avatar color="primary">
                                                {{ share.invitedUser.email.slice(0, 1).toUpperCase() }}
                                            </v-avatar>
                                        </template>
                                        <template v-slot:append>
                                            <v-chip class="me-4" color="primary" variant="outlined">
                                                {{ permissions[share.permission].title }}
                                            </v-chip>
                                            <v-icon color="danger"
                                                @click="() => { deleteVisible = true; shareId = share.shareId }">
                                                mdi-delete
                                            </v-icon>
                                        </template>
                                    </v-list-item>
                                </template>
                            </v-list>
                        </v-card>
                        <span>Pending Invitation</span>
                        <v-card elevation="0" max-height="300">
                            <v-list>
                                <v-list-item v-if="keepInvitedUsers.filter(x => !x.isAccepted).length == 0" height="100"
                                    class="border rounded-lg bg-grey-lighten-3 text-center text-grey">
                                    No Data
                                </v-list-item>
                                <template v-for="share in keepInvitedUsers.filter(x => !x.isAccepted)" :key="share.shareId">
                                    <v-list-item class="py-2 mb-1 border rounded-lg" :title="share.invitedUser.userName"
                                        :subtitle="share.invitedUser.email">
                                        <template v-slot:prepend>
                                            <v-avatar color="primary">
                                                {{ share.invitedUser.email.slice(0, 1).toUpperCase() }}
                                            </v-avatar>
                                        </template>
                                        <template v-slot:append>
                                            <v-chip class="me-4" color="primary" variant="outlined">
                                                {{ permissions[share.permission].title }}
                                            </v-chip>
                                            <v-icon color="danger"
                                                @click="() => { deleteVisible = true; shareId = share.shareId }">
                                                mdi-delete
                                            </v-icon>
                                        </template>
                                    </v-list-item>
                                </template>
                            </v-list>
                        </v-card>
                    </v-window-item>
                    <v-window-item value="done">
                        <v-card max-height="500" elevation="0">
                            <v-list>
                                <v-list-item class="py-2 mb-5 rounded-lg" title="Permission">
                                    <template v-slot:append>
                                        <v-sheet width="120">
                                            <v-select color="primary" class="w-100" density="compact" hide-details chips
                                                :items="permissions" v-model="permissionForAll">
                                                <template v-slot:chip="{ item }">
                                                    <v-chip color="primary" class="mx-auto">{{ item.title }}</v-chip>
                                                </template>
                                            </v-select>
                                        </v-sheet>
                                    </template>
                                </v-list-item>
                                <template v-for="(share, index) in inviteUser" :key="index">
                                    <v-list-item class="py-2 mb-1 rounded-lg border" :title="share.userName"
                                        :subtitle="share.email">
                                        <template v-slot:prepend>
                                            <v-avatar color="primary">
                                                {{ share.email.slice(0, 1) }}
                                            </v-avatar>
                                        </template>
                                        <template v-slot:append>
                                            <v-sheet width="120">
                                                <v-select color="primary" class="w-100" density="compact" hide-details chips
                                                    :items="permissions" v-model="share.permission">
                                                    <template v-slot:chip="{ item }">
                                                        <v-chip color="primary" class="mx-auto">{{ item.title }}</v-chip>
                                                    </template>
                                                </v-select>
                                            </v-sheet>
                                        </template>
                                    </v-list-item>
                                </template>
                            </v-list>
                        </v-card>
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
    <delete-propmt title="Remove member" subtitle="Are you sure you want to remove this user" v-model="deleteVisible"
        @click:yes="() => handleRemove(shareId)"></delete-propmt>
</template>
