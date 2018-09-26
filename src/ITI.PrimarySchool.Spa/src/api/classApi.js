import {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync
} from "../helpers/apiHelper";

const endpoint = process.env.VUE_APP_BACKEND + "/api/class";

export async function getClassListAsync() {
    return await getAsync(endpoint);
}

export async function getClassesWithoutTeacherAsync() {
    return await getAsync(`${endpoint}/NotAssigned`);
}

export async function getClassAsync(classId) {
    return await getAsync(`${endpoint}/${classId}`);
}

export async function createClassAsync(model) {
    return await postAsync(endpoint, model);
}

export async function updateClassAsync(model) {
    return await putAsync(`${endpoint}/${model.classId}`, model);
}

export async function deleteClassAsync(classId) {
    return await deleteAsync(`${endpoint}/${classId}`);
}
