const requiredRule = (val: string | null): boolean | string =>
    val == null || val.trim() == '' ? 'This field is required!' : true

const emailRules = (val: string): boolean | string =>
    /^\w+([\\.-]?\w+)*@\w+([\\.-]?\w+)*(\.\w{2,3})+$/.test(val.trim())
        ? true
        : 'E-mail must be valid.'

const passwordRule = (val: string): boolean | string =>
    val.trim().length < 8 ? 'At least 8 characters!' : true

const contactRules = (val: string): boolean | string =>
    /^[0-9]{10}$/.test(val.trim()) || val == '' ? true : 'Contact number must be valid'

const urlRules = (val: string): boolean | string => {
    return val == null ||
        val.trim() === '' ||
        /^(https?|ftp|ftps?|ws|wss):\/\/([a-zA-Z0-9-]+\.?)+[^\s/$.?#].[^\s]*$/.test(val)
        ? true
        : 'URL must be valid'
}
export { requiredRule, emailRules, passwordRule, contactRules, urlRules }
