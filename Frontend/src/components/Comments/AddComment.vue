<script setup lang="ts">
import { ref, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import TextField from '@/components/Custom/TextField.vue'
import { GlobalStore, ItemStore, UserStore } from '@/stores'
import type { IComment } from '@/Models/CommentModel'

const props = withDefaults(defineProps<{
    itemId: string
    commentId?: string
}>(), {
    commentId: ''
})

const comment: Ref<string> = ref('')
const form = ref()
const { User } = UserStore()
const { AddComment } = ItemStore()
const { Loading } = storeToRefs(GlobalStore())
const submitHandler = async () => {
    const { valid } = await form.value.validate()
    if (!valid) return
    const res = await AddComment({
        content: comment.value,
        itemId: props.itemId,
        commentId: (props.commentId != '') ? props.commentId : null
    })
    if (res) {
        res.user = User.userName
        emits('latestComment', res)
        form.value.reset()
    }
}
const emits = defineEmits<{
    (e: 'latestComment', comment: IComment): void
}>()
</script>
<template>
    <v-form ref="form" validate-on="submit" class="d-flex mt-5" @submit.prevent="submitHandler">
        <text-field v-model="comment" is-required label="Add your comment"></text-field>
        <v-btn icon="mdi-arrow-up" class="ms-4 mt-1" color="primary" @click="submitHandler" :loading="Loading"
            :disabled="Loading"></v-btn>
    </v-form>
</template>