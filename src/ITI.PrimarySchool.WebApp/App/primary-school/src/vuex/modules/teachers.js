import * as types from '../mutation-types'

const state = {
    teacherList: []
}

const mutations = {
    [types.ADD_TEACHER](state, model) {
        state.teacherList.push(model)
    },

    [types.EDIT_TEACHER](state, model) {
        let idx = state.teacherList.findIndex( x => x.teacherId == model.teacherId);

        state.teacherList[idx] = model;
    },

    [types.REMOVE_TEACHER](state, teacherId) {
        let idx = state.teacherList.findIndex( x => x.teacherId == teacherId);

        state.teacherList.splice(idx, 1);
    }
}

export default {
    state,
    mutations
}