<script setup lang="ts">
import { ref, type Ref } from 'vue'
import type { IComment } from '@/Models/CommentModel'
import CommentView from '@/components/Comments/CommentView.vue'
import AddComment from '@/components/Comments/AddComment.vue'
const props = defineProps<{
    itemId: string,
    comments: IComment[]
}>()
const allComments: Ref<IComment[]> = ref(props.comments)
</script>
<template>
    <div class="mt-5">
        <add-comment :item-id="itemId" @latest-comment="(comment) => { allComments.unshift(comment) }"></add-comment>
    </div>
    <v-card elevation="0" class="my-5 overflow-auto py-0" v-if="allComments.length > 0">
        <v-card-text>
            <comment-view v-for="(comment, index) in allComments" :key="index" :comment="comment" :item-id="itemId">
            </comment-view>
        </v-card-text>
    </v-card>
</template>
<style scoped>
.text-subtitle-2 {
    font-size: 0.775rem !important;
}
</style>