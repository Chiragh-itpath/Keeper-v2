import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const colors = {
    success: '#198754',
    info: '#0dcaf0',
    warning: '#ffc107',
    danger: '#dc3545',
    primary: '#26A69A',
    'blur-white': '#f7f9fc'
}
export const vuetify = createVuetify({
    components,
    directives,
    theme: {
        defaultTheme: 'light',
        themes: {
            light: {
                dark: false,
                colors: {
                    ...colors,
                    background: '#E0E0E0'
                }
            },
            dark: {
                dark: true,
                colors: {
                    ...colors,
                    background: '#262626'
                }
            }
        }
    }
})

