import 'babel-polyfill'
import 'jquery'
import 'bootstrap/dist/js/bootstrap'
import Vue from 'vue'
import store from './vuex/store'
import VueRouter from 'vue-router'

import App from './components/App.vue'
import Home from './components/Home.vue'

import ClassList from './components/classes/ClassList.vue'
import ClassEdit from './components/classes/ClassEdit.vue'

import StudentList from './components/students/StudentList.vue'
import StudentEdit from './components/students/StudentEdit.vue'

import TeacherList from './components/teachers/TeacherList.vue'
import TeacherEdit from './components/teachers/TeacherEdit.vue'

import { refreshClassList, refreshStudentList, refreshTeacherList } from './vuex/actions'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  base: '/Home/SinglePageApp',
  routes: [
    { path: '', component: Home },

    { path: '/classes', component: ClassList },
    { path: '/classes/:mode([create|edit]+)/:id?', component: ClassEdit },

    { path: '/students', component: StudentList },
    { path: '/students/:mode([create|edit]+)/:id?', component: StudentEdit },

    { path: '/teachers', component: TeacherList },
    { path: '/teachers/:mode([create|edit]+)/:id?', component: TeacherEdit }
  ]
})

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})

refreshClassList(store)
refreshStudentList(store)
refreshTeacherList(store)