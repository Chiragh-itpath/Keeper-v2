import { computed } from "vue"
import { useTheme } from "vuetify"
type Theme = 'light' | 'dark'
type UserSettings = {
    theme: Theme
}
const userSettings: UserSettings = {
    theme: 'light'
}
const saveToCookie = (settings: UserSettings): void => {
    const settingsJSON = JSON.stringify(settings);
    document.cookie = `userSettings=${settingsJSON}; path=/; max-age=31536000`;
}
const getFromCookie = (): undefined | UserSettings => {
    const cookieValue = document.cookie.replace(/(?:(?:^|.*;\s*)userSettings\s*=\s*([^;]*).*$)|^.*$/, '$1');
    const settings = cookieValue ? JSON.parse(cookieValue) : undefined;
    return settings;
}

const ThemeHelper = () => {
    const oldSettings = getFromCookie()
    const theme = useTheme()
    if (oldSettings) {
        theme.global.name.value = oldSettings.theme
    }
    const dark = computed((): boolean => theme.global.name.value == 'dark')
    const toggle = (value: Theme) => {
        userSettings.theme = value
        saveToCookie(userSettings)
        theme.global.name.value = value
    }
    const currentTheme = computed((): Theme => (theme.global.name.value as Theme))
    return {
        dark,
        currentTheme,
        toggle
    }
}
export {
    ThemeHelper as useTheme
}