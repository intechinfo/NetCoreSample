import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/students";

class StudentApiService {

  constructor(token) {
      this.token = token;
  }

  async getStudentListAsync() {
      return await getAsync(endpoint, 'GetStudentList', this.token);
  }

  async getStudentAsync(studentId) {
      return await getAsync(endpoint, 'GetStudentById', this.token, { studentId: studentId });
  }

  async createStudentAsync(model) {
      return await postAsync(endpoint, 'CreateStudent', this.token, { model: model });
  }

  async updateStudentAsync(model) {
      return await putAsync(endpoint, 'UpdateStudent', this.token, { model: model });
  }

  async deleteStudentAsync(studentId) {
      return await deleteAsync(endpoint, 'DeleteStudent', this.token, { studentId: studentId });
  }
}

export default StudentApiService