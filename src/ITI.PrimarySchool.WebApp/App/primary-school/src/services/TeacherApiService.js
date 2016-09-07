import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/teachers";

class TeacherApiService {

  constructor(token) {
      this.token = token;
  }

  async getTeacherListAsync() {
      return await getAsync(endpoint, 'GetTeacherList', this.token);
  }

  async getTeacherAsync(teacherId) {
      return await getAsync(endpoint, 'GetTeacherById', this.token, { teacherId: teacherId });
  }

  async createTeacherAsync(model) {
      return await postAsync(endpoint, 'CreateTeacher', this.token, { model: model });
  }

  async updateTeacherAsync(model) {
      return await putAsync(endpoint, 'UpdateTeacher', this.token, { model: model });
  }

  async deleteTeacherAsync(teacherId) {
      return await deleteAsync(endpoint, 'DeleteTeacher', this.token, { teacherId: teacherId });
  }
}

export default TeacherApiService