<script setup lang="ts">
import { ref, type Ref } from 'vue';

const props = withDefaults(defineProps<{
    icon?: string,
    iconColor?: 'edit' | 'delete' | 'info' | undefined
}>(), {
    icon: '',
    noIcon: false
})
const color: Ref<string> = ref('')
switch (props.iconColor) {
    case 'edit':
        color.value = 'amber-lighten-1'
        break
    case 'delete':
        color.value = 'red'
        break
    case 'info':
        color.value = 'light-blue'
        break
    default:
        color.value = 'white'
}
</script> 
<template>
    <v-hover v-slot="{ isHovering, props }">
        <div class="d-flex align-center pe-8 ps-3 py-3 my-1 sweep-to-right"
            :class="isHovering ? 'text-white' : 'text-blue-grey-darken-2'" v-bind="props">
            <v-icon v-if="icon != ''" :color="isHovering ? color : ''">mdi-{{ icon }}</v-icon>
            <span class="ms-6 me-9" :class="isHovering ? `text-${color}` : ''">
                <slot></slot>
            </span>
        </div>
    </v-hover>
</template>
<style>
.sweep-to-right {
    position: relative;
    -webkit-transform: translateZ(0);
    transform: translateZ(0);
}

.sweep-to-right:hover {
    color: white !important;
}

.sweep-to-right:before {
    content: '';
    position: absolute;
    z-index: -1;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: #b0bec5;
    -webkit-transform: scaleX(0);
    transform: scaleX(0);
    -webkit-transform-origin: 0 50%;
    transform-origin: 0 50%;
    -webkit-transition-property: transform;
    transition-property: transform;
    -webkit-transition: 600ms ease-out;
    transition: 600ms ease-out;
}

.sweep-to-right:hover:before {
    -webkit-transform: scaleX(1);
    transform: scaleX(1);
    color: white;
}
</style>