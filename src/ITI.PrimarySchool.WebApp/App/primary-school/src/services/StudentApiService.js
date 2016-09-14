import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/student";

class StudentApiService {

  constructor(token) {
      this.token = token;
  }

  async getStudentListAsync() {
      return await getAsync(endpoint, '', this.token);
  }

  async getStudentAsync(studentId) {
      return await getAsync(endpoint, studentId, this.token);
  }

  async createStudentAsync(model) {
      return await postAsync(endpoint, '', this.token, model);
  }

  async updateStudentAsync(model) {
      return await putAsync(endpoint, model.studentId, this.token, model);
  }

  async deleteStudentAsync(studentId) {
      return await deleteAsync(endpoint, studentId, this.token);
  }
}

export default StudentApiService