import Vue from 'vue'
import Vuex from 'vuex'
import createLogger from 'vuex/logger'
import * as actions from './actions'
import * as getters from './getters'
import sampleModule from './modules/sample-module'

Vue.use(Vuex)

const debug = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({
  actions,
  getters,
  modules: {
      sampleModule
  },
  strict: debug,
  plugins: debug ? [createLogger()] : []
})