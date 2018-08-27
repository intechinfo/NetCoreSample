// Vue router setup
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import requireAuth from './helpers/requireAuth';

// Components
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Logout from './components/Logout.vue'

import ClassList from './components/classes/ClassList.vue'
import ClassEdit from './components/classes/ClassEdit.vue'

import StudentList from './components/students/StudentList.vue'
import StudentEdit from './components/students/StudentEdit.vue'

import TeacherList from './components/teachers/TeacherList.vue'
import TeacherEdit from './components/teachers/TeacherEdit.vue'
import TeacherAssign from './components/teachers/TeacherAssign.vue'

import FollowingList from './components/github/FollowingList.vue'

const routes = [
    { path: '', component: Home, beforeEnter: requireAuth },
    
    { path: '/login', component: Login },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },

    { path: '/classes', component: ClassList, beforeEnter: requireAuth },
    { path: '/classes/:mode([create|edit]+)/:id?', component: ClassEdit, beforeEnter: requireAuth },

    { path: '/students', component: StudentList, beforeEnter: requireAuth },
    { path: '/students/:mode([create|edit]+)/:id?', component: StudentEdit, beforeEnter: requireAuth },

    { path: '/teachers', component: TeacherList, beforeEnter: requireAuth },
    { path: '/teachers/:mode([create|edit]+)/:id?', component: TeacherEdit, beforeEnter: requireAuth },
    { path: '/teachers/assign/:id', component: TeacherAssign, beforeEnter: requireAuth },

    { path: '/github/following', component: FollowingList, beforeEnter: requireAuth, meta: { requiredProviders: ['GitHub'] } }
];

export default new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});