<script setup lang="ts">
import type { IComment } from '@/Models/CommentModel'
import { dateDiffernce } from '@/Services/HelperFunction'
import AllReplies from '@/components/Comments/AllReplies.vue'
defineProps<{
    comment: IComment,
    itemId: string
}>()
</script>
<template>
    <div class="mt-2 bg-white border rounded-lg">
        <div class="d-flex align-center">
            <div class="ms-3 mt-2">
                <v-avatar color="primary" size="32"> {{ comment.user[0]?.toUpperCase() }}</v-avatar>
            </div>
            <div>
                <span class="font-weight-bold ms-3">{{ comment.user }}</span>
                <span class="ms-3 text-blue-grey-lighten-2 text-subtitle-2">
                    {{ dateDiffernce(comment.date) }}
                </span>
            </div>
        </div>
        <div class="ms-15 my-2 ">{{ comment.content }}</div>
        <div class="mx-2 my-2">
            <all-replies :comment="comment.comments" :item-id="itemId" :comment-id="comment.id"></all-replies>
        </div>
    </div>
</template>

<style scoped>
.border {
    border: none !important;
    border-left: 2px solid grey !important;
    border-top-left-radius: 0 !important;
}
</style>