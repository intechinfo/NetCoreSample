import * as types from '../mutation-types'

const state = {
    isLoading: false,
    errors: []
}

const mutations = {
    [types.SET_IS_LOADING](state, isLoading) {
        state.isLoading = isLoading
    },

    [types.ERROR_HAPPENED](state, error) {
        state.errors.push(error || "")
    }
}

export default {
    state,
    mutations
}