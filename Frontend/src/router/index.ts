import { createRouter, createWebHistory } from 'vue-router'
import type {
    RouteRecordRaw,
    RouteLocationNormalized,
    NavigationGuardNext,
    Router
} from 'vue-router'
import { RouterEnum } from '@/Models/enum'
import SideBar from '@/components/SideBar.vue'
import NavBar from '@/components/NavBar.vue'
import { getToken } from '@/Services/TokenService'

import {
    ForgotPasswordPage,
    HomePage,
    ItemPage,
    KeepPage,
    LoginPage,
    PageNotFound,
    ProjectPage,
    SignUpPage,
    ContactPage
} from '@/pages'

const IsLoggedIn = (): boolean => {
    const token = getToken()
    return token != ''
}

const requireLoggedIn = (
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext
) => {
    if (IsLoggedIn()) {
        next()
    } else {
        next({ name: RouterEnum.LOGIN })
    }
}
const routes: RouteRecordRaw[] = [
    {
        path: '/',
        component: HomePage,
        name: RouterEnum.HOME
    },
    {
        path: '/login',
        component: LoginPage,
        name: RouterEnum.LOGIN
    },
    {
        path: '/signup',
        component: SignUpPage,
        name: RouterEnum.SIGNUP
    },
    {
        path: '/PasswordReset',
        component: ForgotPasswordPage,
        name: RouterEnum.PASSWORD_RESET
    },
    {
        path: '/:pathMatch(.*)*',
        components: { default: PageNotFound },
        name: RouterEnum.PAGE_NOT_FOUND
    },
    {
        path: '/Project/',
        name: RouterEnum.PROJECT,
        components: {
            default: ProjectPage,
            NavBar: NavBar,
            SideBar: SideBar
        },
        beforeEnter: requireLoggedIn
    },
    {
        path: '/Project/:id',
        name: RouterEnum.KEEP,
        components: {
            default: KeepPage,
            NavBar: NavBar,
            SideBar: SideBar
        },
        beforeEnter: requireLoggedIn
    },
    {
        path: '/Project/:id/Keep/:keepId',
        components: {
            default: ItemPage,
            NavBar: NavBar,
            SideBar: SideBar
        },
        name: RouterEnum.ITEM,
        beforeEnter: requireLoggedIn
    },
    {
        path: '/Contact',
        components: {
            default: ContactPage,
            NavBar: NavBar,
            SideBar: SideBar
        },
        name: RouterEnum.CONTACT,
        beforeEnter: requireLoggedIn
    }
]
const router: Router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export { router }
