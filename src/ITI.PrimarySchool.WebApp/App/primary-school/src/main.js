import 'babel-polyfill'
import 'jquery'
import 'bootstrap/dist/js/bootstrap'
import Vue from 'vue'
import store from './vuex/store'
import VueRouter from 'vue-router'

import App from './components/App.vue'
import Home from './components/Home.vue'
import ClassList from './components/classes/ClassList.vue'
import StudentList from './components/students/StudentList.vue'
import TeacherList from './components/teachers/TeacherList.vue'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  base: '/Home/SinglePageApp',
  routes: [
    { path: '', component: Home },
    { path: '/classes', component: ClassList },
    { path: '/students', component: StudentList },
    { path: '/teachers', component: TeacherList }
  ]
})

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})