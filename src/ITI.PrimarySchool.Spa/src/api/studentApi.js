import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/student";

export async function getStudentListAsync() {
    return await getAsync(endpoint);
}

export async function getStudentAsync(studentId) {
    return await getAsync(`${endpoint}/${studentId}`);
}

export async function createStudentAsync(model) {
    return await postAsync(endpoint, model);
}

export async function updateStudentAsync(model) {
    return await putAsync(`${endpoint}/${model.studentId}`, model);
}

export async function deleteStudentAsync(studentId) {
    return await deleteAsync(`${endpoint}/${studentId}`);
}