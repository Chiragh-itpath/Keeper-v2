<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import type { IKeep, IKeepMembers } from '@/Models/KeepModels'
import { GlobalStore, KeepStore } from '@/stores'
import { permissions } from '@/data/permission'
import { DeletePropmt } from '@/components/Custom'

const props = defineProps<{
    keep: IKeep
}>()

const { Loading } = storeToRefs(GlobalStore())
const visible: Ref<boolean> = ref(false)
const InvitedUsers: Ref<IKeepMembers[]> = ref([])
const updatingUsers: Ref<IKeepMembers[]> = ref([])
const permissionForAll: Ref<number> = ref(0)
const keepStore = KeepStore()
const areAllPermissionsSame = () => {
    if (InvitedUsers.value.length > 0) {
        const firstPermision = InvitedUsers.value[0].permission
        return InvitedUsers.value.every(x => x.permission == firstPermision) ? firstPermision : 0
    }
    return 0
}
const handleValueChanges = (index: number) => {
    const updatingValue = InvitedUsers.value[index]
    const oldValue = props.keep.users[index]
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
const handleRemove = async (shareId: string): Promise<void> => {
    const res: boolean = await keepStore.RemoveUser(shareId, props.keep.id)
    if (res) {
        InvitedUsers.value.splice(InvitedUsers.value.findIndex(x => x.shareId == shareId), 1)
    }
}

const handleUpdate = async () => {
    const updatePermissionModel = updatingUsers.value.map(x => {
        return {
            shareId: x.shareId,
            permission: x.permission
        }
    })
    await keepStore.UpdatePermissions(updatePermissionModel, props.keep.id)
    updatingUsers.value = []
    visible.value = false
}

watch(permissionForAll, () => {
    InvitedUsers.value = InvitedUsers.value.map(x => {
        return { ...x, permission: permissionForAll.value }
    })
    updatingUsers.value = InvitedUsers.value.map(x => {
        return { ...x }
    })
    InvitedUsers.value.map((x, index) => handleValueChanges(index))
})
watch(visible, () => {
    if (!visible.value) emits('close')
    InvitedUsers.value = props.keep.users
        .map(x => {
            return { ...x }
        })
    permissionForAll.value = areAllPermissionsSame()
})
const emits = defineEmits<{
    (e: 'close'): void
}>()
</script>
<template>
    <v-dialog v-model="visible" max-width="700">
        <template v-slot:activator="{ props }">
            <v-list-item v-bind="props">
                <v-icon>mdi-account-multiple-outline</v-icon>
                <span class="mx-3">Manage</span>
            </v-list-item>
        </template>
        <v-card>
            <v-card-title class="bg-primary text-center">
                Manage users and permissions
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-list-item class="px-1 mb-2">
                    <template v-slot:append>
                        <v-sheet width="120">
                            <v-select density="compact" color="primary" hide-details :items="permissions"
                                v-model="permissionForAll"></v-select>
                        </v-sheet>
                    </template>
                </v-list-item>
                <v-list min-height="100" max-height="500" v-if="InvitedUsers.length > 0">

                    <template v-for="(user, index) in InvitedUsers" :key="index">
                        <v-list-item class="border rounded-lg py-2 mb-2" :title="user.invitedUser.userName"
                            :subtitle="user.invitedUser.email">
                            <template v-slot:prepend>
                                <v-avatar color="primary">
                                    {{ user.invitedUser.email.slice(0, 1).toUpperCase() }}
                                </v-avatar>
                            </template>
                            <template v-slot:append>
                                <v-sheet width="120" class="mx-2">
                                    <v-select density="compact" color="primary" hide-details :items="permissions"
                                        v-model="user.permission" @update:model-value="() => handleValueChanges(index)">
                                    </v-select>
                                </v-sheet>
                                <delete-propmt title="Remove user" subtitle="Are you sure you want to remove this user?"
                                    @click:yes="() => handleRemove(user.shareId)">
                                    <template v-slot="{ props }">
                                        <v-icon color="danger" v-bind="props">mdi-delete</v-icon>
                                    </template>
                                </delete-propmt>
                            </template>
                        </v-list-item>
                    </template>
                </v-list>
                <v-card v-else elevation="0" height="100"
                    class="d-flex justify-center align-center bg-grey-lighten-3 text-grey">
                    No invited users
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn color="primary" variant="flat" class="rounded-xl px-8" width="auto"
                    :disabled="updatingUsers.length == 0" :loading="Loading" @click="handleUpdate">
                    update
                    <span v-if="updatingUsers.length != 0">({{ updatingUsers.length }})</span>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>