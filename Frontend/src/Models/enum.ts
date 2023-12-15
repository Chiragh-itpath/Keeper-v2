enum RouterEnum {
    HOME = 'Home',
    LOGIN = 'Login',
    SIGNUP = 'SignUp',
    VERIFY_EMAIL = 'VerifyEmail',
    VERIFY_OTP = 'VerifyOtp',
    PASSWORD_RESET = 'PasswordReset',
    PROJECT = 'Projects',
    SHARED = 'Shared',
    PROJECT_BY_TAG = 'ProjectByTag',
    KEEP_BY_TAG = 'KeepByTag',
    KEEP = 'Keeps',
    ITEM = 'Items',
    CONTACT = 'Contact',
    PAGE_NOT_FOUND = 'PAGE_NOT_FOUND'
}

enum ItemType {
    TICKET,
    PR
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

enum TagTypeEnum {
    PROJECT,
    KEEP
}

enum NoRecord {
    Empty,
    NotFound
}
type Colors = 'success' | 'info' | 'warning' | 'danger' | 'primary' | 'secondary'
export { RouterEnum, ItemType, StatusType, TagTypeEnum, NoRecord, type Colors }
