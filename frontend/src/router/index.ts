import { createRouter, createWebHashHistory } from 'vue-router'
import NotesView from '../views/NotesView.vue'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    { path: '/', component: NotesView },
  ],
})

export default router