<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { GlobalStore, GroupStore } from '@/stores'
import TextField from '@/components/Custom/TextField.vue'
import AllContact from '@/components/Contact/AllContacts.vue'
import { storeToRefs } from 'pinia';

const form = ref()
const visible: Ref<boolean> = ref(false)
const name: Ref<string> = ref('')
const contactList: Ref<string[]> = ref([])
const window: Ref<string> = ref('group')
const search: Ref<string> = ref('')

const { AddGroup } = GroupStore()
const { Loading } = storeToRefs(GlobalStore())
const validateForm = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return

    window.value = 'contact'
}
const handleSubmit = async () => {
    const { valid } = await form.value.validate()
    if (!valid) {
        window.value = 'group'
        return
    }

    await AddGroup({
        name: name.value,
        contactId: contactList.value
    })
    visible.value = false;
}

watch(visible, () => {
    if (visible.value) {
        window.value = 'group'
        search.value = ''
    }
    if (!visible.value) {
        form.value.reset()
    }
})
</script>
<template>
    <v-btn color="primary" prepend-icon="mdi-plus" class="rounded" @click="visible = !visible" width="100%">
        New Group
    </v-btn>
    <v-dialog v-model="visible" max-width="600" class="">
        <v-card class="rounded-lg">
            <v-card-title class="bg-primary text-center">
                <span>New Group</span>
                <span class="float-right">
                    <v-icon @click="visible = !visible">mdi-close</v-icon>
                </span>
            </v-card-title>
            <v-card-text>
                <v-window v-model="window">
                    <v-window-item value="group">
                        <v-form ref="form" validate-on="submit">
                            <text-field label="Name" placeholder="Enter name of your group" v-model="name"
                                is-required></text-field>
                        </v-form>
                    </v-window-item>
                    <v-window-item value="contact">
                        <v-text-field color="primary" label="Search" placeholder="Enter text to search" clearable
                            v-model="search"></v-text-field>
                        <v-card max-height="500" elevation="0">
                            <all-contact checkbox show-border hide-header v-model:selected="contactList"
                                :search="search"></all-contact>
                        </v-card>
                    </v-window-item>
                </v-window>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn class="rounded-xl mx-2 mb-2" variant="elevated" color="primary" width="120" v-if="window == 'group'"
                    append-icon="mdi-arrow-right" @click="validateForm" :loading="Loading" :disabled="Loading">
                    Next
                </v-btn>
                <v-btn class="rounded-xl mx-2 mb-2" variant="elevated" color="primary" width="120" v-else
                    @click="handleSubmit">
                    Done
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>