import { createRouter, createWebHistory } from 'vue-router';
import TaskList from '../components/TaskList.vue';

const routes = [{ path: '/', component: TaskList }];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});
