<script setup lang="ts">
import { onMounted, } from 'vue'
import CommentView from '@/components/Comments/CommentView.vue'
import AddComment from '@/components/Comments/AddComment.vue'
import { ItemStore } from '@/stores'
import { storeToRefs } from 'pinia'
import type { IComment } from '@/Models/CommentModel'

const props = defineProps<{
    itemId: string,
}>()
const itemStore = ItemStore()
const { Comments } = storeToRefs(ItemStore())
const latestComments = (comment: IComment) => {
    Comments.value = [comment, ...Comments.value]
}
onMounted(async () => {
    await itemStore.fetchComments(props.itemId)
})
</script>
<template>
    <div class="mt-5">
        <add-comment :item-id="itemId" @latest-comment="latestComments"></add-comment>
    </div>
    <v-card elevation="0" class="my-5 overflow-auto py-0" v-if="Comments.length > 0">
        <v-card-text>
            <comment-view v-for="(comment, index) in Comments" :key="index" :comment="comment" :item-id="itemId">
            </comment-view>
        </v-card-text>
    </v-card>
</template>
<style scoped>
.text-subtitle-2 {
    font-size: 0.775rem !important;
}
</style>