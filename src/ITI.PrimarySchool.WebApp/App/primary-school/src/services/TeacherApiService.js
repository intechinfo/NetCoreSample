import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/teacher";

class TeacherApiService {
    constructor() {

    }

    async getTeacherListAsync() {
        return await getAsync(endpoint, '', AuthService.accessToken);
    }

    async getTeacherAsync(teacherId) {
        return await getAsync(endpoint, teacherId, AuthService.accessToken);
    }

    async createTeacherAsync(model) {
        return await postAsync(endpoint, '', AuthService.accessToken, model);
    }

    async updateTeacherAsync(model) {
        return await putAsync(endpoint, model.teacherId, AuthService.accessToken, model);
    }

    async deleteTeacherAsync(teacherId) {
        return await deleteAsync(endpoint, teacherId, AuthService.accessToken);
    }

    async getTeacherAssignedClassAsync(teacherId) {
        return await getAsync(endpoint, `${teacherId}/assignedClass`, AuthService.accessToken);
    }

    async assignTeacherToclassAsync(teacherId, classId) {
        return await postAsync(endpoint, `${teacherId}/assignClass`, AuthService.accessToken, { classId: classId });
    }
}

export default new TeacherApiService()