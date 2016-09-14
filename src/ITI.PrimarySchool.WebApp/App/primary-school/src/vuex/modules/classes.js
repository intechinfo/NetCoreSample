import * as types from '../mutation-types'

const state = {
    classList: []
}

const mutations = {
    [types.ADD_CLASS](state, model) {
        state.classList.push(model)
    },

    [types.EDIT_CLASS](state, model) {
        let idx = state.classList.findIndex( x => x.classId == model.classId);

        state.classList[idx] = model;
    },

    [types.REMOVE_CLASS](state, classId) {
        let idx = state.classList.findIndex( x => x.classId == classId);

        state.classList.splice(idx, 1);
    },

    [types.REFRESH_CLASS_LIST](state, list) {
        state.classList = list;
    }
}

export default {
    state,
    mutations
}