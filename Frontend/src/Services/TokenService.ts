const setToken = (token: string): void => {
    const date = new Date()
    date.setHours(date.getHours() + 8)
    document.cookie = `token=${token};expires=${date.toUTCString()} ; path=/;`
}
const removeToken = (): void => {
    document.cookie = `token=; expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`
}
const getToken = (): string | undefined => {
    const cookies = document.cookie.split(';')
    for (const cookie of cookies) {
        const [cookieName, cookieValue] = cookie.split('=').map((c) => c.trim())
        if (cookieName === 'token') {
            return cookieValue
        }
    }
    return undefined
}
export { setToken, removeToken, getToken }
