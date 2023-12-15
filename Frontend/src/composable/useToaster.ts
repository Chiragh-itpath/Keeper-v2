import type { Colors } from '@/Models/enum'
import { GlobalStore } from '@/stores/GlobalStore'
import { storeToRefs } from 'pinia'
export const useToster = (options: { message: string; color?: Colors }) => {
    const { toasterColor, toasterText, tosterVisible } = storeToRefs(GlobalStore())
    tosterVisible.value = true
    toasterText.value = options.message
    toasterColor.value = options.color ?? 'success'
}
