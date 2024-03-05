<script setup lang="ts">
import { ref, watch } from 'vue'
import { QuillEditor } from '@vueup/vue-quill'
import { useTheme } from "@/composable/useTheme"

const props = defineProps<{
    modelValue?: string,
    height?: number
}>()

const { dark } = useTheme()
const quill = ref()
const text = ref(props.modelValue)
watch(props, () => {
    text.value = props.modelValue
})
watch(text, () => {
    if (quill.value.getText() == '\n')
        emits('update:modelValue', undefined)
    else
        emits('update:modelValue', text.value)
})
const emits = defineEmits<{
    (e: 'update:modelValue', modelValue: string | undefined): void
}>()
defineOptions({
    inheritAttrs: false
})
const toolbar = [
    ['bold', 'italic', 'underline', 'strike'],
    ['blockquote', 'code-block', 'link'],
    [{ 'header': [1, 2, 3, false] }],
    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    [{ 'color': [] }, { 'background': [] }],
    [{ 'size': ['small', false, 'large'] }],
    [{ 'font': [] }],
    [{ 'align': [] }],
]
</script>

<template>
    <div class="container" :class="[{ 'dark': dark }]">
        <quill-editor ref="quill" :toolbar="toolbar" :style="`height: ${height ?? 150}px;`" v-model:content="text"
            content-type="html">
        </quill-editor>
    </div>
</template>

<style>
.ql-tooltip {
    left: 10px !important;
}

.ql-action {
    color: #26A69A !important;
}

.dark>.ql-snow>.ql-tooltip {
    background-color: rgb(33, 33, 33) !important;
    color: white;
}

.dark>.ql-snow .ql-picker {
    color: white !important;
}

.dark>.ql-snow .ql-stroke,
.dark>.ql-snow .ql-fill {
    stroke: white;
}
</style>