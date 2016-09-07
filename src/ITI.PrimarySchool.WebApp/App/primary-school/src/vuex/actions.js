import * as types from './mutation-types'
import { ClassApi, TeacherApi, StudentApi } from '../services'

// Wraps the async call to an api service in order to handle loading, and errors.
async function wrapAsyncApiCall(commit, apiCall, success) {
    commit(types.SET_IS_LOADING, true);

    let result = null;

    try {
        result = await apiCall();
        success(result);
    }
    catch (error) {
        commit(types.ERROR_HAPPENED, `${error.status}: ${error.statusText}`);
    }
    finally {
        commit(types.SET_IS_LOADING, false);
    }

    return result;
}

// Classes
export async function createClass({ commit }, model) {
    return wrapAsyncApiCall(commit, () => ClassApi.createClassAsync(model), (result) => commit('ADD_CLASS', result))
}

export async function updateClass({ commit }, model) {
   return wrapAsyncApiCall(commit, () => ClassApi.updateClassAsync(model), (result) => commit('EDIT_CLASS', result))
}

export async function deleteClass({ commit }, classId) {
   return wrapAsyncApiCall(commit, () => ClassApi.deleteClassAsync(classId), (result) => commit('REMOVE_CLASS', result))
}

// Students
export async function createStudent({ commit }, model) {
    return wrapAsyncApiCall(commit, () => StudentApi.createStudentAsync(model), (result) => commit('ADD_STUDENT', result))
}

export async function updateStudent({ commit }, model) {
   return wrapAsyncApiCall(commit, () => StudentApi.updateStudentAsync(model), (result) => commit('EDIT_STUDENT', result))
}

export async function deleteStudent({ commit }, studentId) {
   return wrapAsyncApiCall(commit, () => StudentApi.deleteStudentAsync(studentId), (result) => commit('REMOVE_STUDENT', result))
}

// Teachers
export async function createTeacher({ commit }, model) {
    return wrapAsyncApiCall(commit, () => TeacherApi.createTeacherAsync(model), (result) => commit('ADD_TEACHER', result))
}

export async function updateTeacher({ commit }, model) {
   return wrapAsyncApiCall(commit, () => TeacherApi.updateTeacherAsync(model), (result) => commit('EDIT_TEACHER', result))
}

export async function deleteTeacher({ commit }, teacherId) {
   return wrapAsyncApiCall(commit, () => TeacherApi.deleteTeacherAsync(teacherId), (result) => commit('REMOVE_TEACHER', result))
}