<script setup lang="ts">
import { ref, watch, type Ref } from 'vue'
import type { IComment } from '@/Models/CommentModel'
import AddComment from '@/components/Comments/AddComment.vue'
import CommentView from '@/components/Comments/CommentView.vue'
const props = defineProps<{
    comments: IComment[],
    itemId: string,
    commentId: string
}>()
const replyVisible: Ref<boolean> = ref(false)
const repliesVisible: Ref<boolean> = ref(false)
const comments = ref(props.comments)
watch(props, () => {
    comments.value = props.comments
})
const latestComment = (comment: IComment) => {
    comments.value = [comment, ...comments.value]
    emit('update:comments', comments.value)
}
const emit = defineEmits<{
    (e: 'update:comments', comments: IComment[]): void
}>()
</script>
<template>
    <span class="text-grey me-5" role="button" @click="repliesVisible = !repliesVisible" v-if="comments.length > 0">
        Show {{ comments.length }}* replies</span>
    <span class="font-weight-bold" role="button" @click="replyVisible = !replyVisible">reply</span>
    <add-comment v-if="replyVisible" :item-id="itemId" :comment-id="commentId" @latest-comment="latestComment">
    </add-comment>
    <div v-if="repliesVisible">
        <comment-view v-for="(comment, index) in comments" :key="index" :comment="comment" :item-id="itemId">
        </comment-view>
    </div>
</template>
