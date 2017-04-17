import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/student";

class StudentApiService {
    constructor() {

    }

    async getStudentListAsync() {
        return await getAsync(endpoint);
    }

    async getStudentAsync(studentId) {
        return await getAsync(`${endpoint}/${studentId}`);
    }

    async createStudentAsync(model) {
        return await postAsync(endpoint, model);
    }

    async updateStudentAsync(model) {
        return await putAsync(`${endpoint}/${model.studentId}`, model);
    }

    async deleteStudentAsync(studentId) {
        return await deleteAsync(`${endpoint}/${studentId}`);
    }
}

export default new StudentApiService()