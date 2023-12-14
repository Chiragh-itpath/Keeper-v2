<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { InviteStore, ContactStore, GroupStore } from '@/stores'
import InviteDropDown from '@/components/Contact/InviteDropDown.vue'

const props = withDefaults(defineProps<{
    id: string,
    modelValue: boolean
}>(), {
    id: '',
    modelValue: false
})

const visible: Ref<boolean> = ref(props.modelValue)
const inviteEmails: Ref<string[]> = ref([])
const { InviteUsersToProject } = InviteStore()
const { GetContacts } = ContactStore()
const { GetGroups } = GroupStore()

const handleInvite = async (): Promise<void> => {
    await InviteUsersToProject(props.id, inviteEmails.value)
    visible.value = false

}
watch(props, async () => {
    if (props.modelValue) {
        await GetContacts()
        await GetGroups()
    }
    visible.value = props.modelValue
})
watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()
</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="600" close-on-back>
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Project
                <v-icon class="float-end" @click="visible = false">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12">
                        <invite-drop-down v-model:emails="inviteEmails"></invite-drop-down>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn color="primary" variant="elevated" min-width="130" class="mx-4 mb-3 rounded-xl" @click="handleInvite"
                    :disabled="inviteEmails.length == 0">
                    invite
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
