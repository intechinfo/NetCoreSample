import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/class";

class ClassApiService {
    constructor() {

    }

    async getClassListAsync() {
        return await getAsync(endpoint);
    }

    async getClassesWithoutTeacherAsync() {
        return await getAsync(`${endpoint}/NotAssigned`);
    }

    async getClassAsync(classId) {
        return await getAsync(`${endpoint}/${classId}`);
    }

    async createClassAsync(model) {
        return await postAsync(endpoint, model);
    }

    async updateClassAsync(model) {
        return await putAsync(`${endpoint}/${model.classId}`, model);
    }

    async deleteClassAsync(classId) {
        return await deleteAsync(`${endpoint}/${classId}`);
    }
}

export default new ClassApiService()