import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/teacher";

export async function getTeacherListAsync() {
    return await getAsync(endpoint);
}

export async function getTeacherAsync(teacherId) {
    return await getAsync(`${endpoint}/${teacherId}`);
}

export async function createTeacherAsync(model) {
    return await postAsync(endpoint, model);
}

export async function updateTeacherAsync(model) {
    return await putAsync(`${endpoint}/${model.teacherId}`, model);
}

export async function deleteTeacherAsync(teacherId) {
    return await deleteAsync(`${endpoint}/${teacherId}`);
}

export async function getTeacherAssignedClassAsync(teacherId) {
    return await getAsync(`${endpoint}/${teacherId}/assignedClass`);
}

export async function assignTeacherToclassAsync(teacherId, classId) {
    return await postAsync(`${endpoint}/${teacherId}/assignClass`, { classId: classId });
}