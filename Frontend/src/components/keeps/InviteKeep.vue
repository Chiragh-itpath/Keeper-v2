<script setup lang="ts">
import { type Ref, ref, watch } from 'vue'
import { ContactStore, GlobalStore, GroupStore, InviteStore } from '@/stores'
import InviteDropDown from '@/components/Contact/InviteDropDown.vue'
import { storeToRefs } from 'pinia';

const props = withDefaults(defineProps<{
    id: string,
    projectId: string
    modelValue: boolean
}>(), {
    id: '',
    projectId: '',
    modelValue: false
})

const visible: Ref<boolean> = ref(props.modelValue)
const inviteEmails: Ref<string[]> = ref([])
const { InviteUsersToKeep } = InviteStore()
const { GetContacts } = ContactStore()
const { GetGroups } = GroupStore()
const { Loading } = storeToRefs(GlobalStore())
const handleInvite = async (): Promise<void> => {
    await InviteUsersToKeep(props.id, props.projectId, inviteEmails.value)
    visible.value = false

}
watch(props, async () => {
    visible.value = props.modelValue
    if (visible.value) {
        await GetContacts()
        await GetGroups()
    }
})
watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
})

const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: boolean): void
}>()

watch(visible, () => {
    if (!visible.value)
        emits('update:modelValue', visible.value)
})

</script>
<template>
    <v-dialog transition="scale-transition" v-model="visible" max-width="600" close-on-back max-height="500">
        <v-card>
            <v-card-title class="text-center bg-primary">
                Invite To Keep
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
                    :disabled="Loading || inviteEmails.length == 0" :loading="Loading">
                    invite
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
