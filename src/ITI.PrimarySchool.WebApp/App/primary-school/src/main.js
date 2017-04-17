import 'babel-polyfill'
import $ from 'jquery'
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
import TeacherAssign from './components/teachers/TeacherAssign.vue'

import FollowingList from './components/github/FollowingList.vue'

import AuthService from './services/AuthService'

Vue.use(VueRouter)

/**
 * Filter for routes requiring an authenticated user
 * @param {*} to 
 * @param {*} from 
 * @param {*} next 
 */
function requireAuth (to, from, next)  {
  if (!AuthService.isConnected) {
    next({
      path: '/login',
      query: { redirect: to.fullPath }
    });

    return;
  }

  var requiredProviders = to.meta.requiredProviders;

  if(requiredProviders && !AuthService.isBoundToProvider(requiredProviders)) {
    next( false )
  };

  next();
}

/**
 * Declaration of the different routes of our application, and the corresponding components
 */
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
    { path: '/teachers/:mode([create|edit]+)/:id?', component: TeacherEdit, beforeEnter: requireAuth },
    { path: '/teachers/assign/:id', component: TeacherAssign, beforeEnter: requireAuth },

    { path: '/github/following', component: FollowingList, beforeEnter: requireAuth, meta: { requiredProviders: ['GitHub'] } }
  ]
})

/**
 * Configuration of the authentication service
 */

// Allowed urls to access the application (if your website is http://mywebsite.com, you have to add it)
AuthService.allowedOrigins = ['http://localhost:5000', /* 'http://mywebsite.com' */];

// Server-side endpoint to logout
AuthService.logoutEndpoint = '/Account/LogOff';

// Allowed providers to log in our application, and the corresponding server-side endpoints
AuthService.providers = {
  'Base': {
    endpoint: '/Account/Login'
  },
  'Google': {
    endpoint: '/Account/ExternalLogin?provider=Google'
  },
  'GitHub': {
    endpoint: '/Account/ExternalLogin?provider=GitHub'
  },
};

AuthService.appRedirect = () => router.replace('/');

// Creation of the root Vue of the application
new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})