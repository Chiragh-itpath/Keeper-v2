<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { InviteDropDown } from '@/components/Contact/'
import { DeletePropmt } from '@/components/Custom/'
import { GlobalStore, InviteStore, KeepStore } from '@/stores'
import type { IUser } from '@/Models/UserModels'
import type { IKeep, IKeepMembers } from '@/Models/KeepModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'

const props = withDefaults(defineProps<{
    keep: IKeep | undefined
    project: IProject
    modelValue: boolean
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(props.modelValue)
const inviteUser: Ref<IUser[]> = ref([])
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
watch(props, () => {
    visible.value = props.modelValue
    if (visible.value && props.keep) {
        keepInvitedUsers.value = props.keep.users
        projectInvitedUsers.value = props.project.users
    }
    errorMessage.value = ''
})
watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
})
watch(selectedUser, () => {
    inviteUser.value = selectedUser.value.filter(u =>
        !keepInvitedUsers.value.some(s => s.invitedUser.id == u.id)
    ).filter(u =>
        !projectInvitedUsers.value.some(s => s.invitedUser.id == u.id)
    )
    errorMessage.value = inviteUser.value.length != selectedUser.value.length ?
        'Users already invited in project or keep will be skipped' : ''
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()

</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="700" close-on-back>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Keep
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12">
                        <invite-drop-down v-model:users="selectedUser" :error-message="errorMessage"></invite-drop-down>
                    </v-col>
                </v-row>
                <v-card elevation="0" max-height="300">
                    <v-list lines="two">
                        <v-list-item class="px-1">Members with access</v-list-item>
                        <v-list-item v-if="keepInvitedUsers.length == 0"
                            class="text-grey text-center bg-grey-lighten-4 border rounded-lg">
                            No users invited yet
                        </v-list-item>
                        <template v-for="share in keepInvitedUsers" :key="share.shareId">
                            <v-list-item class="py-2 mb-1 border rounded-lg" :title="share.invitedUser.userName"
                                :subtitle="share.invitedUser.email">
                                <template v-slot:prepend>
                                    <v-avatar color="primary">
                                        {{ share.invitedUser.email.slice(0, 1).toUpperCase() }}
                                    </v-avatar>
                                </template>
                                <template v-slot:append>
                                    <v-tooltip>
                                        <template v-slot:activator="{ props }">
                                            <v-icon class="me-5" v-bind="props"
                                                :color="share.isAccepted ? 'success' : 'amber-darken-4'"
                                                :icon="share.isAccepted ? 'mdi-account-check-outline' : 'mdi-account-clock-outline'">
                                            </v-icon>
                                        </template>
                                        {{ share.isAccepted ? 'Accepted' : 'Pending' }}
                                    </v-tooltip>
                                    <v-icon color="danger" @click="() => { deleteVisible = true; shareId = share.shareId }">
                                        mdi-delete
                                    </v-icon>
                                </template>
                            </v-list-item>
                        </template>
                    </v-list>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn color="primary" variant="elevated" min-width="130" class="mx-4 mb-3 rounded-xl" @click="handleInvite"
                    :disabled="Loading || inviteUser.length == 0" :loading="Loading">
                    invite
                    <template v-if="inviteUser.length != 0">
                        ({{ inviteUser.length }})
                    </template>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
    <delete-propmt title="Remove member" subtitle="Are you sure you want to remove this user" v-model="deleteVisible"
        @click:yes="() => handleRemove(shareId)"></delete-propmt>
</template>
