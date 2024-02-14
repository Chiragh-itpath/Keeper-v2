<script setup lang="ts">
import { ref, watch, type Ref, reactive, computed } from 'vue'
import { ContactStore, GlobalStore, InviteStore, UserStore } from '@/stores'
import { storeToRefs } from 'pinia'
import { TextField } from '@/components/Custom'
import type { IAddContact, IContact } from '@/Models/ContactModels'
import type { IProject } from '@/Models/ProjectModels'
import type { IProjectInvite } from '@/Models/InviteModels'
import { permissions } from '@/data/permission'
import { Permission } from '@/Models/enum'

const { contacts, projects } = defineProps<{
    contacts: IContact[],
    projects: IProject[]
}>()
const contactStore = ContactStore()
const { User } = storeToRefs(UserStore())
const { Loading, errors } = storeToRefs(GlobalStore())
const window: Ref<'form' | 'project'> = ref('form')

const visible: Ref<boolean> = ref(false)
watch(visible, async () => {
    if (form.value) {
        form.value.reset()
    }
    window.value = 'form'
    errors.value = {}
    selected.value = []
    validate.value = false
    invite.value = false
})
const validate: Ref<boolean> = ref(false)
const form = ref()
const addContact = reactive<IAddContact>({
    firstName: '',
    lastName: '',
    email: ''
})
const submitHandler = async () => {
    validate.value = true
    errors.value = {}
    const { valid } = await form.value.validate()
    if (!valid) return
    if (User.value.email.toLowerCase() == addContact.email.toLowerCase()) {
        errors.value.email = 'Cannot add your own email.'
        return
    }
    if (contacts.some(x => x.email == addContact.email)) {
        errors.value.email = 'Contact already exist'
        return
    }
    if (await contactStore.AddContact(addContact)) {
        if (invite.value)
            window.value = 'project'
        else
            visible.value = false
    }
}
const invite: Ref<boolean> = ref(false)
const items = computed((): { title: string, value: IProjectInvite }[] => {
    return projects
        .filter(p => !p.isShared)
        .map(p => {
            return {
                title: p.title,
                value: {
                    email: addContact.email,
                    projectId: p.id,
                    permission: Permission.VIEW
                }
            }
        })
})
const selected: Ref<IProjectInvite[]> = ref([])
const inviteStore = InviteStore()
const inviteHandler = async () => {
    await inviteStore
        .InviteUsersToProject(selected.value);
    visible.value = false
}
</script>
<template>
    <v-dialog v-model="visible" max-width="700" :close-on-back="!Loading">
        <template v-slot:activator="{ props }">
            <v-btn color="primary" class="cursor-pointer" prepend-icon="mdi-plus" v-bind="props">New Contact</v-btn>
        </template>
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>{{ window == 'form' ? 'New Contact' : 'Select Projects' }}</span>
                <span class="float-right">
                    <v-btn @click="visible = !visible" icon="mdi-close" variant="flat" color="primary" :disabled="Loading"
                        density="compact" />
                </span>
            </v-card-title>
            <v-card-text class="py-5">
                <v-window v-model="window" :touch="false">
                    <v-window-item value="form">
                        <v-form ref="form" :validate-on="validate ? 'input' : 'submit'" @submit.prevent="submitHandler">
                            <v-row>
                                <v-col cols="12" sm="6">
                                    <text-field label="First name" is-required v-model="addContact.firstName"></text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <text-field label="Last name" is-required v-model="addContact.lastName"></text-field>
                                </v-col>
                                <v-col cols="12">
                                    <text-field label="Email" is-required is-email v-model="addContact.email"
                                        :error-messages="errors.email"> </text-field>
                                </v-col>
                            </v-row>
                        </v-form>
                    </v-window-item>
                    <v-window-item value="project">
                        <v-select multiple :items="items" density="comfortable" color="primary" v-model="selected"
                            label="Select Projects" chips>
                            <template v-slot:chip>
                                <v-chip color="primary"></v-chip>
                            </template>
                        </v-select>
                        <v-list height="200">
                            <template v-for="(item, index) in selected" :key="index">
                                <v-list-item class="border mb-2 rounded-lg">
                                    {{ projects.find(x => x.id == item.projectId)?.title }}
                                    <template v-slot:append>
                                        <v-menu>
                                            <template v-slot:activator="{ props, isActive }">
                                                <v-chip color="primary" v-bind="props">
                                                    {{ permissions[item.permission].title }}
                                                    <template v-slot:append>
                                                        <v-icon :icon="`mdi-menu-${isActive ? 'up' : 'down'}`" />
                                                    </template>
                                                </v-chip>
                                            </template>
                                            <v-list>
                                                <template v-for="(permission, index) in permissions" :key="index">
                                                    <v-list-item :title="permission.title" :value="permission.value"
                                                        density="compact" @click="item.permission = permission.value">
                                                    </v-list-item>
                                                </template>
                                            </v-list>
                                        </v-menu>
                                    </template>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-window-item>
                </v-window>
            </v-card-text>
            <v-card-actions class="px-5 pb-4">
                <div v-if="window === 'form'" class="d-flex align-center">
                    <v-checkbox label="Invite to project" density="compact" hide-details color="primary" v-model="invite" />
                    <v-tooltip width="200">
                        <template v-slot:activator="{ props }">
                            <v-icon v-bind="props" class="ms-2 cursor-pointer" size="small">mdi-help-circle-outline</v-icon>
                        </template>
                        When checkbox is selected and you add contact pop up will be not closed and will allow you to
                        invited user directly from here
                    </v-tooltip>
                </div>
                <v-spacer></v-spacer>
                <v-btn class="rounded-xl" color="primary" :width="!invite ? 120 : 150" variant="elevated"
                    :text="!invite ? 'add' : 'add & invite'" :loading="Loading" :disabled="Loading" @click="submitHandler"
                    v-if="window === 'form'" />
                <v-btn class="rounded-xl" color="primary" width="120" variant="elevated" text="invite" :loading="Loading"
                    :disabled="Loading || selected.length == 0" @click="inviteHandler" v-else />
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>