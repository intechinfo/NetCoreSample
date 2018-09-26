import './main.vendors'
import './main.auth'
import AuthService from './services/AuthService'
import Vue from 'vue'
import App from './components/App.vue'
import router from './main.router'

Vue.config.productionTip = false

const main = async() => {
  await AuthService.init();

  new Vue({
    router,
    render: h => h(App)
  }).$mount('#app')
};

main();