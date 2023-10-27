<script setup lang="ts">
import { ref, type Ref } from 'vue'
import type { IComment } from '@/Models/CommentModel'
import AddComment from '@/components/Comments/AddComment.vue'
import CommentView from '@/components/Comments/CommentView.vue'
const props = defineProps<{
    comment: IComment[],
    itemId: string,
    commentId: string
}>()
const replyVisible: Ref<boolean> = ref(false)
const repliesVisible: Ref<boolean> = ref(false)
const comments = ref(props.comment)
</script>
<template>
    <span class="text-grey me-5" role="button" @click="repliesVisible = !repliesVisible" v-if="comment.length > 0">
        Show {{ comment.length }}* replies</span>
    <span class="font-weight-bold" role="button" @click="replyVisible = !replyVisible">reply</span>
    <add-comment v-if="replyVisible" :item-id="itemId" :comment-id="commentId"
        @latest-comment="(comment) => { comments.unshift(comment); replyVisible = !replyVisible }"></add-comment>
    <div v-if="repliesVisible">
        <comment-view v-for="(comment, index) in comments" :key="index" :comment="comment" :item-id="itemId">
        </comment-view>
    </div>
</template>
