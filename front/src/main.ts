import './assets/scss/main.scss'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'


import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { useAuthStore } from './stores/auth'

const app = createApp(App)

const pinia = createPinia()
app.use(pinia)

const auth = useAuthStore()
await auth.initialize()

app.use(router)

app.mount('#app')
