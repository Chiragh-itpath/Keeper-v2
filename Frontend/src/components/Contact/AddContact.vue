<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { ContactStore, GlobalStore, UserStore } from '@/stores'
import { debounce } from 'lodash'
import { storeToRefs } from 'pinia'
import type { IUser } from '@/Models/UserModels'
import { Permission } from '@/Models/enum'
import { permissions } from '@/data/permission'
import type { IAddContact, IContact } from '@/Models/ContactModels'
import type { IProject } from '@/Models/ProjectModels'

const props = defineProps<{
    contacts: IContact[],
    project: IProject[]
}>()

const visible: Ref<boolean> = ref(false)
const searchValue: Ref<string | null | undefined> = ref()
const menu: Ref<boolean> = ref(false)
const windowTab: Ref<'search' | 'selection'> = ref('search')
const isLoading: Ref<boolean> = ref(false)
const { Loading } = storeToRefs(GlobalStore())
const Contacts = ref(props.contacts)
const Projects = ref(props.project)
const Users: Ref<IUser[]> = ref([])
const selectedUsers: Ref<IUser[]> = ref([])
const ContactsToAdd: Ref<IAddContact[]> = ref([])

const selection = (selected: IUser, index: number) => {
    selectedUsers.value.push(selected)
    ContactsToAdd.value = selectedUsers.value.map(x => {
        return {
            user: x,
            projectId: undefined,
            permission: Permission.VIEW
        }
    })
    Users.value.splice(index, 1)
}
const submitHandler = async () => {
    await ContactStore().AddContact(ContactsToAdd.value)
    visible.value = false
}
watch(visible, async () => {
    if (visible.value) {
        Users.value = []
        windowTab.value = 'search'
        selectedUsers.value = []
    }
})
const searchHandler = debounce(async (search: string | null): Promise<void> => {
    isLoading.value = true
    menu.value = false
    if (!search || search.trim() == '') {
        Users.value = []
        isLoading.value = false
        return
    }
    const res = await UserStore().SearchEmail(search)
    Users.value = res
        .filter(x =>
            !selectedUsers.value.some(user => user.id === x.id)
        ).filter(x =>
            !Contacts.value.some(user => user.addedPerson.id === x.id)
        )
    isLoading.value = false
    menu.value = true
}, 500)
</script>
<template>
    <v-dialog v-model="visible" max-width="700" :close-on-back="!Loading">
        <template v-slot:activator="{ props }">
            <v-btn color="primary" class="cursor-pointer" prepend-icon="mdi-plus" v-bind="props">New Contact</v-btn>
        </template>
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Contact</span>
                <span class="float-right">
                    <v-btn @click="visible = !visible" icon="mdi-close" variant="flat" color="primary" :disabled="Loading"
                        density="compact" />
                </span>
            </v-card-title>
            <v-card-text class="py-5">
                <v-card elevation="0" height="300">
                    <v-window v-model="windowTab" :touch="false">
                        <v-window-item value="search">
                            <v-menu v-model="menu" :close-on-content-click="false">
                                <template v-slot:activator="{ props }">
                                    <v-text-field v-model="searchValue" v-bind="props" @update:model-value="searchHandler"
                                        hide-details color="primary" class="cursor-text" density="comfortable" clearable />
                                </template>
                                <v-list v-if="searchValue && searchValue != ''">
                                    <v-list-item title="No user found with this email" class="text-center text-grey"
                                        v-if="Users.length == 0" />
                                    <template v-for="(user, index) in Users" :key="index">
                                        <v-list-item :title="user.userName" :subtitle="user.email" :value="user.id"
                                            @click="() => selection(user, index)">
                                            <template v-slot:prepend>
                                                <v-avatar :text="user.email.charAt(0).toUpperCase()" color="primary" />
                                            </template>
                                        </v-list-item>
                                    </template>
                                </v-list>
                            </v-menu>

                            <v-list class="overflow-y-auto px-0">
                                <v-list-item v-for="(user, index) in selectedUsers" :key="index"
                                    class="border mb-1 px-2 py-3 rounded-lg">
                                    <template v-slot:prepend>
                                        <v-avatar color="primary">
                                            {{ user.email.charAt(0).toUpperCase() }}
                                        </v-avatar>
                                    </template>
                                    <template v-slot:title>
                                        {{ user.userName }}
                                    </template>
                                    <template v-slot:subtitle>
                                        {{ user.email }}
                                    </template>
                                    <template v-slot:append>
                                        <v-icon color="danger"
                                            @click="() => selectedUsers.splice(index, 1)">mdi-delete</v-icon>
                                    </template>
                                </v-list-item>
                            </v-list>
                        </v-window-item>
                        <v-window-item value="selection">
                            <v-list>
                                <template v-for="(contact, index) in ContactsToAdd" :key="index">
                                    <v-list-item :title="contact.user.userName" :subtitle="contact.user.email"
                                        class="border rounded-lg py-2 mb-2" :ripple="false" @click.stop>
                                        <template v-slot:prepend>
                                            <v-avatar :text="contact.user.email.charAt(0).toUpperCase()"
                                                color="primary"></v-avatar>
                                        </template>
                                        <template v-slot:append>
                                            <v-menu>
                                                <template v-slot:activator="{ props, isActive }">
                                                    <v-chip v-bind="props"
                                                        :append-icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'"
                                                        color="primary">
                                                        <span class="text-truncate" style="max-width: 100px;">
                                                            {{
                                                                Projects.find(x => x.id == contact.projectId)?.title ??
                                                                'Select Project'
                                                            }}
                                                        </span>
                                                        <template v-slot:append v-if="contact.projectId">
                                                            <v-icon icon="mdi-close-circle"
                                                                @click.stop="contact.projectId = undefined"></v-icon>
                                                        </template>
                                                    </v-chip>
                                                </template>
                                                <v-list max-height="200" width="200">
                                                    <template v-for="(project, index) in Projects.filter(x => !x.isShared)"
                                                        :key="index">
                                                        <v-list-item :title="project.title" class="text-truncate"
                                                            @click="contact.projectId = project.id; contact.permission = 0"
                                                            density="compact" />
                                                    </template>
                                                </v-list>
                                            </v-menu>
                                            <v-menu>
                                                <template v-slot:activator="{ props, isActive }">
                                                    <v-chip v-bind="props" color="primary" class="ms-3"
                                                        :append-icon="isActive ? 'mdi-menu-up' : 'mdi-menu-down'"
                                                        v-if="contact.projectId">
                                                        <span class="text-truncate" style="max-width: 100px;">
                                                            {{ permissions[contact.permission ?? 0].title }}
                                                        </span>
                                                    </v-chip>
                                                </template>
                                                <v-list>
                                                    <template v-for="(permission, index) in permissions" :key="index">
                                                        <v-list-item density="compact" :title="permission.title"
                                                            @click="contact.permission = permission.value" />
                                                    </template>
                                                </v-list>
                                            </v-menu>
                                        </template>
                                    </v-list-item>
                                </template>
                            </v-list>
                        </v-window-item>
                    </v-window>
                </v-card>
            </v-card-text>
            <v-card-actions class="justify-end px-4 pb-4">
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" prepend-icon="mdi-arrow-left"
                    text="prev" :disabled="windowTab == 'search'" @click="windowTab = 'search'">

                </v-btn>
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" append-icon="mdi-arrow-right"
                    text="next" :disabled="selectedUsers.length == 0 || windowTab == 'selection'"
                    @click="windowTab = 'selection'">

                </v-btn>
                <v-spacer></v-spacer>
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" @click="submitHandler"
                    :loading="Loading" :disabled="Loading || selectedUsers.length == 0">
                    Add
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>