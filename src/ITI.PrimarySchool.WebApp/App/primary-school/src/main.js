import 'babel-polyfill'
import 'jquery'
import 'bootstrap/dist/js/bootstrap'
import Vue from 'vue'
import store from './vuex/store'
import VueRouter from 'vue-router'

import App from './components/App.vue'
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Logout from './components/Logout.vue'

import ClassList from './components/classes/ClassList.vue'
import ClassEdit from './components/classes/ClassEdit.vue'

import StudentList from './components/students/StudentList.vue'
import StudentEdit from './components/students/StudentEdit.vue'

import TeacherList from './components/teachers/TeacherList.vue'
import TeacherEdit from './components/teachers/TeacherEdit.vue'

import AuthService from './services/AuthService'

Vue.use(VueRouter)

function requireAuth (route, redirect, next) {
  if (!AuthService.isConnected) {
    redirect({
      path: '/login',
      query: { redirect: route.fullPath }
    })
  } else {
    next()
  }
}

const router = new VueRouter({
  mode: 'history',
  base: '/Home',
  routes: [
    { path: '/login', component: Login },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },

    { path: '', component: Home, beforeEnter: requireAuth },

    { path: '/classes', component: ClassList, beforeEnter: requireAuth },
    { path: '/classes/:mode([create|edit]+)/:id?', component: ClassEdit, beforeEnter: requireAuth },

    { path: '/students', component: StudentList, beforeEnter: requireAuth },
    { path: '/students/:mode([create|edit]+)/:id?', component: StudentEdit, beforeEnter: requireAuth },

    { path: '/teachers', component: TeacherList, beforeEnter: requireAuth },
    { path: '/teachers/:mode([create|edit]+)/:id?', component: TeacherEdit, beforeEnter: requireAuth }
  ]
})

AuthService.allowedOrigins = ['http://localhost:5000'];
AuthService.loginEndpoint = '/Account/Login';
AuthService.logoutEndpoint = '/Account/Logoff';
AuthService.appRedirect = () => router.replace('/');

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})