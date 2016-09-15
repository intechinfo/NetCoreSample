import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/class";

class ClassApiService {
    constructor() {

    }

    async getClassListAsync() {
        return await getAsync(endpoint, '', AuthService.accessToken);
    }

    async getClassAsync(classId) {
        return await getAsync(endpoint, classId, AuthService.accessToken);
    }

    async createClassAsync(model) {
        return await postAsync(endpoint, '', AuthService.accessToken, model);
    }

    async updateClassAsync(model) {
        return await putAsync(endpoint, model.classId, AuthService.accessToken, model);
    }

    async deleteClassAsync(classId) {
        return await deleteAsync(endpoint, classId, AuthService.accessToken);
    }
}

export default new ClassApiService()