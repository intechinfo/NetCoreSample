import * as types from '../mutation-types'

const state = {
    studentList: []
}

const mutations = {
    [types.ADD_STUDENT](state, model) {
        state.studentList.push(model)
    },

    [types.EDIT_STUDENT](state, model) {
        let idx = state.studentList.findIndex( x => x.studentId == model.studentId);

        state.studentList[idx] = model;
    },

    [types.REMOVE_STUDENT](state, studentId) {
        let idx = state.studentList.findIndex( x => x.studentId == studentId);

        state.studentList.splice(idx, 1);
    },

    [types.REFRESH_STUDENT_LIST](state, list) {
        state.studentList = list;
    }
}

export default {
    state,
    mutations
}