import './main.polyfills'
import './main.vendors'
import './main.auth'

import Vue from 'vue'
import store from './store'
import router from './main.router'
import App from './components/App.vue'

// Creation of the root Vue of the application
new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
});