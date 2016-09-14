import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/teacher";

class TeacherApiService {

  constructor(token) {
      this.token = token;
  }

  async getTeacherListAsync() {
      return await getAsync(endpoint, '', this.token);
  }

  async getTeacherAsync(teacherId) {
      return await getAsync(endpoint, teacherId, this.token);
  }

  async createTeacherAsync(model) {
      return await postAsync(endpoint, '', this.token, model);
  }

  async updateTeacherAsync(model) {
      return await putAsync(endpoint, model.teacherId, this.token, model);
  }

  async deleteTeacherAsync(teacherId) {
      return await deleteAsync(endpoint, teacherId, this.token);
  }
}

export default TeacherApiService