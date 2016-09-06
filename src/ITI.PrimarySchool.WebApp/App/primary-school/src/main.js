import 'babel-polyfill'
import 'jquery'
import 'bootstrap/dist/js/bootstrap'
import Vue from 'vue'
import store from './vuex/store'
import App from './components/App.vue'

new Vue({
  el: '#app',
  store,
  render: h => h(App)
})