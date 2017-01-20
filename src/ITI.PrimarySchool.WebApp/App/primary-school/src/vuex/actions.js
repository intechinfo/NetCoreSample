import * as types from './mutation-types'

import ClassApi from '../services/ClassApiService'
import StudentApi from '../services/StudentApiService'
import TeacherApi from '../services/TeacherApiService'

// Wraps the async call to an api service in order to handle loading, and errors.
async function wrapAsyncApiCall(commit, apiCall, rethrowError) {
    commit(types.SET_IS_LOADING, true);

    let result = null;

    try {
        return await apiCall();
    }
    catch (error) {
        commit(types.ERROR_HAPPENED, `${error.status}: ${error.responseText || error.statusText}`);
        
        if(rethrowError) throw error;
    }
    finally {
        commit(types.SET_IS_LOADING, false);
    }
}

// Generic request
export async function requestAsync({ commit }, action, rethrowError) {
    return await wrapAsyncApiCall(commit, action, rethrowError);
}

// Classes
export async function createClass({ commit }, model) {
    var result = await wrapAsyncApiCall(commit, () => ClassApi.createClassAsync(model));
    if(result) commit(types.ADD_CLASS, result);
    return result;
}

export async function updateClass({ commit }, model) {
   var result = await wrapAsyncApiCall(commit, () => ClassApi.updateClassAsync(model));
   if(result) commit(types.EDIT_CLASS, result);
   return result;
}

export async function deleteClass({ commit }, classId) {
    var result = await wrapAsyncApiCall(commit, () => ClassApi.deleteClassAsync(classId));
    if(result) commit(types.REMOVE_CLASS, result);
    return result;
}

export async function refreshClassList({ commit }) {
    var result = await wrapAsyncApiCall(commit, () => ClassApi.getClassListAsync());
    if(result) commit(types.REFRESH_CLASS_LIST, result);
    return result;
}

// Students
export async function createStudent({ commit }, model) {
    var result = await wrapAsyncApiCall(commit, () => StudentApi.createStudentAsync(model));
    if(result) commit(types.ADD_STUDENT, result);
    return result;
}

export async function updateStudent({ commit }, model) {
    var result = await wrapAsyncApiCall(commit, () => StudentApi.updateStudentAsync(model));
    if(result) commit(types.EDIT_STUDENT, result);
    return result;
}

export async function deleteStudent({ commit }, studentId) {
    var result = await wrapAsyncApiCall(commit, () => StudentApi.deleteStudentAsync(studentId));
    if(result) commit(types.REMOVE_STUDENT, result)
    return result;
}

export async function refreshStudentList({ commit }) {
    var result = await wrapAsyncApiCall(commit, () => StudentApi.getStudentListAsync());
    if(result) commit(types.REFRESH_STUDENT_LIST, result);
    return result;
}

// Teachers
export async function createTeacher({ commit }, model) {
    var result = await wrapAsyncApiCall(commit, () => TeacherApi.createTeacherAsync(model));
    if(result) commit(types.ADD_TEACHER, result);
    return result;
}

export async function updateTeacher({ commit }, model) {
    var result = await wrapAsyncApiCall(commit, () => TeacherApi.updateTeacherAsync(model));
    if(result) commit(types.EDIT_TEACHER, result);
    return result;
}

export async function deleteTeacher({ commit }, teacherId) {
    var result = await wrapAsyncApiCall(commit, () => TeacherApi.deleteTeacherAsync(teacherId));
    if(result) commit(types.REMOVE_TEACHER, result);
    return result;
}

export async function refreshTeacherList({ commit }) {
    var result = await wrapAsyncApiCall(commit, () => TeacherApi.getTeacherListAsync());
    if(result) commit(types.REFRESH_TEACHER_LIST, result);
    return result;
}