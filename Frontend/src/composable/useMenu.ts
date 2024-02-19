import { ref, type Ref } from "vue"

export const useMenu = () => {
    const menu: Ref<boolean> = ref(false)
    const menuHide: Ref<boolean> = ref(false)
    const close = () => {
        menu.value = false
        setTimeout(() => {
            menuHide.value = false
        }, 500);
    }
    return {
        menu,
        menuHide,
        close
    }
}