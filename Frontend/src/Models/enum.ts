enum RouterEnum {
    HOME = 'Home',
    LOGIN = 'Login',
    SIGNUP = 'SignUp',
    PASSWORD_RESET = 'PasswordReset',
    PROJECT = 'Projects',
    KEEP = 'Keeps',
    ITEM = 'Items',
    CONTACT = 'Contact',
    PAGE_NOT_FOUND = 'PAGE_NOT_FOUND'
}

const enum StatusType {
    SUCCESS,
    ALREADY_EXISTS,
    NOT_FOUND,
    UNAUTHORISED,
    NOT_VALID,
    EMAIL_NOT_FOUND,
    EMAIL_EXISTS,
    PASSWORD_NOT_MATCHED,
    INTERNAL_SERVER_ERROR
}

const enum Permission {
    VIEW,
    EDIT,
    CREATE,
    ALL
}

const enum ItemStatus {
    NEW,
    PENDING,
    FOLLOW_UP,
    COMPLETED,
    RE_OPEN
}
const enum ItemType {
    TICKET,
    PR
}

export {
    RouterEnum,
    StatusType,
    Permission,
    ItemStatus,
    ItemType
}
