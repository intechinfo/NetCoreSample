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
        commit(types.ERROR_HAPPENED, `${error.status}: ${error.responseText || error.statusText}`);
    }
    finally {
        commit(types.SET_IS_LOADING, false);
    }

    return result;
}

// Classes
export async function createClass({ commit }, model) {
    return wrapAsyncApiCall(commit, () => ClassApi.createClassAsync(model), (result) => commit(types.ADD_CLASS, result))
}

export async function updateClass({ commit }, model) {
   return wrapAsyncApiCall(commit, () => ClassApi.updateClassAsync(model), (result) => commit(types.EDIT_CLASS, result))
}

export async function deleteClass({ commit }, classId) {
   return wrapAsyncApiCall(commit, () => ClassApi.deleteClassAsync(classId), (result) => commit(types.REMOVE_CLASS, result))
}

export async function refreshClassList({ commit }) {
    return wrapAsyncApiCall(commit, () => ClassApi.getClassListAsync(), (result) => commit(types.REFRESH_CLASS_LIST, result))
}

    // Students
export async function createStudent({ commit }, model) {
    return wrapAsyncApiCall(commit, () => StudentApi.createStudentAsync(model), (result) => commit(types.ADD_STUDENT, result))
}

export async function updateStudent({ commit }, model) {
    return wrapAsyncApiCall(commit, () => StudentApi.updateStudentAsync(model), (result) => commit(types.EDIT_STUDENT, result))
}

export async function deleteStudent({ commit }, studentId) {
    return wrapAsyncApiCall(commit, () => StudentApi.deleteStudentAsync(studentId), (result) => commit(types.REMOVE_STUDENT, result))
}

export async function refreshStudentList({ commit }) {
    return wrapAsyncApiCall(commit, () => StudentApi.getStudentListAsync(), (result) => commit(types.REFRESH_STUDENT_LIST, result))
}

    // Teachers
export async function createTeacher({ commit }, model) {
    return wrapAsyncApiCall(commit, () => TeacherApi.createTeacherAsync(model), (result) => commit(types.ADD_TEACHER, result))
}

export async function updateTeacher({ commit }, model) {
    return wrapAsyncApiCall(commit, () => TeacherApi.updateTeacherAsync(model), (result) => commit(types.EDIT_TEACHER, result))
}

export async function deleteTeacher({ commit }, teacherId) {
    return wrapAsyncApiCall(commit, () => TeacherApi.deleteTeacherAsync(teacherId), (result) => commit(types.REMOVE_TEACHER, result))
}

export async function refreshTeacherList({ commit }) {
    return wrapAsyncApiCall(commit, () => TeacherApi.getTeacherListAsync(), (result) => commit(types.REFRESH_TEACHER_LIST, result))
}