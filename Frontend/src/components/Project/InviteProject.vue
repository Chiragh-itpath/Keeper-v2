<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { storeToRefs } from 'pinia';
import { InviteStore, GlobalStore, ProjectStore } from '@/stores'
import { InviteDropDown } from '@/components/Contact'
import { DeletePropmt } from '@/components/Custom'
import type { IUser } from '@/Models/UserModels'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'

const props = withDefaults(defineProps<{
    project: IProject | undefined,
    modelValue: boolean
}>(), {
    modelValue: false
})

const visible: Ref<boolean> = ref(props.modelValue)
const shareId: Ref<string> = ref('')
const deleteVisible: Ref<boolean> = ref(false)
const inviteUser: Ref<IUser[]> = ref([])
const selectedUsers: Ref<IUser[]> = ref([])
const InvitedUsers: Ref<IProjectMembers[]> = ref([])
const errorMessage: Ref<string> = ref('')
const { InviteUsersToProject } = InviteStore()
const { Loading } = storeToRefs(GlobalStore())
const projectStore = ProjectStore()

const handleInvite = async (): Promise<void> => {
    if (props.project) {
        await InviteUsersToProject(props.project.id, inviteUser.value)
        InvitedUsers.value = await projectStore.GetInvitedMembers(props.project.id)
        inviteUser.value = []
        visible.value = false
    }
}
const handleRemove = async (id: string) => {
    var res = await projectStore.RemoveUser(id)
    if (res) {
        InvitedUsers.value.splice(InvitedUsers.value.findIndex(x => x.shareId == shareId.value), 1)
    }
    deleteVisible.value = false
}
watch(props, async () => {
    visible.value = props.modelValue
    InvitedUsers.value = props.project?.users ?? []
})
watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
    errorMessage.value = ''
})
watch(selectedUsers, () => {
    inviteUser.value = selectedUsers.value.filter(x =>
        !InvitedUsers.value.some(i => i.invitedUser.id == x.id)
    );
    errorMessage.value = selectedUsers.value.length != inviteUser.value.length ?
        'Users already invited in project will be skipped' : '';
});
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()
</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="700" close-on-back>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Project
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12">
                        <invite-drop-down v-model:users="selectedUsers" :error-message="errorMessage"></invite-drop-down>
                    </v-col>
                </v-row>
                <v-card elevation="0" max-height="300">
                    <v-list>
                        <v-list-item class="px-1">Members with access</v-list-item>
                        <v-list-item v-if="InvitedUsers.length == 0"
                            class="border rounded-lg bg-grey-lighten-3 text-center text-grey">
                            No users invited yet
                        </v-list-item>
                        <template v-for="share in InvitedUsers">
                            <v-list-item class="py-2 mb-1 rounded-lg border" :title="share.invitedUser.userName"
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
                                    <v-icon icon="mdi-delete" color="danger"
                                        @click="() => { deleteVisible = true; shareId = share.shareId }">
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
                    <span v-if="inviteUser.length > 0">
                        ({{ inviteUser.length }})
                    </span>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
    <delete-propmt title="Remove Member" subtitle="Are you sure you want to remove this user" v-model="deleteVisible"
        @click:yes="handleRemove(shareId)"></delete-propmt>
</template>