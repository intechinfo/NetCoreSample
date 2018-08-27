import './main.vendors'
import './main.auth'
import AuthService from './services/AuthService'
import Vue from 'vue'
import App from './components/App.vue'
import router from './main.router'
import store from './store'

Vue.config.productionTip = false

const main = async() => {
  await AuthService.init();

  new Vue({
    router,
    store,
    render: h => h(App)
  }).$mount('#app')
};

main();