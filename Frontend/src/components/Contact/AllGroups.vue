<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { GroupStore } from '@/stores'
import type { IGroup } from '@/Models/GroupModels'
import GroupDetails from '@/components/Contact/GroupDetails.vue';

const props = withDefaults(defineProps<{
    search?: string
}>(), {
    search: ''
})

const { Groups } = storeToRefs(GroupStore())


const GroupsToDisplay: Ref<IGroup[]> = ref(Groups.value)
watch(props, () => {
    if (props.search != '' && props.search != null) {
        GroupsToDisplay.value = Groups.value.filter(x => x.name.startsWith(props.search) || x.userEmail.startsWith(props.search))
    } else {
        GroupsToDisplay.value = Groups.value
    }
})
const HighLightText = (text: string) => {
    const searchText = props.search;
    if (searchText == '') {
        return text;
    }
    const regex = new RegExp(searchText, 'gi')
    return text.replace(regex, match => `<span class="high-light">${match}</span>`)
}
</script>
<template>
    <v-table>
        <thead>
            <tr>
                <th></th>
                <th>Name</th>
                <th>Contacts</th>
            </tr>
        </thead>
        <tbody>
            <tr v-if="Groups.length == 0 || GroupsToDisplay.length == 0">
                <td class="text-center" colspan="3">No Groups</td>
            </tr>
            <tr v-for="(group, index) in GroupsToDisplay " :key="index" class="cursor-pointer">
                <group-details :group="group" v-slot="{ onclick }">
                    <td @click="onclick" class="v-col-1">
                        <v-icon color="primary">mdi-account-group-outline</v-icon>
                    </td>
                    <td @click="onclick">
                        <span v-html="HighLightText(group.name)"></span>
                    </td>
                    <td @click="onclick">
                        <span>{{ group.contacts.length }}</span>
                    </td>
                </group-details>
            </tr>
        </tbody>
    </v-table>
</template>