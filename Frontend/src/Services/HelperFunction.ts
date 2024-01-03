const dateDiffernce = (dateArg: string) => {
    const inputDate = new Date(dateArg)
    if (isNaN(inputDate.getTime())) {
        return 'Invalid date'
    }

    const currentDate = new Date()
    const timeDifference = currentDate.getTime() - inputDate.getTime()

    const milliseconds = Math.abs(timeDifference)
    const seconds = Math.floor(milliseconds / 1000)
    const minutes = Math.floor(seconds / 60)
    const hours = Math.floor(minutes / 60)
    const days = Math.floor(hours / 24)

    if (days > 0) {
        return `${days} day(s) ago`
    } else if (hours > 0) {
        return `${hours} hour(s) ago`
    } else if (minutes > 0) {
        return `${minutes} minute(s) ago`
    } else if (seconds > 0) {
        return `${seconds} second(s) ago`
    } else {
        return 'Just now'
    }
}

export { dateDiffernce }
