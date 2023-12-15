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

export { RouterEnum, StatusType }
