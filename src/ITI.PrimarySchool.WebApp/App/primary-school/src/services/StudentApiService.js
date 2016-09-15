import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/student";

class StudentApiService {
    constructor() {

    }

    async getStudentListAsync() {
        return await getAsync(endpoint, '', AuthService.accessToken);
    }

    async getStudentAsync(studentId) {
        return await getAsync(endpoint, studentId, AuthService.accessToken);
    }

    async createStudentAsync(model) {
        return await postAsync(endpoint, '', AuthService.accessToken, model);
    }

    async updateStudentAsync(model) {
        return await putAsync(endpoint, model.studentId, AuthService.accessToken, model);
    }

    async deleteStudentAsync(studentId) {
        return await deleteAsync(endpoint, studentId, AuthService.accessToken);
    }
}

export default new StudentApiService()