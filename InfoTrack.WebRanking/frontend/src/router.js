import { createRouter, createWebHistory } from 'vue-router';
import SearchComponent from './components/SearchComponent.vue';
import HistoryComponent from './components/HistoryComponent.vue';

const routes = [
    {
      path: '/',
      name: 'search',
      component: SearchComponent
    },
    {
      path: '/history',
      name: 'history',
      component: HistoryComponent
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
