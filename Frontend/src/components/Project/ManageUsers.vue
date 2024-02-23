<script setup lang="ts">
import { ref, watch, type Ref, onMounted } from 'vue'
import type { IProject, IProjectMembers } from '@/Models/ProjectModels'
import { GlobalStore, ProjectStore, UserStore } from '@/stores'
import { DeletePrompt } from '@/components/Custom'
import { permissions } from '@/data/permission'
import { storeToRefs } from 'pinia'
import { useTheme } from '@/composable/useTheme'

const { dark } = useTheme()
const props = defineProps<{
    project: IProject
}>()

const permissionForAll = ref(0)
const InvitedUsers: Ref<IProjectMembers[]> = ref([])
const updatingUsers: Ref<IProjectMembers[]> = ref([])
const projectStore = ProjectStore()
const { Loading } = storeToRefs(GlobalStore())
const areAllPermissionsSame = () => {
    if (InvitedUsers.value.length > 0) {
        const firstPermision = InvitedUsers.value[0].permission
        return InvitedUsers.value.every(x => x.permission == firstPermision) ? firstPermision : 0
    }
    return 0
}
const handleValueChanges = (index: number) => {
    const updatingValue = InvitedUsers.value[index]
    const oldValue = props.project.users.filter(u => u.invitedUser.id != UserStore().User.id)[index]
    if (updatingUsers.value.some(x => x.shareId == updatingValue.shareId)) {
        if (updatingValue.permission == oldValue.permission) {
            updatingUsers.value.splice(updatingUsers.value.findIndex(x => x.shareId == updatingValue.shareId), 1)
        } else {
            updatingUsers.value.splice(updatingUsers.value.findIndex(x => x.shareId == updatingValue.shareId), 1, updatingValue)
        }
    } else {
        updatingUsers.value.push(updatingValue)
    }
    const firstPermision = InvitedUsers.value[index].permission
    if (InvitedUsers.value.every(x => x.permission == firstPermision)) {
        permissionForAll.value = firstPermision
    }
}
const handleRemove = async (id: string) => {
    var res = await projectStore.RemoveUser(id, props.project.id)
    if (res) {
        InvitedUsers.value.splice(InvitedUsers.value.findIndex(x => x.shareId == id), 1)
    }
}
const handleUpdate = async () => {
    const updatePermissionModel = updatingUsers.value.map(x => {
        return {
            shareId: x.shareId,
            permission: x.permission
        }
    })
    await projectStore.UpdatePermissions(updatePermissionModel, props.project.id)
    updatingUsers.value = []
}
watch(permissionForAll, () => {
    InvitedUsers.value = InvitedUsers.value.map(x => {
        return { ...x, permission: permissionForAll.value }
    })
    updatingUsers.value = InvitedUsers.value.map(x => {
        return { ...x }
    })
    InvitedUsers.value.map((val, index) => handleValueChanges(index))
})
onMounted(() => {
    InvitedUsers.value = props.project.users
        .filter(u => u.invitedUser.id != UserStore().User.id)
        .map(x => {
            return { ...x }
        })
    permissionForAll.value = areAllPermissionsSame()
})
</script>
<template>
    <v-row class="justify-end mb-3 align-center">
        <v-col cols="auto">
            <v-sheet width="120" class="px-0 bg-transparent">
                <v-select density="compact" color="primary" hide-details :items="permissions" v-model="permissionForAll">
                    <template v-slot:item="{ props }">
                        <v-list-item v-bind="props" density="compact"></v-list-item>
                    </template>
                </v-select>
            </v-sheet>
        </v-col>
        <v-col cols="auto">
            <v-btn color="primary" variant="elevated" min-width="120" class="mx-4 mb-1 rounded"
                :disabled="updatingUsers.length == 0 || Loading" :loading="Loading" @click="handleUpdate">
                Save
                <span v-if="updatingUsers.length" class="ms-2"> ({{ updatingUsers.length }})</span>
            </v-btn>
        </v-col>
    </v-row>
    <v-row>
        <v-col cols="12">
            <v-list v-if="InvitedUsers.length > 0" class="px-3 pt-4 bg-transparent">
                <template v-for="(user, index) in InvitedUsers" :key="index">
                    <v-list-item class="py-3 mb-3 border rounded-lg" :title="user.invitedUser.userName"
                        :subtitle="user.invitedUser.email" :class="[{ 'bg-white': !dark }]">
                        <template v-slot:prepend>
                            <v-avatar color="primary">
                                {{ user.invitedUser.email.slice(0, 1).toUpperCase() }}
                            </v-avatar>
                        </template>
                        <template v-slot:append>
                            <v-sheet width="120" class="mx-3">
                                <v-select density="compact" color="primary" hide-details :items="permissions"
                                    v-model="user.permission" @update:model-value="() => handleValueChanges(index)">
                                    <template v-slot:item="{ props }">
                                        <v-list-item v-bind="props" density="compact"></v-list-item>
                                    </template>
                                </v-select>
                            </v-sheet>
                            <delete-prompt title="Remove Member" subtitle="Are you sure you want to remove this user"
                                @click:yes="() => handleRemove(user.shareId)">
                                <template v-slot:default="{ props }">
                                    <v-icon color="danger" v-bind="props">mdi-delete</v-icon>
                                </template>
                            </delete-prompt>
                        </template>
                    </v-list-item>
                </template>
            </v-list>
            <v-card class="d-flex justify-center align-center text-grey border rounded-lg" height="250" elevation="0"
                v-else>
                No invited users
            </v-card>
        </v-col>
    </v-row>
</template>
