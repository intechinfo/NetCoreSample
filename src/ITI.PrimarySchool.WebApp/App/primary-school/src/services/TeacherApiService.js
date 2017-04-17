import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/teacher";

class TeacherApiService {
    constructor() {

    }

    async getTeacherListAsync() {
        return await getAsync(endpoint);
    }

    async getTeacherAsync(teacherId) {
        return await getAsync(`${endpoint}/${teacherId}`);
    }

    async createTeacherAsync(model) {
        return await postAsync(endpoint, model);
    }

    async updateTeacherAsync(model) {
        return await putAsync(`${endpoint}/${model.teacherId}`, model);
    }

    async deleteTeacherAsync(teacherId) {
        return await deleteAsync(`${endpoint}/${teacherId}`);
    }

    async getTeacherAssignedClassAsync(teacherId) {
        return await getAsync(`${endpoint}/${teacherId}/assignedClass`);
    }

    async assignTeacherToclassAsync(teacherId, classId) {
        return await postAsync(`${endpoint}/${teacherId}/assignClass`, { classId: classId });
    }
}

export default new TeacherApiService()