const requiredRule = (val: string | null): boolean | string =>
    val == null || val.trim() == '' ? 'This field is required!' : true

const emailRules = (val: string): boolean | string =>
    /^\w+([\\.-]?\w+)*@\w+([\\.-]?\w+)*(\.\w{2,3})+$/.test(val.trim())
        ? true
        : 'E-mail must be valid.'

const passwordRule = (val: string): boolean | string =>
    val.trim().length < 8 ? 'At least 8 characters!' : true

const contactRules = (val: string): boolean | string =>
    /^[0-9]{10}$/.test(val.trim()) || val == '' ? true : 'Contact number must be valid!'

const urlRules = (val: string): boolean | string => {
    return val == null ||
        val.trim() === '' ||
        /^(https?|ftp|ftps?|ws|wss):\/\/([a-zA-Z0-9-]+\.?)+[^\s/$.?#].[^\s]*$/.test(val)
        ? true
        : 'URL must be valid'
}
const acceptedFileTypes: string[] = [
    'image/png',
    'image/jpeg',
    'image/pjpeg',
    'image/bmp',
    'application/pdf',
    'application/json',
    'application/msword',
    'application/doc',
    'application/ms-doc',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    'application/excel',
    'application/vnd.ms-excel',
    'application/x-excel',
    'application/x-msexcel',
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    'application/mspowerpoint',
    'application/powerpoint',
    'application/vnd.ms-powerpoint',
    'application/x-mspowerpoint',
    'application/vnd.openxmlformats-officedocument.presentationml.presentation',
    'text/csv',
    'text/xml'
];
const fileRule = (files: File[] | null): boolean | string => {
    if (files) {
        const sizeCheck = files.every(x => x.size < 5000000)
        if (!sizeCheck) return 'Files must be under 5 MB!'
        const typeCheck = files.every(x => acceptedFileTypes.includes(x.type))
        if (!typeCheck) return 'Invalid file type'
    }
    return true
}
export { requiredRule, emailRules, passwordRule, contactRules, urlRules, fileRule }
