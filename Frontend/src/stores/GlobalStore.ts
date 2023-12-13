import { defineStore, storeToRefs } from 'pinia'
import { computed, ref, type Ref } from 'vue'
import { InviteStore } from '@/stores'
import { Colors } from '@/Models/enum'

const GlobalStore = defineStore('globalStore', () => {
    const { InvitedProjectList, InvitedKeepList } = storeToRefs(InviteStore())
    const tosterVisible: Ref<boolean> = ref(false)
    const toasterText: Ref<string> = ref('')
    const toasterColor: Ref<string> = ref('primary')
    const Loading: Ref<boolean> = ref(false)
    const SideBarIsVisible: Ref<boolean> = ref(true)
    const errors: Ref<Record<string, string>> = ref({})
    const setError = (key: string, value: string) => {
        errors.value = {}
        errors.value[key] = value
    }
    const NotificationCount = computed((): number => {
        return InvitedProjectList.value.length + InvitedKeepList.value.length
    })
    const ToggleSideBar = (): void => {
        SideBarIsVisible.value = !SideBarIsVisible.value
    }
    const makeToster = (message: string, _color: Colors) => {
        toasterText.value = message
        toasterColor.value = _color
        tosterVisible.value = true
    }
    return {
        tosterVisible,
        toasterText,
        toasterColor,
        Loading,
        SideBarIsVisible,
        NotificationCount,
        errors,
        makeToster,
        ToggleSideBar,
        setError
    }
})
export { GlobalStore }
