import Vue from 'vue'
import Vuex from 'vuex'
import createLogger from 'vuex/logger'

import * as actions from './actions'
import * as getters from './getters'

import app from './modules/app'
import classes from './modules/classes'
import students from './modules/students'
import teachers from './modules/teachers'

Vue.use(Vuex)

const debug = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({
  actions,
  getters,
  modules: {
      app,
      classes,
      students,
      teachers
  },
  strict: debug,
  plugins: debug ? [createLogger()] : []
})