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
}

export default new TeacherApiService()